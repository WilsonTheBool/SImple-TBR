using System;
using UnityEngine;

public static class TileMath
{
    public static float GetZRotation(Vector3Int ownerPos, Vector3Int targetPos)
    {
        Vector3Int diff = targetPos - ownerPos;
        
        float res = 0;

        if (diff.x == -1)
        {
            res = 180;
        }
        else
        if (diff.y == 1)
        {
            res = 90;
        }
        else
        if (diff.y == -1)
        {
            res = -90;
        }

        return res;
    }
}

