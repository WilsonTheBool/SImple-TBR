using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
public class AnimatedTIle : Tile
{
    public Sprite[] allSprites;

    public float speed;

    public float randomOffSet;

    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        tileAnimationData.animatedSprites = allSprites;
        tileAnimationData.animationSpeed = speed;
        tileAnimationData.animationStartTime = Random.Range(0, randomOffSet);

        return true;

        
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create an Asset
    [MenuItem("Assets/Create/Battle Tiles/AnimatedTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Animated Tile", "New Animated Tile", "Asset", "Save Animated Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AnimatedTIle>(), path);
    }
#endif
}
