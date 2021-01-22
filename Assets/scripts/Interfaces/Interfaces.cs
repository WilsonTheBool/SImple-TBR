using System;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    bool IsDead { get; set; }

    void Die(BattleObject killer);

    void TakeDamage(AttackData attack);

    //event EventHandler<TakeDamageArgs> OnDamageTaken;

    event EventHandler OnDeath;

    //public class TakeDamageArgs : EventArgs
    //{
    //    public float damageAmmount;

    //    public TakeDamageArgs(float expGain)
    //    {
    //        damageAmmount = expGain;
    //    }
    //}
}

public interface IRangeAttacker 
{
    event EventHandler<AttackEventArgs> BeforeRangeAttack;
    event EventHandler<AttackEventArgs> AfterRangeAttack;

    void StartAttackRange(Vector3Int attackPos);
}


public interface IMeleeAttacker
{

    event EventHandler<AttackEventArgs> OnBeforeAttack;
    event EventHandler<AttackEventArgs> OnAfterAttack;
    bool CanAttack(Vector3Int attackPos);

    void AttackMelee(Vector3Int attackPos, List<Vector3Int> path);

   
}



public interface IMoveable: ICanAct
{
    void MoveBy(Vector3Int reletiveToThis, bool endTurnOnDone);

    void MoveTo(Vector3Int globalTilePos);

    void MovePath(List<Vector3Int> path, bool endTurnOnDone);

    float WalkAnimSpeed { get; set; }

    event EventHandler OnMoveStart;

    event EventHandler OnMoveEnd;


}

public interface IPushable
{
    void PushThis(Vector3Int positionToPush);
}

public interface ICanAct
{
    bool CanAct { get; set; }

    int Initiative { get; set; }

}

public interface ICanLevelUp
{
    int Level { get; set; }

    int CurentEXP { get; set; }

    int MaxEXP { get; set; }

    void GainEXP(int ammount);

    void LevelUp();

    event EventHandler LeveledUp;
    event EventHandler<Character.ExpGainArgs> ExpGained;

    bool CanLevelUp();
}

public interface IDropLoot
{
    //Шанс что выпадет хоть какой то предмет
    float LootChanse { get; set; }

    GameObject GetRandomLoot();

    bool TryDropLoot();

    void DropLoot();

    event EventHandler OnLootDroped;
}
