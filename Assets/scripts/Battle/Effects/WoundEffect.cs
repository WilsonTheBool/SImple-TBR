using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WoundEffect : TickEffect, IConvertToBattleMessage
{
    DamageModifier woundModifier;


    public float damageTakeMod;

    public WoundEffect(Character owner, float damageMod) : base(owner)
    {
        maxDuration = duration;
        name = "Wound";


        woundModifier = new DamageModifier
        {
           
            duration_Type = DamageModifier.duration_type.infinite,
            Type = DamageModifier.type.mult,
            value = damageMod,

        };

    }

    public override void OnAdd()
    {
        owner.defenseModifiers.Add(woundModifier);
    }

    public override void OnRemove()
    {
        owner.defenseModifiers.Remove(woundModifier);
    }

    public string GetBattleMessage()
    {
        return "Wounded";
    }

    public Color GetMessageColor()
    {
        return BattleMessagesController.messagesController.defaultColor;
    }
}

