using System;
using System.Collections.Generic;
using UnityEngine;


public class SkillTargetArgs: EventArgs
{
    public Vector3Int tileMousePos;

    public SkillTargetArgs(Vector3Int tileMousePos)
    {
        this.tileMousePos = tileMousePos;
    }
}

