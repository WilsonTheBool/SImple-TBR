using UnityEngine;
using System.Collections.Generic;
using System;

public class NoTargetSkill : ActiveSkill
{
    [TextArea]
    public string InfoWindowText;
    public virtual List<Vector3Int> GetTargetPos()
    {
       var list =  new List<Vector3Int>(1);
        list.Add(owner.TilePosition);
        return list;
    }

    public override void Activate(SkillTargetArgs targetArgs)
    {
        
        UpdateSkill();
    }
}
