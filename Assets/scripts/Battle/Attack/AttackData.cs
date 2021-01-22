using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AttackData
{
    public BattleObject owner;

    public float damage;

    public DamageType damageType;
    public RangeType rangeType;
    public AttackType attackType;

    public bool isReducedByBlock;

    public AttackData(BattleObject owner, float damage, DamageType damageType, RangeType rangeType, AttackType attackType, bool isReducedByBlock)
    {
        this.owner = owner;
        this.damage = damage;
        this.damageType = damageType;
        this.rangeType = rangeType;
        this.attackType = attackType;

        this.isReducedByBlock = isReducedByBlock;
    }

    public enum DamageType
    {
        physical,
        magic,
    }

    public enum RangeType
    {
        melee,
        ranged,
        other,
    }

    public enum AttackType
    {
        regualar,
        skill,
        effect,
        other,
    }


}

