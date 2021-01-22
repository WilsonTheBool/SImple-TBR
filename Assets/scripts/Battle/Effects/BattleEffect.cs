using System;
using System.Collections.Generic;


public class BattleEffect
{
    protected Character owner;

    public Skill skill;

    public string name;

    public Effect_Type type;

    public enum Effect_Type
    {
        good, bad
    }

    public bool isGood()
    {
        return type == Effect_Type.good;
    }

    public virtual void OnAdd()
    {

    }

    public virtual void OnRemove()
    {

    }

    public BattleEffect(Character owner)
    {
        this.owner = owner;
    }
}

