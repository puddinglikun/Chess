using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexTest
{
    [SerializeField]
    private int a, b;

    public int A { get { return a; } }
    public int B { get { return b; } }

    public HexTest(int a, int b)
    {
        this.a = a;
        this.b = b;
    }

}
