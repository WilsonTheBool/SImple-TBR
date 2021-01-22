using UnityEngine;
using System.Collections;
using System;

public class ActiveSkill : Skill
{
    public int cooldown;

    public int maxCooldown;

    public event EventHandler SkillUpdated;

    public override void Start()
    {
        base.Start();
        owner.TurnEnd += Owner_TurnEnd;
        AddController();
    }

    public bool isCooldown()
    {
        return cooldown > 0;
    }

    public void SetCooldown()
    {
        cooldown = maxCooldown;
        SkillUpdated?.Invoke(this, null);
    }

    public void TickCooldown()
    {
        if(cooldown > 0)
        {
            cooldown -= 1;

            SkillUpdated?.Invoke(this, null);
        }

    }

    public void UpdateSkill()
    {
        SkillUpdated?.Invoke(this, null);
    }

    public override bool CanActivate()
    {
        return !isCooldown();
    }

    
        

    private void Owner_TurnEnd(object sender, EventArgs e)
    {
        TickCooldown();
        
    }

    protected virtual void AddController()
    {
        
        controller = GetComponent<SkillController>();
        controller.SetUp(this);
    }

    public virtual void Activate(SkillTargetArgs targetArgs)
    {
        
    }

    public virtual bool CanCast(Vector3Int tilePos)
    {
        return true;
    }


}
