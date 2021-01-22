using System;
using System.Collections.Generic;



public class SwitchSkillController : SkillButtonController
{


    protected override void HandleClick()
    {
        
        if (switchSkill.isActive)
        {
            switchSkill.OnDiactivate();
        }
        else
        {
            if(switchSkill.CanActivate())
            switchSkill.Activate(null);
        }
    }

    public override void AddToButton(SkillButtonBase button)
    {
        base.AddToButton(button);
        UpdateButton();
    }

    private SwitchSkill switchSkill;

    private void UpdateButton()
    {
        if(button != null)
        {
                button.SetToActive(switchSkill.isActive);    
            
                button.SetToCooldown(switchSkill.cooldown);
          
        }
       


    }

    public override void SetUp(ActiveSkill skill)
    {

        base.SetUp(skill);
    
        switchSkill = skill as SwitchSkill;
        skill.SkillUpdated += Skill_SkillUpdated;
        
    }

    private void Skill_SkillUpdated(object sender, EventArgs e)
    {
        UpdateButton();

    }
}

