using System;
using System.Collections.Generic;
using UnityEngine;



public class WoundingCut: SwitchSkill
{
    DamageModifier modifier;

    MeleeAttackChanger changer;

    [SerializeField]
    float multValue = 1.5f;

    [SerializeField]
    float DamageTakenMultValue = 1.3f;

    [SerializeField]
    int woundDuration;

    public override void Activate(SkillTargetArgs targetArgs)
    {
        
        base.Activate(targetArgs);

        if(owner is MeleeCharacter)
        {
            MeleeCharacter melee = owner as MeleeCharacter;

            melee.OnAfterAttack += Melee_OnAfterAttack;

            changer = new AttackChanger_ApplyWound
            {
                owner = melee,
                woundDamageTakenMultValue = DamageTakenMultValue,
                woundDuration = this.woundDuration,
            };
            melee.meleeAttackChangers.Add(changer) ;

            modifier = new DamageModifier { numOfUses = 1, Type = DamageModifier.type.mult, value = multValue, };

            melee.attackDamageModifiers.Add(modifier);
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
        
        MeleeCharacter melee = owner as MeleeCharacter;
        melee.OnAfterAttack -= Melee_OnAfterAttack;
        melee.meleeAttackChangers.Remove(changer);
        melee.attackDamageModifiers.Remove(modifier);
    }

    protected override void AddController()
    {

        controller = GetComponent<SwitchSkillController>();
        controller.SetUp(this);

    }
}

