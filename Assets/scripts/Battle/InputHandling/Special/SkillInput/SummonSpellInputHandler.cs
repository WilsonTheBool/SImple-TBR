using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class SummonSpellInputHandler : BattleInputHandler
{

    public ActiveSkill skill;

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetMouseButton(0))
        {
            if(skill.CanActivate() && skill.CanCast(tileMousePosition))
            {
                skill.Activate(new SkillTargetArgs(tileMousePosition));
                TryCancelInputHandler();
            }

            
        }

        skillDecorator.ClearTileMap(true);

        if(skill.CanCast(tileMousePosition))
        skillDecorator.DrawTileColored(tileMousePosition, skillDecorator.TargetTile, skillDecorator.summonSpellColor_target, true);
        
    }

    protected override void OnCancel()
    {
        skillDecorator.ClearTileMap(true);
        skillDecorator.ClearTileMap(false);
        
        
    }

    public override void OnBegin()
    {
        skillDecorator.DrawCircleColored(skill.owner.TilePosition, 1, skillDecorator.FourWayTile, skillDecorator.summonSpellColor, skill.CanCast, true, false);
        skillDecorator.ClearMoveTilemap();
    }
}
