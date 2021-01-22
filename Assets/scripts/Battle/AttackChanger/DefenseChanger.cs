using System;
using UnityEngine;

public class DefenseChanger
{
    public Character owner;

    public void SetUp(Character owner)
    {
        this.owner = owner;

        this.owner.OnAfterTakeDamage += Owner_OnAfterTakeDamage;

    }

    private void Owner_OnAfterTakeDamage(object sender, Character.TakeDamageEventArgs e)
    {
        Activate(e);
    }
    public virtual void Activate(Character.TakeDamageEventArgs e)
    {

    }
}
