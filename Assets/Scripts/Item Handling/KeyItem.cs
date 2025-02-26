using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Item
{
    [Header("Belonged PuzzleObject")]
    public PuzzleObject belongedPuzzleObject;
    public virtual void ApplyBehaviour()
    {
        return;
    }
}
