using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初始化棋子
/// </summary>
public class InitializeChess : MonoBehaviour
{
    ChessControl[] chesslist;
    public ChessControl chessPrefab;
    private GridInit gridInit;

    void Start()
    {
        InitGrid();
    }

    /// <summary>
    /// 初始化棋子
    /// </summary>
    private void InitGrid()
    {
        GameObject GridObject = GameObject.FindWithTag("Grid");
        gridInit = GridObject.GetComponent<GridInit>();
        ReadConfigTeamData();
    }

    /// <summary>
    /// 读取棋子队伍信息 并完成初始化
    /// </summary>
    private void ReadConfigTeamData()
    {
        foreach (var teamConfig in ConfigMgr.Ins.ConfigChessTeam)
        {
            // 索引对应坐标的格子
            GridControl gridItem = gridInit.mapGrid[teamConfig.posX][teamConfig.posY];
            var localPoint = gridItem.transform.position;
            // 初始化棋子 并设定坐标
            ChessControl chessItem = Instantiate(chessPrefab);
            chessItem.transform.SetParent(transform, false);
            chessItem.transform.localPosition = localPoint;
            // 设定信息
            chessItem.SetChessInfo(teamConfig);
            // 把棋子绑定
            gridItem.ContentChess = chessItem;
        } 
    }
}



