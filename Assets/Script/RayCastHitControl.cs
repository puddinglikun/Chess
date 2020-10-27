using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastHitControl : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    GameObject obj;

    GridControl activeGrid;
    int activeMark = 0;
    Dictionary<GridControl, Direction> saveGrid;
    GridControl lastTouchGrid;  // 上一次点击的格子

    public GameObject showText;
    Text teamText;
    Text hpText;

    int repeatedJumpMark = 0;   // 连续跳动标记
    int moveEndMark = 1;        // 移动过后就标记为0

    Team teamTurnMark = Team.RedTeam;   // 队伍回合标记
    int teamNumber = 1;                 // 队伍每个回合行动两次
    bool gameBeginMark = true;              // 游戏刚开始的标记

    public Text turnText;   // 队伍回合说明文本

    // Start is called before the first frame update
    void Start()
    {
        teamText = showText.transform.Find("Team").GetComponent<Text>();
        hpText = showText.transform.Find("HP").GetComponent<Text>();
        showText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckClick();

    }

    void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("点击鼠标左键");

            int clickBoxMark = 0;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("hit 3D box");
                //Debug.Log(hit.collider.gameObject.name);
                obj = hit.collider.gameObject;
                //Debug.Log(obj.transform.position);
                clickBoxMark = 1;
       
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2 = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit2.collider != null & repeatedJumpMark == 0 & moveEndMark == 1)
            {
                // 点击格子状态
                var grid = hit2.collider.gameObject.GetComponent<GridControl>();
                
                if (clickBoxMark == 1)
                {
                    
                    // 检测是否这个棋子的回合
                    if (!grid.ContentChess.CompareTeam(teamTurnMark))
                    {
                        return;
                    }

                    // 若两次点击同一个格子
                    if (lastTouchGrid == grid & activeMark == 1)
                    {
                        activeGrid.CancelTouch(saveGrid);
                        activeMark = 0;
                        showText.SetActive(false);
                        return;
                    }
                    lastTouchGrid = grid;

                    // 同时点击到棋子
                    if (activeMark == 0)
                    {
                        // 处于未激活的状态
                        activeMark = 1;
                        activeGrid = grid;
                        saveGrid = grid.TouchColor();
                    } else
                    {
                        // 已经在有棋子格子 激活状态 点其他的棋子
                        activeGrid.CancelTouch(saveGrid);
                        activeGrid = grid;
                        
                        saveGrid = grid.TouchColor();

                    }
                } else
                {
                    // 点击没有棋子的格子
                    if (grid.GetColor() == ColorState.Neighbor)
                    {
                        // 状态为可跳动的格子

                        // 移动棋子
                        bool jumped = MovingChess(grid);
                        
                        // 连跳状态的标记 设置为1
                        repeatedJumpMark = jumped ? 1 : 0;
                        // 只是平移后就锁定
                        moveEndMark = 0;
                    }
                    else
                    {
                        // 点击没棋子 也不是可跳的格子
                        if (activeGrid)
                        {
                            activeGrid.CancelTouch(saveGrid);
                            activeMark = 0;
                        }
                        showText.SetActive(false);
                    }
                } 
            }
            else if (hit2.collider == null & clickBoxMark == 0 & repeatedJumpMark == 0)
            {
                // 若没有点击到棋子也没有点到格子
                if (activeGrid)
                {
                    activeGrid.CancelTouch(saveGrid);
                    activeMark = 0;
                }
                showText.SetActive(false);

            }
            else if (hit2.collider != null & repeatedJumpMark == 1)
            {
                // 点到格子 并且进入连续跳状态

                var grid = hit2.collider.gameObject.GetComponent<GridControl>();

                // 点到允许连跳的格子
                if (grid.GetColor() == ColorState.Neighbor)
                {
                    // 移动棋子
                    MovingChess(grid);

                }
            }

            if (hit2.collider != null & clickBoxMark == 1)
            {
                var grid = hit2.collider.gameObject.GetComponent<GridControl>();
                // 显示棋子文本框
                ShowText(grid.ContentChess);
            }
        }
    }


    void ShowText(ChessControl chess)
    {
        int hp = chess.GetHp();
        Team team = chess.GetTeam();
        hpText.text = "血量：" + hp.ToString();
        string teamWord = team == Team.RedTeam ? "红队" : "蓝队";
        teamText.text = "队伍：" + teamWord;

        showText.SetActive(true);
    }

    public void TimeEnding()
    {
        repeatedJumpMark = 0;
        moveEndMark = 1;
        if (activeGrid)
        {
            activeGrid.CancelTouch(saveGrid);
            activeMark = 0;
        }
        showText.SetActive(false);

        if (gameBeginMark)
        {
            teamNumber = 3;
            gameBeginMark = false;
        }

        teamNumber++;
        
        if (teamNumber > 2)
        {
            teamNumber = 1;
            teamTurnMark = teamTurnMark == Team.RedTeam ? Team.BlueTeam : Team.RedTeam;
            TurnTeam();
        } else
        {
            UpdateTurnText();
        }
        
    }

    public bool MovingChess(GridControl grid)
    {
        // 移动棋子
        activeGrid.CancelTouch(saveGrid);

        ChessControl chessItem = activeGrid.ContentChess;
        chessItem.transform.position = grid.transform.position;

        activeGrid.ContentChess = null;
        grid.ContentChess = chessItem;

        // 寻找途中有无棋子 有且不同队可以扣血
        Direction direction = saveGrid[grid];
        GridControl gridTmp = activeGrid.neighbors[(int)direction];
        bool jumped = false;
        while (gridTmp != grid)
        {
            jumped = true;
            if (gridTmp.CheckChess())
            {
                if (!gridTmp.ContentChess.CompareTeam(chessItem.GetTeam()))
                {
                    gridTmp.ContentChess.ReduceHP();
                }
            }
            gridTmp = gridTmp.neighbors[(int)direction];
        }

        if (jumped)
        {
            saveGrid = grid.RepeatedJump(activeGrid);
        }
        
        activeGrid = grid;

        return jumped;
    }

    void TurnTeam()
    {
        UpdateTurnText();
        turnText.color = teamTurnMark == Team.RedTeam ? Color.red : Color.blue;
    }

    void UpdateTurnText()
    {
        string textTmp = teamTurnMark == Team.RedTeam ? "红队回合" : "蓝队回合";
        turnText.text = textTmp + teamNumber.ToString();
    }
}
