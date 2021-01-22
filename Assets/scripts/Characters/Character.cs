using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Character : BattleObject, IKillable, ICanAct, IMoveable, ICanLevelUp, IPushable
{
    // private BattleObject lastBattleObject;
    public BrainBase brain;

    public CharacterAnimationController animationController;
    public CharacterMessagePlayer characterMessagePlayer;

    public string Name;

    public List<Skill> skills;

    public List<TraitBase> traits;

    public List<BattleEffect> effects;

    public List<DamageModifier> defenseModifiers;

    public CharacterStats stats;

    public bool isDead;
    public bool IsDead { get { return isDead; } set { isDead = value; } }

    float hp;

    
    
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            if(value > MaxHP)
            {
                hp = MaxHP;

            }
            else
            {
                hp = value;
            }
        }
    }

    [SerializeField]
    public float MaxHP;

    [SerializeField]
    public float Damage;

    [SerializeField]
    public float Def;

    public bool CanAct { get; set; }

    [SerializeField]
    int initiative;
    public int Initiative
    {
        get
        {
            return initiative;
        }
        set
        {
            if(value < 0)
            {
                initiative = 0;
            }
            else
            {
                initiative = value;
            }
        }
    }

    [SerializeField]
    public int ExpDrop;

    [SerializeField]
    public int speed;

    [SerializeField]
    int level;

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    [SerializeField]
    int curentEXP;

    public int CurentEXP
    {
        get
        {
            return curentEXP;
        }
        set
        {
            curentEXP = value;
        }
    }

    [SerializeField]
    int maxEXP;

    public int MaxEXP
    {
        get
        {
            return maxEXP;
        }
        set
        {
            maxEXP = value;
        }
    }

    public Sprite icon;

    public bool isBigModel;

    public event EventHandler TurnStart;
    public event EventHandler TurnEnd;

   

    public event EventHandler OnDeath;
    public event EventHandler OnDeathStart;
    public event EventHandler OnDeathAnimEnded;

    public event EventHandler<EffectArgs> OnAddEffect;
    public event EventHandler<EffectArgs> OnRemoveEffect;

    public event EventHandler OnMoveStart;
    public event EventHandler OnMoveEnd;

    public event EventHandler AnyActionEnded;


    public event EventHandler OnTryCantMove;

    [SerializeField]
    private float walkAniumSpeed = 3;
    public float WalkAnimSpeed { get { return walkAniumSpeed; } set { walkAniumSpeed = value; } }

    public int GetAIThreat()
    {
        return 1;
    }
    public void AddEffect(BattleEffect effect)
    {
        print($"Added effect: \"{effect.name}\"");
        effects.Add(effect);
        effect.OnAdd();

        OnAddEffect?.Invoke(this, new EffectArgs(effect));
       
    }

    public void RemoveEffect(BattleEffect effect)
    {
        print($"Removed effect: \"{effect.name}\"");
        effects.Remove(effect);
        effect.OnRemove();

        OnRemoveEffect?.Invoke(this, new EffectArgs(effect));
    }

    public void LookAt(Vector3 pos)
    {
        if(pos.x != transform.position.x)
        if(pos.x > transform.position.x)
        {
            animationController.SwitchSide(CharacterAnimationController.Side.right);
        }
        else
        {
            animationController.SwitchSide(CharacterAnimationController.Side.left);
        }
    }

    public void AddTrait(TraitBase trait)
    {
        traits.Add(trait);
    }

    public void LookAt(Vector3Int pos)
    {
        if (pos.x != TilePosition.x)
        if (pos.x > TilePosition.x)
        {
            animationController.SwitchSide(CharacterAnimationController.Side.right);
        }
        else
        {
            animationController.SwitchSide(CharacterAnimationController.Side.left);
        }
    }

    public void SwitchSide(CharacterAnimationController.Side side)
    {
        animationController.SwitchSide(side);
    }

    public virtual void Die(BattleObject killer)
    {
        DropEXP(killer);

        RemoveActionPoint();

        OnDeathStart?.Invoke(this, null);
    }

    public virtual void DropEXP(BattleObject killer)
    {
        if(killer is ICanLevelUp)
        {
            (killer as ICanLevelUp).GainEXP(ExpDrop);
        }
    }

    public bool CanMove = true;


    protected virtual IEnumerator Move(List<Vector3Int> path, bool endTurnOnDone)
    {
        while(path.Count > 0)
        {
            Vector3 nextPos = battleController.GetGlobalFromTilePosition(path[0]);

            while((transform.position - nextPos).magnitude > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, WalkAnimSpeed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }

            path.RemoveAt(0);
            yield return new WaitForFixedUpdate();
        }

        OnMoveEnd?.Invoke(this, null);

        if (endTurnOnDone)
        {
            EndTurn();
        }

       
    }


    public bool TryMove()
    {
        bool canMove = CanAct && CanMove;

        if (!canMove)
            OnTryCantMove?.Invoke(this, null);

        return canMove;
    }
    public virtual void MovePath(List<Vector3Int> path, bool endTurnOnDone)
    {
        if (TryMove())
        {
            OnMoveStart?.Invoke(this, null);

            StartCoroutine(Move(path, endTurnOnDone));

            RemoveActionPoint();

            
        }


    }

    public void MoveBy(Vector3Int reletiveToThis, bool endTurnOnDone)
    {
        if (TryMove())
        {
            OnMoveStart?.Invoke(this, null);

            StartCoroutine(Move(battleController.PathFinder.GetPathGlobal(TilePosition + reletiveToThis), endTurnOnDone));

            RemoveActionPoint();
        }


    }

   

    public void MoveTo(Vector3Int globalTilePos)
    {
        battleController.PathFinder.GetPathGlobal(globalTilePos);
    }

    public event EventHandler<TakeDamageEventArgs> OnBeforeTakeDamage;
    public event EventHandler<TakeDamageEventArgs> OnAfterTakeDamage;
    public event EventHandler LeveledUp;
    public event EventHandler<ExpGainArgs> ExpGained;

    public class ExpGainArgs : EventArgs
    {
        public float expGainAmmount;

        public ExpGainArgs(float expGain)
        {
            expGainAmmount = expGain;
        }
    }

    public void TakeDamage(AttackData attack)
    {
        float ammount = attack.damage;
        
        if(attack.damage > 0)
        {
            attack.damage = GetDamageTake(ammount);

            OnBeforeTakeDamage?.Invoke(this, new TakeDamageEventArgs(attack));
            HP -= attack.damage;

            OnAfterTakeDamage?.Invoke(this, new TakeDamageEventArgs(attack));
            if (HP <= 0)
            {
                Die(attack.owner);

            }

            
        }
        
    }

    public float GetDamageTake(float damage)
    {

        foreach (DamageModifier mod in defenseModifiers)
        {
            if (mod.Type == DamageModifier.type.add)
            {
                damage = mod.ChangeDamage(damage);
            }
        }

        foreach (DamageModifier mod in defenseModifiers)
        {
            if (mod.Type == DamageModifier.type.mult)
            {
                damage = mod.ChangeDamage(damage);
            }
        }

        damage -= Def;

        if (damage < 0)
        {
            damage = 0;
        }

        return damage;
    }

    public float GetDamageTake(float damage, bool isReal)
    {
        if (isReal)
        {
            damage = GetDamageTake(damage);
        }
        else
        {
            foreach (DamageModifier mod in defenseModifiers)
            {
                if (mod.Type == DamageModifier.type.add)
                {
                    damage = mod.ChangeDamage_ForInfo(damage);
                }
            }

            foreach (DamageModifier mod in defenseModifiers)
            {
                if (mod.Type == DamageModifier.type.mult)
                {
                    damage = mod.ChangeDamage_ForInfo(damage);
                }
            }

            damage -= Def;

            if(damage < 0)
            {
                damage = 0;
            }
        }
       

        return damage;
    }

    public  void Awake()
    {
        SetUp();
    }
    protected override void SetUp()
    {
        base.SetUp();

        HP = MaxHP;

        IsDead = false;
        
        CanAct = true;

        effects = new List<BattleEffect>();

        defenseModifiers = new List<DamageModifier>();

        OnDeathAnimEnded += Character_OnDeathAnimEnded;

    }


    private void Character_OnDeathAnimEnded(object sender, EventArgs e)
    {

        OnDeath?.Invoke(sender, null);

        if(battleController.selectedCharacter == this)
        {
            EndTurn();
        }
        
    }

    public void RemoveActionPoint()
    {
        CanAct = false;
    }

    public void StartTurn()
    {
        StartCoroutine(StartTurnWait());
 
       
    }

    IEnumerator StartTurnWait()
    {
        if(battleController != null)
        while (battleController.SomeoneActing)
        {
            yield return new WaitForFixedUpdate();
        }


        if(brain != null)
        {
            brain.ProsessTurn();
        }

        TurnStart?.Invoke(this, null);
    }

    public void EndTurn()
    {
        CanAct = false;
        StartCoroutine(TurnWait());
    }

    
    IEnumerator TurnWait()
    {
        yield return new WaitForFixedUpdate();

        while (battleController.SomeoneActing)
        {
            yield return new WaitForFixedUpdate();
        }


        TurnEnd?.Invoke(this,null);
    }

   public class TakeDamageEventArgs: EventArgs
    {
        public AttackData attackData;

        public TakeDamageEventArgs(AttackData attackData)
        {
            this.attackData = attackData;
        }
    }

    public class EffectArgs : EventArgs
    {
        public BattleEffect effect;

        public EffectArgs(BattleEffect effect)
        {
            this.effect = effect;
        }
    }

    public void DeathAnimEnded()
    {
        OnDeathAnimEnded?.Invoke(this, null);
    }

    public virtual void GainEXP(int ammount)
    {
        CurentEXP += ammount;
        ExpGained?.Invoke(this, new Character.ExpGainArgs(ammount));
        if(CurentEXP >= MaxEXP)
        {
            LevelUp();
            CurentEXP = 0;
            
        }
    }

    public virtual void LevelUp()
    {
       LeveledUp?.Invoke(this, null);
    }

    public virtual  bool CanLevelUp()
    {
        throw new NotImplementedException();
    }

    public void PushThis(Vector3Int positionToPush)
    {

        Vector3Int destination = GetDestination(positionToPush, out Vector3Int direction);

        if (battleController.IsTileEmptyForPushing(destination))
        {
           
            
            MovePushNoCollision(destination);
        }
        else
        {
           
            MovePushWithCollision(destination, destination - direction);
        }
    }


    private void MovePushNoCollision(Vector3Int pos)
    {
        StartCoroutine(PushMove(pos));
    }

    private void MovePushWithCollision(Vector3Int colidingPos, Vector3Int savePos)
    {
        StartCoroutine(PushMoveCollision(colidingPos, savePos));
    }

    private void OnMoveCollision(BattleObject obj)
    {

        this.TakeDamage(new AttackData(null, GetPushCollisionDamage(), AttackData.DamageType.physical, AttackData.RangeType.other, AttackData.AttackType.other, false));

        if (obj != null)
        {
            if(obj is IKillable)
            {
                (obj as IKillable).TakeDamage(
                    new AttackData(null,GetPushCollisionDamage(), AttackData.DamageType.physical, AttackData.RangeType.other, AttackData.AttackType.other, false)
                    );
            }
        }
    }

    public float GetPushCollisionDamage()
    {
        return BattleController.PushCollisonDamage;
    }

    public static float PushSpeed = 4;

    IEnumerator PushMove(Vector3Int pushPos)
    {
        battleController.AddActor();
        OnMoveStart?.Invoke(this, null);
        animationController.EndAnim_Run(this, null);
        Vector3 pos = tileMap.GetCellCenterWorld(pushPos);
        
        while(transform.position != pos)
        {

            transform.position = Vector3.MoveTowards(transform.position, pos, PushSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        battleController.RemoveActor();
        OnMoveEnd?.Invoke(this, null);
    }

    IEnumerator PushMoveCollision(Vector3Int pushPos, Vector3Int savePos)
    {
        BattleObject collision = battleController.GetObjectOnTile(pushPos);

        battleController.AddActor();
        OnMoveStart?.Invoke(this, null);
        Vector3 pos = tileMap.GetCellCenterWorld(savePos);

        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, PushSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Vector3 npos = pos + ((tileMap.GetCellCenterWorld(pushPos) - pos) / 2.5f);

        while (transform.position != npos)
        {
            transform.position = Vector3.MoveTowards(transform.position, npos, PushSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        OnMoveCollision(collision);

        yield return new WaitForEndOfFrame();


        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, PushSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        battleController.RemoveActor();
        OnMoveEnd?.Invoke(this, null);

        
    }


    public void StartNotAnim_Attack(Vector3Int ownerPos, Vector3Int targetPos)
    {
        StartCoroutine(AttackNotAnimMove(targetPos, ownerPos));
    }
    private IEnumerator AttackNotAnimMove(Vector3Int pushPos, Vector3Int savePos)
    {

        battleController.AddActor();
        Vector3 pos = tileMap.GetCellCenterWorld(savePos);

        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, PushSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Vector3 npos = pos + ((tileMap.GetCellCenterWorld(pushPos) - pos) / 2.5f);

        while (transform.position != npos)
        {
            transform.position = Vector3.MoveTowards(transform.position, npos, PushSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }


        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, PushSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        battleController.RemoveActor();

    }



    private Vector3Int GetDestination(Vector3Int positionToPush, out Vector3Int direction)
    {
        Vector3Int dif = positionToPush - TilePosition;

        direction = new Vector3Int();

        direction.x = Math.Sign(dif.x);
        
        direction.y = Math.Sign(dif.y);



        Vector3Int tempPos = TilePosition + direction;
        while(tempPos != positionToPush && CanPushToPos(tempPos))
        {
            tempPos += direction;
        }

        return tempPos;
    }

    public T[] GetSkillsOfType<T>() where T : Skill
    {
        List<T> skillsOFType = new List<T>();
        foreach(Skill skill in skills)
        {
            if(skill is T)
            {
                skillsOFType.Add(skill as T);
            }
        }

        return skillsOFType.ToArray();
    }

    private bool CanPushToPos(Vector3Int pos)
    {
        
        return battleController.IsTileEmptyForPushing(pos);
    }

    public void AddStats(CharacterStats stats)
    {
        this.stats = stats;
    }

}
