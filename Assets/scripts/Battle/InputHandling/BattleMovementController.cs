using UnityEngine;
using System.Collections.Generic;
using System;

public class BattleMovementController : BattleInputHandler
{
    //подразумевает что персонаж уже выбран, и уже построен MovePlane для этого персонажа



    public override void HandleInput()
    {
        Character character = BattleController.selectedCharacter;

        if(character.CanAct)
        {

            if (BattleController.movePlainTileMap.HasTile(tileMousePosition) && !BattleController.SomeoneActing)
            {

                List<Vector3Int> path = BattleController.PathFinder.GetPathGlobal(tileMousePosition);


                UpdatePathTileMap(path);
               
                //on right Click (move)
                if (Input.GetMouseButtonDown(1))
                {
                    character.MovePath(path, true);
                }
            }

            



        }

        
    }

    protected override void SetUp()
    {
       
        BattleController.SomeActionStarted += OnAnyMove;
        outlineController = CharacterOutlineSetController.outlineSetController;
    }
    
    private void OnAnyMove(object mover, EventArgs e)
    {
        
    }

   

    protected void UpdatePathTileMap(List<Vector3Int> path)
    {

        

        if (BattleController.pathTile is PathTile)
            (BattleController.pathTile as PathTile).curentPath = path;

        foreach (Vector3Int vec in path)
        {
            BattleController.pathTileMap.SetTile(vec, BattleController.pathTile);
        }
    }

}
