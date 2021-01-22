using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Character))]
public class BrainBase : MonoBehaviour
{
    private BattleController battleController;
    private Character owner;
    void Start()
    {
        battleController = BattleController.battleController;
        owner = GetComponent<Character>();

    }

    public void ProsessTurn()
    {
        MoveToClosestTileToPlayer();
    }

    void MoveToClosestTileToPlayer()
    {
        battleController.PathFinder.UpdateSavedNodes(owner.TilePosition, owner.speed);
        List<Vector3Int> allAIPoitioons = battleController.PathFinder.GetSavedPositions();
        allAIPoitioons.Add(owner.TilePosition);

        List<Character> chList = battleController.GetAllCharactersInBattle();

       

        Vector3Int bestPos = GetBestPosition(allAIPoitioons, chList, out Character target);

        if(target == null)
        {
            owner.MovePath(battleController.PathFinder.GetPathGlobal(bestPos), false);
        }
        else
        {
            (owner as MeleeCharacter).AttackMelee(target.TilePosition, battleController.PathFinder.GetPathGlobal(bestPos));
        }
        

        owner.EndTurn();

    }

    List<Vector3Int> GetAllPlayerPositions(List<Character> allCharacters)
    {
        List<Vector3Int> result = new List<Vector3Int>();

        foreach(Character ch in allCharacters)
        {
            if(ch.team == BattleObject.Team.player)
            {
                result.Add(ch.TilePosition);
            }
        }

        return result;
    }

    //без приоритетов
    Vector3Int GetBestPosition(List<Vector3Int> allAIPos, List<Character> allCharacters, out Character target)
    {
        target = null;
        float minValue = float.MaxValue;
        Vector3Int minTilePos = new Vector3Int(-1,-1,0);

        foreach(Character ch in allCharacters)
        {
            if(ch.team == BattleObject.Team.player)
            {
                foreach (Vector3Int pos in allAIPos)
                {
                    float value = Vector3Int.Distance(ch.TilePosition, pos) + ch.GetAIThreat();

                    if (value <= minValue)
                    {
                        minValue = value;
                        minTilePos = pos;
                        
                    }

                }
            }
           
        }

        foreach (Character ch in allCharacters)
        {
            if (ch.team == BattleObject.Team.player)
            {
                if(Vector3Int.Distance(ch.TilePosition, minTilePos) < 2)
                {
                    target = ch;
                    break;
                }
            }
        }

        if(minValue != float.MaxValue)
        {
            return minTilePos;
        }
        else
        {
            Debug.LogError("AI не смог найти позиция для движения");
            return minTilePos;
        }


    }
}
