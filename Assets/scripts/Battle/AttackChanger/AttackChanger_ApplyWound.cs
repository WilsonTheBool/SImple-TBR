using UnityEngine;
using System.Collections;


public class AttackChanger_ApplyWound : MeleeAttackChanger
{

    public float woundDamageTakenMultValue;


   public  int woundDuration;

    public override void Activate(AttackEventArgs e)
    {
        if (e.target is Character)
        {

            Character target = e.target as Character;

            target.AddEffect(new WoundEffect(target, woundDamageTakenMultValue)
            {
                duration = woundDuration,

                type = BattleEffect.Effect_Type.bad,
            }
            ) ;
        }
    }
}

