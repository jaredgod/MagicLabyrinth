using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Ind { get; private set; }
    public CharacterClass C { get; private set; }

    public void setInd(int ind)
    {
        Ind = ind;
    }
    public void setClass(CharacterClass c)
    {
        C = c;
    }

    public void setPos(int x, int y)
    {
        X = x;
        Y = y;
        transform.position = new Vector3(X, 0, Y);
    }
}
