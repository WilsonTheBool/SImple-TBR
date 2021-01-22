using UnityEngine;
using System.Collections;
using System;

public class DestructableObject : BattleObject, IKillable
{
    public bool IsDead {get; set;}
    public float HP
    {
        get
        {
            return HP;
        }
        set
        {
            if(value > MaxHP)
            {
                HP = MaxHP;
            }
            else
            {
                HP = value;
            }
        }
    }

    public float MaxHP
    {
        get
        {
            return MaxHP;
        }
        set
        {
            if(value < 0)
            {
                MaxHP = 0;
            }
            else
            {
                MaxHP = value;
            }
        }
    }

    public int ExpDrop { get; set; }

    public event EventHandler OnDeath;
    public event EventHandler OnDamageTaken;


    public override void Start()
    {
       
        SetUp();

        
    }

    protected override void SetUp()
    {
        OnDeath += ChangeAnimStateOnDeath;
        OnDamageTaken += ChangeAnimStateOnDamage;
    }

    public void Die(BattleObject killer)
    {
        IsDead = true;

        OnDeath?.Invoke(this, null);
    }

    public void TakeDamage(AttackData attack)
    {
        if(attack.damage > 0 && !IsDead)
        {
            if(attack.damage >= HP)
            {
                HP = 0;
                Die(attack.owner);
            }
            else
            {
                HP -= attack.damage;
                
            }

            OnDamageTaken?.Invoke(this, null);
        }
    }

    //animation handling
    private void ChangeAnimStateOnDeath(object obj, EventArgs e)
    {
        animator.SetBool("Dead", true);
    }

    private void ChangeAnimStateOnDamage(object obj, EventArgs e)
    {
        animator.SetBool("Damaged", true);
    }
}
