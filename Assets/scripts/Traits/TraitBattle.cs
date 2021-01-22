using UnityEngine;
using System.Collections;

public class TraitBattle : TraitBase
{

    protected BattleController battleController;

    protected override void SetUp()
    {
        base.SetUp();

        battleController = BattleController.battleController;
        battleController.OnBattleStart += BattleController_OnBattleStart;
    }

    private void BattleController_OnBattleStart(object sender, System.EventArgs e)
    {
        OnBattleStart();
    }

    protected virtual void OnBattleStart()
    {

    }
    
}
