using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInit : MonoBehaviour
{
    public GridControl[][] mapGrid;
    readonly int mapSize = 7;
    public GridControl gridPrefab; 

    // Start is called before the first frame update
    void Awake()
    {
        GridSet();
    }

    void GridSet()
    {
        mapGrid = new GridControl[mapSize * 2 - 1][];

        for (int i = 0; i < mapSize * 2 - 1; i++)
        {
            int lineAmount = i + 1 < 8 ? i + 1  : mapSize * 2 - 1 - i;
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
                    } else
                    {
                        gridItem.SetNeighbor(Direction.NW, mapGrid[i - 1][j]);
                        gridItem.SetNeighbor(Direction.NE, mapGrid[i - 1][j + 1]);
                    }
                    
                    
                } 
            }
        }

    }

    void OldSet()
    {
        //map = new Grid[(mapSize * 2 - 1), mapSize];

        //for (int i = 0; i < mapSize * 2 - 1; i++)
        //{
        //    for (int j = 0; j < mapSize; j++)
        //    {
        //        map[i, j] = new Grid(i, j);
        //        Grid oneGrid = map[i, j];

        //        if (i >= j && i < j + mapSize)
        //        {
        //            oneGrid.Content = 0;

        //            int columnNum = i + 1;
        //            int rowNum = i <= mapSize - 1 ? j + 1 : j - (i - mapSize);
        //            string findNum = columnNum.ToString() + rowNum.ToString();
        //            Debug.Log(findNum);
        //            oneGrid.GridTran = transform.Find(findNum);
        //        }
        //        else
        //            oneGrid.Content = -1;
        //    }
        //}

        //map[1, 0].Content = 21;
        //map[1, 1].Content = 22;
        //map[2, 0].Content = 23;
        //map[2, 1].Content = 24;
        //map[2, 2].Content = 25;
        //map[(mapSize * 2 - 3), (mapSize - 2)].Content = 11;
        //map[(mapSize * 2 - 3), (mapSize - 1)].Content = 12;
        //map[(mapSize * 2 - 4), (mapSize - 3)].Content = 13;
        //map[(mapSize * 2 - 4), (mapSize - 2)].Content = 14;
        //map[(mapSize * 2 - 4), (mapSize - 1)].Content = 15;
        //Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
