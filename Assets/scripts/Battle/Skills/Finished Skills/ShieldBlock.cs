using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShieldBlock: NoTargetSkill
{
    public override void Activate(SkillTargetArgs targetArgs)
    {
        SetCooldown();
        UpdateSkill();
        owner.AddEffect(new ShieldBlockEffect(owner));

        owner.EndTurn();

        
    }

}

