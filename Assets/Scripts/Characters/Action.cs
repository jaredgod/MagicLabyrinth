using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    moveU = 0,
    moveR,
    moveD,
    moveL,
    explore,
    teleport,
    vent,
    flipTimer,
}

public class Action : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public ActionType A { get; private set; }

    public void setAction(ActionType action)
    {
        A = action;
    }

    public void setPos(int x, int y)
    {
        X = x;
        Y = y;
        transform.position = new Vector3(X, 0, Y);
    }
}
