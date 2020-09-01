using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Grid(int column, int row)
    {
        Column = column;
        Row = row;
    }
    public int Column { get; set; }
    public int Row { get; set; }
    public int Content { get; set; }
    public int Jumped { get; set; }

    public Chess ContentChess { get; set; }
    public Transform GridTran { get; set; }
}

public class GridInit : MonoBehaviour
{
    public Grid[,] map;
    readonly int mapSize = 7;

    // Start is called before the first frame update
    void Awake()
    {
        map = new Grid[(mapSize * 2 - 1), mapSize];
        
        for (int i = 0; i < mapSize * 2 - 1; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                map[i, j] = new Grid(i, j);
                Grid oneGrid = map[i, j];

                if (i >= j && i < j + mapSize)
                {
                    oneGrid.Content = 0;

                    int columnNum = i + 1;
                    int rowNum = i <= mapSize - 1 ? j + 1 : j - (i - mapSize) ;
                    string findNum = columnNum.ToString() + rowNum.ToString();
                    //Debug.Log(findNum);
                    oneGrid.GridTran = transform.Find(findNum);
                } 
                else
                    oneGrid.Content = -1;
            }
        }

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


    void Show()
    {
        foreach (var g in map)
        {
            if (g.GridTran != null)
            {
                Debug.Log($"the column and row is ({g.Row}, {g.Column}), pos: {g.GridTran.position}");
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
