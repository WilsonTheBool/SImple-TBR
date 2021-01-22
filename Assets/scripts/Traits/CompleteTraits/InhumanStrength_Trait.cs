using UnityEngine;
using System.Collections;

public class InhumanStrength_Trait : TraitBattle
{
    MeleeCharacter melee;

    [SerializeField]
    int pushPower = 1;

    protected override void SetUp()
    {
        base.SetUp();
        melee = owner as MeleeCharacter;
    }

    protected override void OnBattleStart()
    {
        AttackChanger_Push push = new AttackChanger_Push() { owner = melee, pushPower = 1 };
        melee.AddMeleeAttackChanger(push);
    }
}
