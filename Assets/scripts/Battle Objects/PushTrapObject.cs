using UnityEngine;
using System.Collections;
using System;

public class PushTrapObject : BattleObject
{
    public event EventHandler OnActivate;
    public override void Start()
    {
        base.Start();
        SetUp();
    }
    protected virtual void SetUp()
    {
        battleController.SomeoneMoved += BattleController_SomeoneMoved;
    }

    public virtual void OnDestroy()
    {
        battleController.SomeoneMoved -= BattleController_SomeoneMoved;
    }

    private void BattleController_SomeoneMoved(object sender, EventArgs e)
    {
        BattleObject obj = sender as BattleObject;

        if(obj.TilePosition == this.TilePosition)
        {
            Activate(obj);
        }
    }

    public virtual void Activate(BattleObject pusher)
    {
        OnActivate?.Invoke(this, null);
    }

}
