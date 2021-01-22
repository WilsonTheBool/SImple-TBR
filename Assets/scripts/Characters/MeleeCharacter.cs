using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class MeleeCharacter : Character, IMeleeAttacker
{
    public event EventHandler<AttackEventArgs> OnBeforeAttack;

    public event EventHandler<AttackEventArgs> OnAfterAttack;

    public List<MeleeAttackChanger> meleeAttackChangers;

    public List<DamageModifier> attackDamageModifiers;

    public void AttackMelee(Vector3Int attackPos, List<Vector3Int> path)
    {
        BattleObject target = battleController.GetObjectOnTile(attackPos);
        IKillable killable = target as IKillable;
        
        if(killable != null && CanAttack(attackPos))
        {
            SetTarget(killable, attackPos);
            MovePath(path, false);
            OnMoveEnd += MeleeCharacter_OnMoveEnd;
        }
    }

    protected IKillable CurentTarget;
    protected Vector3Int targetTile;

    public void SetTarget(IKillable target, Vector3Int TargetTile)
    {
        CurentTarget = target;
        targetTile = TargetTile;
    }

    public bool endTurnOnAttackDone = true;
    private void MeleeCharacter_OnMoveEnd(object sender, EventArgs e)
    {
        OnBeforeAttack?.Invoke(sender, new AttackEventArgs(CurentTarget, targetTile));

        StartCoroutine(Attack(CurentTarget, targetTile, endTurnOnAttackDone));

        OnMoveEnd -= MeleeCharacter_OnMoveEnd;
    }

    public bool CanAttack(Vector3Int attackPos)
    {
       return CanAct && (battleController.GetObjectOnTile<BattleObject>(attackPos) is IKillable);
    }

    public float AttackDelay = 0.1f;

    private IEnumerator Attack(IKillable killable, Vector3Int tile, bool endTurnOnDone)
    {
        
        yield return new WaitForSeconds(AttackDelay);

        float damage = Damage;

        damage = GetDamage(damage);

        killable.TakeDamage(new AttackData(this, damage, AttackData.DamageType.physical, AttackData.RangeType.melee, AttackData.AttackType.regualar, true));

        OnAfterAttack?.Invoke(this, new AttackEventArgs(killable, tile));

       

        if (endTurnOnDone)
        {
            EndTurn();
        }
    }

    public float GetDamage(float damage)
    {

        foreach (DamageModifier mod in attackDamageModifiers)
        {
            if (mod.Type == DamageModifier.type.add)
            {

               damage = mod.ChangeDamage(damage);
            }
        }

        foreach (DamageModifier mod in attackDamageModifiers)
        {
            if (mod.Type == DamageModifier.type.mult)
            {

                damage = mod.ChangeDamage(damage);
            }
        }

        return damage;
    }

    public float GetDamage(float damage, bool isAttack)
    {
        if (isAttack)
        {
            damage = GetDamage(damage);
        }
        else
        {
            foreach (DamageModifier mod in attackDamageModifiers)
            {
                if (mod.Type == DamageModifier.type.add)
                {

                    damage = mod.ChangeDamage_ForInfo(damage);
                }
            }

            foreach (DamageModifier mod in attackDamageModifiers)
            {
                if (mod.Type == DamageModifier.type.mult)
                {

                    damage = mod.ChangeDamage_ForInfo(damage);
                }
            }

           
        }
        return damage;
    }

    private void ActivateAttackChangers(object sender, AttackEventArgs e)
    {
        foreach(MeleeAttackChanger attackChanger in meleeAttackChangers)
        {
            attackChanger.Activate(e);
        }
    }

    protected override void SetUp()
    {
        base.SetUp();

        attackDamageModifiers = new List<DamageModifier>();

        meleeAttackChangers = new List<MeleeAttackChanger>();

        OnBeforeAttack += MeleeCharacter_OnBeforeAttack;

        OnAfterAttack += ActivateAttackChangers;
    }

    private void MeleeCharacter_OnBeforeAttack(object sender, AttackEventArgs e)
    {
        LookAt(e.TargetPos);
    }

    public void AddMeleeAttackChanger(MeleeAttackChanger meleeAttackChanger)
    {
        meleeAttackChangers.Add(meleeAttackChanger);
    }


}


public class AttackEventArgs : EventArgs
{
    public IKillable target;

    public Vector3 TargetPos;

    public AttackEventArgs(IKillable killable, Vector3Int targetPos)
    {
        target = killable;
        this.TargetPos = targetPos;
    }
}


