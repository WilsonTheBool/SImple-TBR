using UnityEngine;
using System.Collections;

public class SummonImp_Skill : ActiveSkill
{

    [SerializeField]
    BattleObject impPrefab;

    public override void Activate(SkillTargetArgs targetArgs)
    {
        Vector3 spawnPos = battleController.floorTilemap.GetCellCenterWorld(targetArgs.tileMousePos);
        BattleObject spawned = Instantiate<BattleObject>(impPrefab, spawnPos, new Quaternion());
        battleController.AddNewBattleObject(spawned);

        owner.EndTurn();
    }

    public override bool CanCast(Vector3Int tilePos)
    {

        if (battleController != null && battleController.IsTileEmpty(tilePos) && Vector3Int.Distance(tilePos, owner.TilePosition) < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
