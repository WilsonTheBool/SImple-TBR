using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : SkillButtonController
{

    protected override void HandleClick()
    {
        if(skillInputHandler != null)
        {
            StartInputHandler();
        }
        else
        {
            skill.Activate(new SkillTargetArgs(mainInput.tileMousePosition));
        }
    }

    private void StartInputHandler()
    {
        mainInput.SetInputPermanent(skillInputHandler);
    }


}
