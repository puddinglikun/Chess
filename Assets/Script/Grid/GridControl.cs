using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NE, E, SE, SW, W, NW
}

public static class DirecionExtensions
{
    public static Direction Opposite(this Direction direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    public static Direction Previous(this Direction direction)
    {
        return direction == Direction.NE ? Direction.NW : (direction - 1);
    }

    public static Direction Next(this Direction direction)
    {
        return direction == Direction.NW ? Direction.NE : (direction + 1);
    }
}

public enum ColorState
{
    Default, Neighbor, Choose
}

public enum SearchWay
{
    SearchAll, SearchJump
}

/// <summary>
/// 格子控制脚本
/// </summary>
public class GridControl : MonoBehaviour
{

    SpriteRenderer spriteRender;
    public Color chooseColor;
    public Color neighborColor;
    public Color defaultColor;

    ColorState colorState = ColorState.Default;

    public void SetCoordinate(int column, int row)
    {
        Column = column;
        Row = row;
    }
    public int Column { get; set; }
    public int Row { get; set; }
    public int Content { get; set; }
    public int Jumped { get; set; }

    public ChessControl ContentChess { get; set; }

    [SerializeField]
    public GridControl[] neighbors = new GridControl[6];

    public GridControl GetNeighbor(Direction direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(Direction direction, GridControl cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        //spriteRender.color = new Color(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        //Debug.Log("click grid");
        //Debug.Log(gameObject.name);
        //TouchColor();
    }

    public Dictionary<GridControl, Direction> TouchColor()
    {
        ChangeColor(2);
        Dictionary<GridControl, Direction> waitGrid = ShowNeigbhorColor(SearchWay.SearchAll);
        ActiveColor(waitGrid, 1);
        return waitGrid;
    }

    public Dictionary<GridControl, Direction> RepeatedJump(GridControl jumpedGrid)
    {
        // 需要排除跳过的格子

        Dictionary<GridControl, Direction> waitGrid = ShowNeigbhorColor(SearchWay.SearchJump, jumpedGrid);
        if (waitGrid.Count != 0)
        {
            ChangeColor(2);
            ActiveColor(waitGrid, 1);
        }

        return waitGrid;
    }

    public void CancelTouch()
    {
        // 弃用
        ChangeColor(0);
        ShowNeigbhorColor(SearchWay.SearchAll);
    }

    public void CancelTouch(Dictionary<GridControl, Direction> saveGrid)
    {
        ChangeColor(0);
        foreach (var g in saveGrid.Keys)
        {
            g.ChangeColor(0);
        }
    }

    Dictionary<GridControl, Direction> ShowNeigbhorColor(SearchWay way, GridControl jumpedGrid = null)
    {
        // 存储找到的方格
        Dictionary<GridControl, Direction> saveGrid = new Dictionary<GridControl, Direction>();

        // 先搜索是否空格 是 存储 并开始计数
        for (int i = 0; i < 6; i++)
        {
            GridControl neighborGird = neighbors[i];
            if (neighborGird)
            {
                // 检测这个方向上格子是否存在棋子
                if (!neighborGird.CheckChess())
                {
                    // 若为空格 存储 并且搜索方式为全搜索
                    if (way == SearchWay.SearchAll)
                    {
                        saveGrid.Add(neighborGird, (Direction)i);
                        //neighborGird.ChangeColor(mark);
                    }


                    // 开始寻找这个方向上是否有跳板
                    Direction direction = (Direction)i;
                    int gridSpaceNum = 1;   // 记录到有棋子的格子数

                    // 检测出存在棋子的格子
                    var tmpNeighbor = neighborGird.neighbors[i];
                    while (tmpNeighbor)
                    {
                        if (!tmpNeighbor.CheckChess())
                        {
                            gridSpaceNum++;
                        }
                        else
                        {
                            break;
                        }
                        tmpNeighbor = tmpNeighbor.neighbors[i];
                    }

                    // 若存在
                    if (tmpNeighbor)
                    {
                        var chooseGrid = CheckLineGrid(gridSpaceNum, tmpNeighbor, i);
                        if (chooseGrid)
                        {
                            if (way == SearchWay.SearchJump & jumpedGrid != null)
                            {
                                if (jumpedGrid != chooseGrid)
                                {
                                    saveGrid.Add(chooseGrid, (Direction)i);
                                }
                            }
                            else
                            {
                                saveGrid.Add(chooseGrid, (Direction)i);
                            }
                        }

                    }
                }
                else
                {
                    // 若格子上有棋子
                    var outGrid = neighborGird.neighbors[i];
                    if (outGrid)
                    {
                        if (!outGrid.CheckChess())
                        {
                            if (way == SearchWay.SearchJump & jumpedGrid != null)
                            {
                                if (jumpedGrid != outGrid)
                                {
                                    saveGrid.Add(outGrid, (Direction)i);
                                }
                            }
                            else
                            {
                                saveGrid.Add(outGrid, (Direction)i);
                            }
                        }
                    }
                }
            }

        }

        return saveGrid;

    }

    public void ChangeColor(int num)
    {
        switch (num)
        {
            case (0):
                spriteRender.color = defaultColor;
                colorState = ColorState.Default;
                break;
            case (1):
                spriteRender.color = neighborColor;
                colorState = ColorState.Neighbor;
                break;
            case (2):
                spriteRender.color = chooseColor;
                colorState = ColorState.Choose;
                break;
        }

    }

    public void ActiveColor(Dictionary<GridControl, Direction> gridList, int color)
    {
        foreach (var g in gridList.Keys)
        {
            g.ChangeColor(color);
        }
    }

    public bool CheckChess()
    {
        if (ContentChess)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public ColorState GetColor()
    {
        return colorState;
    }

    public GridControl CheckLineGrid(int num, GridControl gridItem, int direction)
    {
        // 检测同一行的 作为跳板后是否还有空格子
        GridControl tmpGrid = gridItem.neighbors[direction];

        while (num > 0)
        {
            num--;
            if (tmpGrid == null)
            {
                break;
            }
            tmpGrid = tmpGrid.neighbors[direction];

        }
        if (tmpGrid == null) { return tmpGrid; }
        // 若检测到的格子有棋子 则作废
        if (tmpGrid.CheckChess()) { tmpGrid = null; }
        return tmpGrid;
    }
}

