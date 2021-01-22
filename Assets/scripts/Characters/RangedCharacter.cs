using UnityEngine;
using System.Collections;
using System;

public class RangedCharacter : MeleeCharacter, IRangeAttacker
{

    public float rangeDamage;

    public event EventHandler<AttackEventArgs> BeforeRangeAttack;
    public event EventHandler<AttackEventArgs> AfterRangeAttack;

    

    public bool forceToMelee = false;
    public bool CanRangeAttack()
    {
        bool isNearCharacter = false;

        if (CanAct)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector3Int offSet = new Vector3Int(i, j, 0);
                    Character neighbour = battleController.GetObjectOnTile<Character>(TilePosition + offSet);
                    if (neighbour != null && IsOpositeTeam(team, neighbour.team))
                    {
                        isNearCharacter = true;
                        break;
                    }
                }
            }
        }






        return CanAct && !isNearCharacter && !forceToMelee;
    }

    public float GetRangeDamage(float damage, bool isReal)
    {
        return damage;
    }

    public void ForceToMelee()
    {
        if (forceToMelee)
        {
            forceToMelee = false;
        }
        else
        {
            forceToMelee = true;
        }
    }

    protected override void SetUp()
    {
        base.SetUp();
        

        animationController.FireAnimationEnded += AttackRange;
    }

   

    public void StartAttackRange(Vector3Int attackPos)
    {
        if (CanAttack(attackPos))
        {


            IKillable target = battleController.GetObjectOnTile<BattleObject>(attackPos) as IKillable;

            SetTarget(target, attackPos);

            LookAt(attackPos);

            BeforeRangeAttack?.Invoke(this, new AttackEventArgs(target, attackPos));

        }
            
        
    }

    public void AttackRange()
    {
        CurentTarget.TakeDamage(new AttackData(this, rangeDamage, AttackData.DamageType.physical, AttackData.RangeType.ranged, AttackData.AttackType.regualar, true));

        CanAct = false;

        EndTurn();
    }
}
