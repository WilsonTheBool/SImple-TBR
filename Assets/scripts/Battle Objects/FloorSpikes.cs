using UnityEngine;
using System.Collections;
using System;


public class FloorSpikes : PushTrapObject
{
    public float damage;


    
    public override void Activate(BattleObject pusher)
    {
        base.Activate(pusher);

        if(pusher is IKillable)
        {
            (pusher as IKillable).TakeDamage(new AttackData(null, damage, AttackData.DamageType.physical, AttackData.RangeType.other, AttackData.AttackType.other, false));
        }

        if(pusher is Character)
        {
            (pusher as Character).TurnEnd += DamageOnNextTurn;
        }


    }

    private void DamageOnNextTurn(object obj, EventArgs e)
    {
        

        Character pusher = obj as Character;

        if(pusher != null)
        {
            base.Activate(pusher);

            if (pusher.TilePosition == this.TilePosition)
            {
                pusher.TakeDamage(new AttackData(null, damage, AttackData.DamageType.physical, AttackData.RangeType.other, AttackData.AttackType.other, false));
            }
            else
            {
                pusher.TurnEnd -= DamageOnNextTurn;
            }

            if (pusher.IsDead)
            {
                pusher.TurnEnd -= DamageOnNextTurn;
            }
        }
        


    }


}
