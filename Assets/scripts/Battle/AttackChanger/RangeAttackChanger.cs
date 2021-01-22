using UnityEngine;
using System.Collections;

public class RangeAttackChanger
{
    public RangedCharacter owner;

    public void SetUp(RangedCharacter owner)
    {
        this.owner = owner;

        this.owner.AfterRangeAttack += Owner_OnAfterAttack;

    }

    private void Owner_OnAfterAttack(object sender, AttackEventArgs e)
    {
        Invoke(e);
    }
    public virtual void Invoke(AttackEventArgs e)
    {

    }

}
