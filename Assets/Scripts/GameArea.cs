using UnityEngine;
using System.Collections;

public class AreaSquare
{
    public KhetObject obj;
    public KhetModifier mod;
    public AreaSquare()
    {
        obj = null;
        mod = KhetModifier.None;
    }
}

public static class GameArea {
    public static AreaSquare[,] area;
    public static void Init()
    {
        area = new AreaSquare[10, 8];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                area[i, j] = new AreaSquare();
            }
        }
    }
}
