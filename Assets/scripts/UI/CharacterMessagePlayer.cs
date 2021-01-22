using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMessagePlayer: MonoBehaviour
{
    private BattleMessagesController messagesController;

    private float maxDelyay;

    private float delay = 0;

    [SerializeField]
    private Character owner;

    private Queue<BattleMessagesController.TextDraw_Data> drawQueue;

    private void Start()
    {
        messagesController = BattleMessagesController.messagesController;
        maxDelyay = messagesController.maxDelyay;
        drawQueue = new Queue<BattleMessagesController.TextDraw_Data>();

        if (owner == null)
        {
            owner = GetComponent<Character>();
        }

        owner.OnAfterTakeDamage += Owner_OnAfterTakeDamage;
        owner.ExpGained += Owner_ExpGained;
        owner.LeveledUp += Owner_LeveledUp;

        owner.OnAddEffect += Owner_OnAddEffect;
    }

    private void Owner_OnAddEffect(object sender, Character.EffectArgs e)
    {
        if (e.effect is IConvertToBattleMessage bm)
            drawQueue.Enqueue(new BattleMessagesController.TextDraw_Data(owner, bm.GetMessageColor(), bm.GetBattleMessage(), messagesController.defultFontSize));
    }

    private void Owner_LeveledUp(object sender, EventArgs e)
    {
        if (owner != null)
        {
            drawQueue.Enqueue(new BattleMessagesController.TextDraw_Data(owner, messagesController.defaultColor, "Level up!", messagesController.defultFontSize, false));
        }
    }

    private void Owner_ExpGained(object sender, Character.ExpGainArgs e)
    {
        if (owner != null)
        {
            drawQueue.Enqueue(new BattleMessagesController.TextDraw_Data(owner, messagesController.defaultColor, $"+ {e.expGainAmmount} Exp", messagesController.defultFontSize, false));
        }
    }

    private void Owner_OnAfterTakeDamage(object sender, Character.TakeDamageEventArgs e)
    {
        if(owner != null)
        if (owner is Character ch && ch.Def > 0)
        {
            drawQueue.Enqueue(new BattleMessagesController.TextDraw_Data(owner, messagesController.redColor, $"{e.attackData.damage} <color=white>({ch.Def})</color>", messagesController.defultFontSize ));
        }
        else
        {
            drawQueue.Enqueue(new BattleMessagesController.TextDraw_Data(owner, messagesController.redColor, $"{e.attackData.damage}", messagesController.defultFontSize));
        }
    }


    private void FixedUpdate()
    {
        if(delay > 0)
        {
            delay -= Time.fixedDeltaTime;
        }

        TryDraw();
    }

    void TryDraw()
    {
        if(drawQueue.Count > 0 && delay <= 0)
        {
            Draw();
            delay = maxDelyay;
        }
    }

    void Draw()
    {
        messagesController.DrawText(drawQueue.Dequeue());
    }
}

