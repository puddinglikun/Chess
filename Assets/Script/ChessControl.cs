using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    RedTeam, BlueTeam
}

public class ChessControl : MonoBehaviour
{
    int chesshp;
    int chessatk;
    Team team;

    int HPMaxValue = 3;

    public int Chessid { get; set; }

    public void SetChess(int id, int hp, int atk, Team t)
    {
        Chessid = id;
        chesshp = hp;
        chessatk = atk;
        team = t;
    }

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
        if (chesshp == 0)
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
        } else
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
        chesshp = chesshp >= hp ? chesshp - hp : 0 ;
        UpdateHP();
    }

    void UpdateHP()
    {
        slider.value = (float)chesshp / HPMaxValue;
    }

    public int GetHp()
    {
        return chesshp;
    }
}
