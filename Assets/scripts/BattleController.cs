using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BattleController : MonoBehaviour
{
    public static float PushCollisonDamage = 5;

    public static BattleController battleController; 

    public event EventHandler SomeoneMoved;
    public event EventHandler SomeActionStarted;

    public event EventHandler SomeInitiativeChanged;
    public event EventHandler<Character.TakeDamageEventArgs> SomeTakeDamage;
    public event EventHandler<Character.ExpGainArgs> SomeExpGained;
    public event EventHandler SomeLevelUp;
    public event EventHandler SomeTurnEnded;
    public event EventHandler SomeTurnStarted;
    public event EventHandler BattleObjectsChanged;
    public event EventHandler OnBattleStart;



    public Tilemap wallTilemap;
    public Tilemap floorTilemap;
    public Tilemap movePlainTileMap;
    public Tilemap pathTileMap;
    public Tilemap attackTileMap;

    public Tile moveTile;
    public Tile pathTile;
    public Tile enemyTargetTile;
    public Tile attackPathTile;
    public Tile spellTargetTile;

    public List<BattleObject> battleObjects;

    public Character selectedCharacter;
    

    public Character actingCharacter
    {
        get
        {
            return selectedCharacter;
        }

        set
        {
            selectedCharacter = value;
        }
    }



    public PathFinder PathFinder;
    public TurnOrderController TurnController;
    public BattleMessagesController messagesController;

    public event EventHandler NewBattleObjectAdded;
    public event EventHandler OnBattleObjectRemoved;
    public event EventHandler ActiveCharacterChanged;



    public Camera camera;

    public GameObject highlightTile;

    protected void Awake()
    {
        camera = FindObjectOfType<Camera>();
        TurnController = FindObjectOfType<TurnOrderController>();
        messagesController = FindObjectOfType<BattleMessagesController>();

        TurnController.NewTurnCycle += TurnController_NewTurnCycle;

        NewBattleObjectAdded += BattleController_NewBattleObjectAdded; ;
        OnBattleObjectRemoved += BattleController_OnBattleObjectRemoved; ;

        var objs = FindObjectsOfType<BattleObject>();

        foreach (BattleObject obj in objs)
        {
            AddNewBattleObject(obj);
        }



        if (battleController == null)
        {
            battleController = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        StartCoroutine(FakeBattleStarter());
    }



    private IEnumerator FakeBattleStarter()
    {
        AddActor();
        yield return new WaitForSeconds(0.5f);
        OnBattleStart?.Invoke(this,null);
        RemoveActor();
    }
    public  void AddHighlightedCharacter(Character ch)
    {
        highlightTile.transform.position = floorTilemap.GetCellCenterWorld(ch.TilePosition);
    }

    public void RemoveHIghlight(Character ch)
    {
        if(highlightTile.transform.position == floorTilemap.GetCellCenterWorld(ch.TilePosition))
        {
            highlightTile.transform.position = new Vector3(0, 0, -100);
        }
    }

    public void SetActiveCharacter(Character ch)
    {
        //selectedCharacter != ch 
        if (true)
        {
            selectedCharacter = ch;
            ActiveCharacterChanged?.Invoke(ch, null);
            ch.StartTurn();
        }
        
        
    }

    /// <summary>
    /// is tile empty and can walk on
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool IsTileEmpty(Vector3Int tilePos)
    {
        if(floorTilemap.HasTile(tilePos))
        {
            return (GetObjectOnTile(tilePos) == null || GetObjectOnTile(tilePos).canWalkOn);
        }
        else
        {
            return false;
        }
        
    }
    public bool IsTileEmptyForPushing(Vector3Int tilePos)
    {
        if (wallTilemap.HasTile(tilePos))
        {
            return false;
        }
        else
        {
            return (GetObjectOnTile(tilePos) == null || GetObjectOnTile(tilePos).canWalkOn);
        }

    }
    public BattleObject GetObjectOnTile(Vector3Int tilePos)
    {
        foreach(BattleObject obj in battleObjects)
        {
            if(obj is Character && obj.TilePosition == tilePos)
            {
                return obj;
            }
        }

        foreach (BattleObject obj in battleObjects)
        {
            if (obj.TilePosition == tilePos)
            {
                return obj;
            }
        }

        return null;
    }

    public T GetObjectOnTile<T> (Vector3Int tilePos) where T : BattleObject
    {
        foreach (BattleObject obj in battleObjects)
        {
            if (obj is T && obj.TilePosition == tilePos)
            {
                return obj as T;
            }
        }

        return null;
    }
    public TileBase GetFloorTile(Vector3Int position)
    {
        return floorTilemap.GetTile(position);
    }

    public bool AddNewBattleObject(BattleObject objToAdd)
    {
        objToAdd.PositionCahnged += OnObjectPositionChanged;

        if(objToAdd is IMoveable)
        {
            (objToAdd as IMoveable).OnMoveStart += BattleController_OnMoveStart;
            (objToAdd as IMoveable).OnMoveEnd += BattleController_OnMoveEnd;
        }

        if(objToAdd is IMeleeAttacker)
        {
            (objToAdd as IMeleeAttacker).OnBeforeAttack += BattleController_OnMoveStart;
            (objToAdd as IMeleeAttacker).OnAfterAttack += BattleController_OnMoveEnd;
        }

        if (objToAdd is IRangeAttacker)
        {
            (objToAdd as IRangeAttacker).BeforeRangeAttack += BattleController_BeforeRangeAttack;
        }

        if (objToAdd is IKillable)
        {
            (objToAdd as IKillable).OnDeath += BattleController_OnDeath;
            
            
        }

        if(objToAdd is ICanLevelUp)
        {
            (objToAdd as ICanLevelUp).ExpGained += BattleController_ExpGained;
        }

        if (objToAdd is Character)
        {
            (objToAdd as Character).TurnEnd += BattleController_TurnEnd;
            (objToAdd as Character).TurnStart += BattleController_TurnStart;
            (objToAdd as Character).OnAfterTakeDamage += BattleController_OnDamageTaken;
            (objToAdd as Character).LeveledUp += BattleController_LeveledUp; ;

        }

        battleObjects.Add(objToAdd);

        NewBattleObjectAdded?.Invoke(objToAdd,null);

        return true;
    }

    private void BattleController_LeveledUp(object sender, EventArgs e)
    {
        SomeLevelUp?.Invoke(sender, e);
    }

    private void BattleController_BeforeRangeAttack(object sender, AttackEventArgs e)
    {
        SomeActionStarted?.Invoke(sender, e);
    }

    private void BattleController_ExpGained(object sender, Character.ExpGainArgs e)
    {
        SomeExpGained?.Invoke(sender, e);
    }

    private void BattleController_OnDamageTaken(object sender, Character.TakeDamageEventArgs e)
    {
        SomeTakeDamage?.Invoke(sender, e);
    }

    private void BattleController_OnDeath(object sender, EventArgs e)
    {
        RemoveBattleObject(sender as BattleObject);

        OnBattleObjectRemoved?.Invoke(sender, e);

    }

    void RemoveBattleObject(BattleObject battleObject)
    {
        battleObjects.Remove(battleObject);

    }

    private void BattleController_TurnStart(object character, EventArgs e)
    {
        SomeTurnStarted?.Invoke(character, e);
    }

    private void BattleController_TurnEnd(object character, EventArgs e)
    {
        SomeTurnEnded?.Invoke(character, e);


    }

    private void BattleController_OnMoveEnd(object sender, EventArgs e)
    {
        ActorsCount -= 1;
    }

    public event EventHandler SomeMoveStarted;
    private void BattleController_OnMoveStart(object sender, EventArgs e)
    {
        ActorsCount += 1;
        SomeMoveStarted?.Invoke(sender, e);
        SomeActionStarted?.Invoke(sender, e);
    }

    private void OnObjectPositionChanged(object obj, EventArgs e)
    {
        SomeoneMoved?.Invoke(obj, null);

        
    }

   
    public int ActorsCount = 0;

    public bool SomeoneActing
    {
        get
        { 
            return ActorsCount != 0;
        }
        
    }

    public bool CanMoveNotForced(Character obj)
    {
        if(obj == actingCharacter && obj.CanAct && !SomeoneActing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetGlobalFromTilePosition(Vector3Int tilePos)
    {
        return floorTilemap.GetCellCenterWorld(tilePos);
    }

   public List<Character> GetAllCharactersInBattle()
    {
        List<Character> chList = new List<Character>();
        
        foreach(BattleObject obj in battleObjects)
        {
            if(obj is Character)
            {
                chList.Add(obj as Character);
            }
        }

        return chList;
    }

   

    private void BattleController_NewBattleObjectAdded(object sender, EventArgs e)
    {
        BattleObjectsChanged?.Invoke(sender, null);
    }

    private void BattleController_OnBattleObjectRemoved(object sender, EventArgs e)
    {
        BattleObjectsChanged?.Invoke(sender, null);
    }

    private void TurnController_NewTurnCycle(object sender, EventArgs e)
    {
        
        RegenActionsOnNewCycle();
    }

    private void RegenActionsOnNewCycle()
    {
        foreach(BattleObject obj in battleObjects)
        {
            if(obj is ICanAct)
            {
                (obj as ICanAct).CanAct = true;
            }
        }
    }

    public List<Character> GetAllActiveCharacters()
    {
        List<Character> returnList = new List<Character>();

        foreach(BattleObject obj in battleObjects)
        {
            if(obj is Character)
            {
                Character character = obj as Character;

                if (character.CanAct)
                {
                    returnList.Add(character);
                }
            }
        }

        return returnList;
    }

    public List<Character> GetAllInactiveCharacters()
    {
        List<Character> returnList = new List<Character>();

        foreach (BattleObject obj in battleObjects)
        {
            if (obj is Character)
            {
                Character character = obj as Character;

                if (!character.CanAct)
                {
                    returnList.Add(character);
                }
            }
        }

        return returnList;
    }

   
   public void AddActor()
    {
        ActorsCount += 1;

    }

    public void RemoveActor()
    {
        ActorsCount -= 1;
    }

    public void SetTImedActor(float seconds)
    {

    }

    //unsafe
    public void ForceEndCurentTurn()
    {
        selectedCharacter.EndTurn();
    }
}
