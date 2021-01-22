using System;
using System.Collections.Generic;
using UnityEngine;


public class HeavyStrikeSkill: SwitchSkill
{
    DamageModifier modifier;

    MeleeAttackChanger attackChanger;

    MeleeCharacter melee;

    [SerializeField]
    int pushPower = 1;

    [SerializeField]
    float multValue = 1.5f;

    public override void Activate(SkillTargetArgs targetArgs)
    {
        base.Activate(targetArgs);

        if(owner is MeleeCharacter)
        {
            melee = owner as MeleeCharacter;

            melee.OnAfterAttack += Melee_OnAfterAttack;

            modifier = new DamageModifier
            {
                numOfUses = 1,
                Type = DamageModifier.type.mult,
                value = multValue,
            };

            melee.attackDamageModifiers.Add(modifier);


            attackChanger = new AttackChanger_Push
            {
                owner = melee,
                pushPower = this.pushPower,
            };

            melee.meleeAttackChangers.Add(attackChanger);
        }
    }


  

    private void Melee_OnAfterAttack(object sender, AttackEventArgs e)
    {
        SetCooldown();

        OnDiactivate();
    }

    public override void OnDiactivate()
    {
        base.OnDiactivate();
        melee.OnAfterAttack -= Melee_OnAfterAttack;
        melee.meleeAttackChangers.Remove(attackChanger);
        melee.attackDamageModifiers.Remove(modifier);
    }

    protected override void AddController()
    {

        controller = GetComponent<SwitchSkillController>();
        controller.SetUp(this);
        
    }
   

   
}

