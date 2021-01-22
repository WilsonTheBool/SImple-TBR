using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShieldBlockEffect : BattleEffect
{
    DamageModifier modifier;
    public ShieldBlockEffect(Character owner) : base(owner)
    {
        name = "Shielded";

        this.type = Effect_Type.good;

        modifier = new DamageModifier { numOfUses = 1, Type = DamageModifier.type.mult, value = 0, };

        modifier.OnActivate += Modifier_OnActivate;

        owner.defenseModifiers.Add(modifier);
    }

    private void Modifier_OnActivate()
    {
       // owner.defenseModifiers.Remove(modifier);
        owner.RemoveEffect(this);
    }
}

