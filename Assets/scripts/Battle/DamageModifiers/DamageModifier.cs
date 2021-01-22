using UnityEngine;
using System;

public class DamageModifier
{
    public int numOfUses;

    public event Action OnActivate;
    public enum type
    {
        add, mult
    }

    public enum duration_type
    {
        defult, infinite, 
    }

    public duration_type duration_Type;

    public type Type;

    public float value;

    /// <summary>
    /// changing damage value w/out activating
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public virtual float ChangeDamage_ForInfo(float damage)
    {
        if (numOfUses > 0 || duration_Type == duration_type.infinite)
        {
            if (Type == type.add)
            {
                damage += value;
            }
            else
        if (Type == type.mult)
            {
                damage *= value;
            }

        }

        return damage;
    }

    public virtual float ChangeDamage(float damage)
    {
        if(numOfUses > 0 || duration_Type == duration_type.infinite)
        {
            if (Type == type.add)
            {
                damage += value;
            }
            else
        if (Type == type.mult)
            {
                damage *= value;
            }

            if(duration_Type != duration_type.infinite)
            {
                numOfUses--;
            }
            
        }

        OnActivate?.Invoke();

        return damage;
    }
}
