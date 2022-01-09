using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子的初始化
/// </summary>
public class GridInit : MonoBehaviour
{
    public GridControl[][] mapGrid;
    readonly int mapSize = 7;
    public GridControl gridPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        // 初始化配表信息
        ConfigMgr.Ins.InitConfig();
        GridSet();
    }

    void GridSet()
    {
        mapGrid = new GridControl[mapSize * 2 - 1][];

        for (int i = 0; i < mapSize * 2 - 1; i++)
        {
            int lineAmount = i + 1 < 8 ? i + 1 : mapSize * 2 - 1 - i;
            mapGrid[i] = new GridControl[lineAmount];
            for (int j = 0; j < lineAmount; j++)
            {
                Vector3 localPoint = new Vector3(
                    (lineAmount - 1) * (-0.5f) + j * 1.0f,
                    4.5f - (i * 0.75f),
                    0f
                );

                GridControl gridItem = mapGrid[i][j] = Instantiate<GridControl>(gridPrefab);
                gridItem.SetCoordinate(i, j);
                gridItem.transform.SetParent(transform, false);
                gridItem.transform.localPosition = localPoint;

                // set grid direction
                if (j > 0)
                {
                    gridItem.SetNeighbor(Direction.W, mapGrid[i][j - 1]);
                }
                if (i > 0)
                {
                    if (i < 7)
                    {
                        if (j - 1 >= 0)
                        {
                            gridItem.SetNeighbor(Direction.NW, mapGrid[i - 1][j - 1]);
                        }
                        if (j <= i - 1)
                        {
                            gridItem.SetNeighbor(Direction.NE, mapGrid[i - 1][j]);
                        }
                    }
                    else
                    {
                        gridItem.SetNeighbor(Direction.NW, mapGrid[i - 1][j]);
                        gridItem.SetNeighbor(Direction.NE, mapGrid[i - 1][j + 1]);
                    }


                }
            }
        }

    }
}

