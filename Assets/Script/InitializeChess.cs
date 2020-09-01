using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chess
{
    public int chesshp;
    public int chessatk;

    public int Chessid { get; set; }

    public Chess(int id, int hp, int atk)
    {
        Chessid = id;
        chesshp = hp;
        chessatk = atk;

    }

    public GameObject chessObject { get; set; }
}

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
    Chess[] chesslist;
    public GameObject chessPrefab;
    private GridInit gridInit;

    public Postition[] createTuple = 
    {
        new Postition(1, 0),
        new Postition(1, 1),
        new Postition(2, 0),
        new Postition(2, 1),
        new Postition(2, 2),
        new Postition(11, 5),
        new Postition(11, 6),
        new Postition(10, 4),
        new Postition(10, 5),
        new Postition(10, 6)
    };

    // Start is called before the first frame update
    void Start()
    {
        GameObject GridObject = GameObject.FindWithTag("Grid");
        if (GridObject != null)
        {
            gridInit = GridObject.GetComponent<GridInit>();
        }

        chesslist = new Chess[10];

        int i = 0;
        foreach (var tup in createTuple)
        {
            int idNum = i < 5 ? 11 : 16;
            chesslist[i] = new Chess(i + idNum, 3, 1);
            //Debug.Log($"({tup.column}, {tup.row})");
            var pos = gridInit.map[tup.column, tup.row].GridTran.position;
            chesslist[i].chessObject = Instantiate(chessPrefab, pos, Quaternion.identity);
            gridInit.map[tup.column, tup.row].ContentChess = chesslist[i];
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
