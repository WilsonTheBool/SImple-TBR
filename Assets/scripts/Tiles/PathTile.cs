using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;
using System;

public class PathTile : FourWayTile
{
    public List<Vector3Int> curentPath;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int index = GetIndexInPath(location);

        int mask = HasThisTile(tilemap, location + new Vector3Int(0, 1, 0), index) ? 1 : 0;
        mask += HasThisTile(tilemap, location + new Vector3Int(1, 0, 0), index) ? 2 : 0;
        mask += HasThisTile(tilemap, location + new Vector3Int(0, -1, 0), index) ? 4 : 0;
        mask += HasThisTile(tilemap, location + new Vector3Int(-1, 0, 0), index) ? 8 : 0;
        Sprite sprite = GetSprite((byte)mask);

        tileData.sprite = sprite;


        tileData.color = this.color;

        tileData.flags = TileFlags.LockTransform;
        tileData.colliderType = ColliderType.None;


    }

    private Sprite GetSprite(byte mask)
    {
        switch (mask)
        {
            case 0: return None;
            case 3: return Angle_Left_Down;
            case 6: return Angle_Left_Up;
            case 9: return Angle_Right_Down;
            case 12: return Angle_Right_Up;
            case 1: return Only_up;
            case 2: return Only_right;
            case 4: return Only_down;
            case 5: return Left_Right;
            case 10: return Up_Down;
            case 8: return Only_left;
            case 7: return Not_Left;
            case 11: return Not_Down;
            case 13: return Not_Right;
            case 14: return Not_Up;
            case 15: return All;
        }
        return All;
    }

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        int index = GetIndexInPath(location);
        tilemap.RefreshTile(location);
        if (index != 0 )
        {
            tilemap.RefreshTile(curentPath[index - 1]);
        }
        if (index != curentPath.Count - 1)
        {
            tilemap.RefreshTile(curentPath[index + 1]);
        }


    }

    private int GetIndexInPath(Vector3Int position)
    {
        if (curentPath.Contains(position))
            return curentPath.IndexOf(position);
        else
            return -5;
    }


    private bool HasThisTile(ITilemap tilemap, Vector3Int position, int pathIndex)
    {
        if (Math.Abs(GetIndexInPath(position) - pathIndex) == 1)
            return true;
        else
            return false;
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/Battle Tiles/Path Tile")]
    public static void CreatePathTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Path Tile", "New Path Tile", "Asset", "Save Path Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PathTile>(), path);
    }
#endif
}
