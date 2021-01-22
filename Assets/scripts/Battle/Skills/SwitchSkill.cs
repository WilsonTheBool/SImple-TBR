using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwitchSkill : ActiveSkill
{
    public bool isActive;

    public override void Start()
    {
        base.Start();
        owner.TurnEnd += Owner_TurnEnd;
    }

    private void Owner_TurnEnd(object sender, EventArgs e)
    {
        if(isActive)
        OnDiactivate();
    }

    public override void Activate(SkillTargetArgs targetArgs)
    {
        isActive = true;
        UpdateSkill();
    }

    public virtual void OnDiactivate()
    {
        isActive = false;

        UpdateSkill();
    }
}
