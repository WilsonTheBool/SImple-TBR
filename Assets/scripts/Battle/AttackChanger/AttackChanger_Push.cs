using UnityEngine;
using System.Collections;

public class AttackChanger_Push : MeleeAttackChanger
{

    public int pushPower;

    public override void Activate(AttackEventArgs e)
    {
        var target = e.target;
        if(target is IPushable)
        {
            Vector3Int pushPos = GetPushPosition(owner.TilePosition, (e.target as BattleObject).TilePosition);

            (target as IPushable).PushThis(pushPos);
        }
    }

    private Vector3Int GetPushPosition(Vector3Int pusherPos, Vector3Int targetPos)
    {
        return targetPos + ((targetPos - pusherPos) * pushPower);
    }
}
