using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class FourWayTile : Tile
{
    public Sprite All;
    public Sprite None;

    public Sprite Only_up;
    public Sprite Only_down;
    public Sprite Only_left;
    public Sprite Only_right;

    public Sprite Up_Down;
    public Sprite Left_Right;

    public Sprite Not_Up;
    public Sprite Not_Down;
    public Sprite Not_Left;
    public Sprite Not_Right;

    public Sprite Angle_Right_Up;
    public Sprite Angle_Right_Down;
    public Sprite Angle_Left_Up;
    public Sprite Angle_Left_Down;


    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasThisTile(tilemap, position))
                    tilemap.RefreshTile(position);
            }
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int mask = HasThisTile(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
        mask += HasThisTile(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
        mask += HasThisTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
        mask += HasThisTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;
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

    private bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }


#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/Battle Tiles/Four Way Tile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Four Way Tile", "New Four Way Tile", "Asset", "Save Four Way Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FourWayTile>(), path);
    }
#endif

}
