using UnityEngine;
using System.Collections;

public class MeleeAttackChanger
{
    public MeleeCharacter owner;

    public void SetUp(MeleeCharacter owner)
    {
        this.owner = owner;

        this.owner.OnAfterAttack += Owner_OnAfterAttack;
        
    }

    private void Owner_OnAfterAttack(object sender, AttackEventArgs e)
    {
        Activate(e);
    }
    public virtual void Activate(AttackEventArgs e)
    {

    }
}
