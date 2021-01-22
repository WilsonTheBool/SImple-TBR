using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using System;

public class MovePathPlaneController : MonoBehaviour
{
    public BattleController battleController;
    public Camera camera;
    // Use this for initialization
    void Start()
    {
        battleController = FindObjectOfType<BattleController>();

        //battleController.SomeoneMoved += ClearMovePlain;

        battleController.SomeTurnStarted += TryUpdate;
        battleController.OnBattleObjectRemoved += TryUpdate;
        battleController.NewBattleObjectAdded += TryUpdate;
        battleController.SomeoneMoved += TryUpdate;
        battleController.SomeActionStarted += ClearMovePlain;

        TryUpdate(null, null);
    }

    public void ClearMovePlain(object mover, EventArgs e)
    {
        battleController.movePlainTileMap.ClearAllTiles();
    }

    private bool isUpdating = false;

    private void TryUpdate(object obj, EventArgs e)
    {
        if (!isUpdating)
        {
            StartCoroutine(TryUpdateWait());
        }
    }
    private IEnumerator TryUpdateWait()
    {
        isUpdating = true;
        while (battleController.SomeoneActing)
        {
            yield return new WaitForFixedUpdate();
        }

        UpdateMovePlain();
        isUpdating = false;

    }
    public void UpdateMovePlain()
    {


        int range = battleController.actingCharacter.speed;

        battleController.PathFinder.UpdateSavedNodes(battleController.actingCharacter.TilePosition, range);

        Vector3Int[] tilesPos = battleController.PathFinder.GetSavedPositions().ToArray();



        Tile[] tiles = new Tile[tilesPos.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = battleController.moveTile;
        }
        battleController.movePlainTileMap.ClearAllTiles();
        battleController.movePlainTileMap.SetTiles(tilesPos, tiles);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = battleController.floorTilemap.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

        

        if (Input.GetMouseButtonDown(0))
        {
           
        }

        //if (battleController.movePlainTileMap.HasTile(pos))
        //{
        //    List<Vector3Int> path = battleController.PathFinder.GetPathGlobal(pos);
        //    battleController.pathTileMap.ClearAllTiles();
        //    foreach (Vector3Int vec in path)
        //    {
        //        battleController.pathTileMap.SetTile(vec, battleController.pathTile);
        //    }
        //}
    }
}
