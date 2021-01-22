using UnityEngine;
using System.Collections;

public class BattleNoTargetSkillInputHandler : BattleInputHandler
{
    public NoTargetSkill skill;

    private NoTargetSkillController noTargetSkillController;

    protected override void SetUp()
    {
        noTargetSkillController = GetComponent<NoTargetSkillController>();
    }

    public override void HandleInput()
    {
        foreach(Vector3Int pos in skill.GetTargetPos())
        {
            BattleController.attackTileMap.SetTile(pos, BattleController.spellTargetTile);
        }

        if (Input.GetMouseButtonDown(1))
        {
            noTargetSkillController.RemoveAcceptWindow();

        }
    }
}
