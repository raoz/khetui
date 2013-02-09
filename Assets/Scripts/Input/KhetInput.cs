using UnityEngine;
using System.Collections;

public abstract class KhetInput {
    public abstract void StartInput();
    public abstract void StopInput();
    public Move OnMove;
    public Rotate OnRotate;
    public Action OnEndTurn;
}
