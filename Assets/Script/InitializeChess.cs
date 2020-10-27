using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Postition
{
    public Postition(int c, int r)
    {
        column = c;
        row = r;
    }
    public int column;
    public int row;

    
}

public class InitializeChess : MonoBehaviour
{
    ChessControl[] chesslist;
    public ChessControl chessPrefab;
    private GridInit gridInit;

    public Postition[] createTuple = 
    {
        new Postition(1, 0),
        new Postition(1, 1),
        new Postition(2, 0),
        new Postition(2, 1),
        new Postition(2, 2),
        new Postition(11, 0),
        new Postition(11, 1),
        new Postition(10, 0),
        new Postition(10, 1),
        new Postition(10, 2)
    };

    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitGrid()
    {
        GameObject GridObject = GameObject.FindWithTag("Grid");
        gridInit = GridObject.GetComponent<GridInit>();
        //if (GridObject != null)
        //{
        //    gridInit = GridObject.GetComponent<GridInit>();
        //}

        chesslist = new ChessControl[10];

        int i = 0;
        foreach (var tup in createTuple)
        {
            int idNum = i < 5 ? 11 : 16;

            Team team = Team.RedTeam;

            GridControl gridItem = gridInit.mapGrid[tup.column][tup.row];
            var localPoint = gridItem.transform.position;
            //var localPoint = gridInit.mapGrid[11][5].transform.position;

            ChessControl chessItem = chesslist[i] = Instantiate<ChessControl>(chessPrefab);
            chessItem.transform.SetParent(transform, false);
            chessItem.transform.localPosition = localPoint;
            if (i < 5)
            {
                chessItem.GetComponent<MeshRenderer>().material.color = Color.blue;
                team = Team.BlueTeam;
            }

            chessItem.SetChess(i + idNum, 3, 1, team);
            

            gridItem.ContentChess = chessItem;
            i++;

        }
    }
}
