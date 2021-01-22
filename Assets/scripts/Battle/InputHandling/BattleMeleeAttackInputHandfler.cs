using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class BattleMeleeAttackInputHandfler : BattleInputHandler
{

    private Tilemap pathTileMap;

    private Tilemap movementTileMap;

    private Tile targetEnemy;

    private BattleMessagesController messagesController;

    protected override void SetUp()
    {
        base.SetUp();
        pathTileMap = BattleController.pathTileMap;
        movementTileMap = BattleController.movePlainTileMap;
        targetEnemy = BattleController.enemyTargetTile;
        messagesController = BattleController.messagesController;
        damageText = messagesController.DrawStaticTextSmall(new Vector3(), "", Color.white);

        outlineController = CharacterOutlineSetController.outlineSetController;

        BattleController.SomeActionStarted += OnAnyMove;
    }

    public override void OnHandlerDisabled()
    {
        RemoveText();
    }

    
    public override void HandleInput()
    {
        Character character = BattleController.selectedCharacter;

        if (character == BattleController.actingCharacter && character.CanAct && character is MeleeCharacter)
        {

            if (!BattleController.SomeoneActing)
            {
                Vector3Int closestTile = FindClosestTile(tileMousePosition, mousePosition);
                if (BattleController.PathFinder.SavedNodeContainsPosition(closestTile))
                {
                    List<Vector3Int> path = BattleController.PathFinder.GetPathGlobal(closestTile);


                    UpdatePathTileMap(path, tileMousePosition);
                    BattleController.attackTileMap.SetTile(tileMousePosition, targetEnemy);



                    //on right Click (move)
                    if (Input.GetMouseButtonDown(1))
                    { 
                        (character as MeleeCharacter).AttackMelee(tileMousePosition, path);
                       
                    }
                }

                if(selected == null)
                {
                    selected = BattleController.GetObjectOnTile(tileMousePosition);
                }
                UpdateDamageText(selected);
               
            }





        }

    }

    BattleObject selected;

    public override void MouseSelectedCharacterChanged_Begin(BattleObject newObj)
    {
        outlineController.SetOutline(newObj, CharacterOutlineSetController.ColorType.red);
        selected = newObj;
    }

    public override void MouseSelectedCharacterChanged_End(BattleObject oldObj)
    {
        outlineController.SetToNormal(oldObj);
        selected = null;
    }

    private void OnAnyMove(object mover, EventArgs e)
    {
        outlineController.SetToNormal(selected);
    }

    private Text damageText;

   private void UpdateDamageText(BattleObject target)
    {
        MeleeCharacter melee = BattleController.selectedCharacter as MeleeCharacter;

        if (melee != null && selected != null)
        {
            damageText.gameObject.SetActive(true);
            damageText.transform.position = mousePosition + messagesController.smallTextOffset;

           // if(last != selected)
            damageText.text = "deal " + GetDamage(melee, target).ToString();
        }
        else
        {
            RemoveText();
        }
    }

    private void RemoveText()
    {
        damageText.gameObject.SetActive(false);
    }

    private float GetDamage(MeleeCharacter melee, BattleObject target)
    {
        float damage = melee.Damage;

        if (target is Character)
        {

            damage = melee.GetDamage(damage, false);
            damage = (target as Character).GetDamageTake(damage, false);

        }
        else
        {

            damage = melee.GetDamage(damage, false);
        }
        return damage;
    }

    protected void UpdatePathTileMap(List<Vector3Int> path, Vector3Int target)
    {



        path.Add(target);

        if(BattleController.pathTile is PathTile)
        (BattleController.pathTile as PathTile).curentPath = path;

        foreach (Vector3Int vec in path)
        {
            BattleController.pathTileMap.SetTile(vec, BattleController.pathTile);
        }
        BattleController.pathTileMap.SetTile(target, BattleController.pathTile);

        path.RemoveAt(path.Count - 1);

    }

    private Vector3Int FindClosestTile(Vector3Int attackTile, Vector3 mousePosition)
    {

        Vector3Int bestNeighbour = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        float bestValue = float.MaxValue;

        Vector3Int neighbour;

        neighbour = attackTile + new Vector3Int(1, 0, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            float value = (mousePosition - BattleController.floorTilemap.GetCellCenterWorld(neighbour)).magnitude;
            if (value < bestValue)
            {
                bestNeighbour = neighbour;
                bestValue = value;
            }
        }

        neighbour = attackTile + new Vector3Int(-1, 0, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            float value = (mousePosition - BattleController.floorTilemap.GetCellCenterWorld(neighbour)).magnitude;
            if (value < bestValue)
            {
                bestNeighbour = neighbour;
                bestValue = value;
            }
        }

        neighbour = attackTile + new Vector3Int(0, 1, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            float value = (mousePosition - BattleController.floorTilemap.GetCellCenterWorld(neighbour)).magnitude;
            if (value < bestValue)
            {
                bestNeighbour = neighbour;
                bestValue = value;
            }
        }

        neighbour = attackTile + new Vector3Int(0, -1, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            float value = (mousePosition - BattleController.floorTilemap.GetCellCenterWorld(neighbour)).magnitude;
            if (value < bestValue)
            {
                bestNeighbour = neighbour;
                bestValue = value;
            }
        }

        return bestNeighbour;
    }
}

//public class MoveAttackMeleEventArgs : EventArgs
//{
//    public bool isAttack;

//    public IKillable target;

//    public Vector3Int targetPos;

//    public Vector3Int movePos;

//    public MoveAttackMeleEventArgs(IKillable target, Vector3Int targetPos, Vector3Int movePos)
//    {
//        this.target = target;
//        this.targetPos = targetPos;
//        this.movePos = movePos;
//        isAttack = true;
//    }

//    public MoveAttackMeleEventArgs(Vector3Int movePos)
//    {
//        this.target = null;
//        this.targetPos = movePos;
//        this.movePos = movePos;
//        isAttack = false;
//    }


//}

