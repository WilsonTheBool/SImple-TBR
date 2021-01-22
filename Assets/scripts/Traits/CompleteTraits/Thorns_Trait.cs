using UnityEngine;
using System.Collections;

public class Thorns_Trait : TraitBattle
{

    protected override void OnBattleStart()
    {
        base.OnBattleStart();
        owner.OnAfterTakeDamage += Owner_OnAfterTakeDamage;
    }

    [Range(0,2)]
    [SerializeField]
    float damageReturn;
    private void Owner_OnAfterTakeDamage(object sender, Character.TakeDamageEventArgs e)
    {
        if(e.attackData.owner != null && e.attackData.owner is IKillable killable)
        {
            killable.TakeDamage(new AttackData(e.attackData.owner, e.attackData.damage, AttackData.DamageType.magic, AttackData.RangeType.melee, AttackData.AttackType.skill, true));
        }
    }
}
