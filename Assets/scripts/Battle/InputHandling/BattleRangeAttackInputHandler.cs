using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;

public class BattleRangeAttackInputHandler : BattleInputHandler
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
        outlineController = CharacterOutlineSetController.outlineSetController;
        messagesController = BattleController.messagesController;
        damageText = messagesController.DrawStaticTextSmall(new Vector3(), "", Color.white);

        BattleController.SomeActionStarted += OnAnyMove;
    }

    BattleObject selected;
    public override void OnHandlerDisabled()
    {
        RemoveText();
    }
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
        
    }

    private Text damageText;


    private void UpdateDamageText(BattleObject target)
    {
        RangedCharacter ranged = BattleController.selectedCharacter as RangedCharacter;

        if (ranged != null && selected != null)
        {
            damageText.gameObject.SetActive(true);
            damageText.transform.position = mousePosition + messagesController.smallTextOffset;

            // if(last != selected)
            damageText.text = "deal " + GetDamage(ranged, target).ToString();
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

    private float GetDamage(RangedCharacter ranged, BattleObject target)
    {
        float damage = ranged.rangeDamage;

        if (target is Character)
        {

            damage = ranged.GetRangeDamage(damage, false);
            damage = (target as Character).GetDamageTake(damage, false);

        }
        else
        {

            damage = ranged.GetDamage(damage, false);
        }
        return damage;
    }

    public override void HandleInput()
    {
        Character character = BattleController.selectedCharacter;

        if (character.CanAct && character is RangedCharacter)
        {

            if (!BattleController.SomeoneActing)
            {

                BattleController.attackTileMap.SetTile(tileMousePosition, targetEnemy);
                //on right Click (move)
                if (Input.GetMouseButtonDown(1))
                {
                    (character as RangedCharacter).StartAttackRange(tileMousePosition);

                }
            }


        }

        UpdateDamageText(selected);



    }

}

