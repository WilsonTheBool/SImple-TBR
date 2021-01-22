using UnityEngine;
using System.Collections;

public class CharacterStatsController : MonoBehaviour
{
    [SerializeField]
    private TurnOrderController turnOrderController;
    private BattleController battleController;

    public CharacterStats enemyStatsPrefab;
    public CharacterStats playerStatsPrefab;

    [SerializeField]
    private Canvas battleCanvas;

    [SerializeField]
    Vector3 statsOffSet;    
    
    [SerializeField]
    Vector3 hugeModelOffSet;

    private void Start()
    {
        battleController = BattleController.battleController;
        battleController.NewBattleObjectAdded += BattleController_NewBattleObjectAdded;
        battleController.OnBattleStart += BattleController_OnBattleStart;
    }

    private void BattleController_OnBattleStart(object sender, System.EventArgs e)
    {
        foreach(BattleObject obj in battleController.battleObjects)
        {
            Character ch = obj as Character;
            if(ch != null)
            {
                SpawnStats(ch);
            }
        }
    }

    private void BattleController_NewBattleObjectAdded(object sender, System.EventArgs e)
    {
        Character ch = sender as Character;

        if(ch != null)
        {
            SpawnStats(ch);
        }
    }

    public void SpawnStats(Character owner)
    {
        if(owner.team == BattleObject.Team.enemy || owner.team == BattleObject.Team.other)
        {
            CharacterStats stats = Instantiate(enemyStatsPrefab.gameObject, battleCanvas.transform).GetComponent<CharacterStats>();
            stats.SetUp(statsOffSet, hugeModelOffSet, owner, turnOrderController);
        }
        else
       if (owner.team == BattleObject.Team.player)
        {
            CharacterStats stats = Instantiate(playerStatsPrefab.gameObject, battleCanvas.transform).GetComponent<CharacterStats>();
            stats.SetUp(statsOffSet, hugeModelOffSet, owner, turnOrderController);
        }
    }
}
