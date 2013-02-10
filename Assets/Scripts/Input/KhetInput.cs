using UnityEngine;
using System.Collections;

public enum Result
{
    RedWon,
    SilverWon,
    Draw
}

public abstract class KhetInput {
    public abstract void StartInput();
    public abstract void StopInput();
    public abstract void GameOver(Result result);
    public string name;
    public Move OnMove;
    public Rotate OnRotate;
    public Action OnEndTurn;
}
