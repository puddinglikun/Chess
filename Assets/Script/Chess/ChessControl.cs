using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 棋子的控制脚本
/// </summary>
public class ChessControl : MonoBehaviour
{
    /// <summary>
    /// 棋子队伍
    /// </summary>
    private Team team;
    /// <summary>
    /// 棋子攻击力
    /// </summary>
    private int chessAtk;
    /// <summary>
    /// 棋子血量
    /// </summary>
    private int chessHp;
    /// <summary>
    /// 棋子的最大HP
    /// </summary>
    private int hpMaxValue = 3;

    public int ChessId { get; set; }

    public void SetChess(int id, int hp, int atk, Team t)
    {
        ChessId = id;
        chessHp = hp;
        chessAtk = atk;
        team = t;
    }

    /// <summary>
    /// 棋子信息
    /// </summary>
    public ChessTeam TeamInfo { get; private set; }
    /// <summary>
    /// 设置棋子信息
    /// </summary>
    /// <param name="teamInfo"></param>
    public void SetChessInfo(ChessTeam teamInfo)
    {
        TeamInfo = teamInfo;
        ChessId = teamInfo.id;
        team = teamInfo.teamType == 0 ? Team.BlueTeam : Team.RedTeam;

        HeroInfo = ConfigMgr.Ins.ConfigChessHeroInfo.Find(hero => hero.id == teamInfo.heroId);
        hpMaxValue = HeroInfo.hpMaxValue;
        chessHp = hpMaxValue;
        chessAtk = HeroInfo.atk;

        SetTeamColor();
    }

    /// <summary>
    /// 棋子上的英雄信息
    /// </summary>
    public ChessHeroInfo HeroInfo { get; private set; }

    public Vector2 hpPosition;
    public RectTransform recTransform;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = recTransform.GetComponent<Slider>();
        //slider.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        HPFollow();
        HPZero();
    }

    void HPFollow()
    {
        Vector2 player2DPosition = Camera.main.WorldToScreenPoint(transform.position);
        recTransform.position = player2DPosition + hpPosition;

        //血条超出屏幕就不显示
        if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        {
            recTransform.gameObject.SetActive(false);
        }
        else
        {
            recTransform.gameObject.SetActive(true);
        }
    }

    void HPZero()
    {
        if (chessHp == 0)
        {
            GameObject.Destroy(gameObject);
        }

    }

    private void OnMouseDown()
    {
        //Debug.Log("you click the chess");
    }

    public bool CompareTeam(Team t)
    {
        if (t == team)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Team GetTeam()
    {
        return team;
    }

    public void ReduceHP(int hp = 1)
    {
        //Debug.Log(hp);
        //Debug.Log(chesshp);
        chessHp = chessHp >= hp ? chessHp - hp : 0;
        UpdateHP();
    }

    void UpdateHP()
    {
        slider.value = (float)chessHp / hpMaxValue;
    }

    public int GetHp()
    {
        return chessHp;
    }

    /// <summary>
    /// 设置队伍颜色
    /// </summary>
    private void SetTeamColor()
    {
        string teamString = team == Team.BlueTeam ? "BlueTeam" : "RedTeam";
        var material = Resources.Load($"Material/{teamString}") as Material;
        Material[] mat = new Material[1] { material };
        GetComponent<Renderer>().materials = mat;
    }
}

