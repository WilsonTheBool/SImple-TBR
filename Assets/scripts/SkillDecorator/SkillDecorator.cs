using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkillDecorator: MonoBehaviour
{
    public static SkillDecorator skillDecorator;

    private void Awake()
    {
        if(skillDecorator != null)
        {
            Destroy(this);
        }
        else
        {
            skillDecorator = this;
        }
    }

    [SerializeField]
    Tilemap skillEffectTilemap;
    [SerializeField]
    Tilemap skillEffectTilemap_upper;

    [SerializeField]
    public Tile FourWayTile;

    [SerializeField]
    public Tile TargetTile;

    [SerializeField]
    public Material outlineMaterial;

    [SerializeField]
    public Color summonSpellColor;

    [SerializeField]
    public Color summonSpellColor_target;

    public void DrawTilesColored(Vector3Int[] positions, Tile tile, Color color, bool isUpper)
    {
        if (isUpper)
        {
            tile.color = color;

            foreach (Vector3Int pos in positions)
            {
                skillEffectTilemap.SetTile(pos, tile);
            }
        }
        else
        {
            tile.color = color;

            foreach (Vector3Int pos in positions)
            {
                skillEffectTilemap_upper.SetTile(pos, tile);
            }
        }
       
    }

    public void DrawTileColored(Vector3Int pos, Tile tile, Color color, bool isUpper)
    {
        tile.color = color;
        if(isUpper)
            skillEffectTilemap.SetTile(pos, tile);
        else
            skillEffectTilemap_upper.SetTile(pos, tile);

    }

    public void ClearMoveTilemap()
    { 
            BattleController.battleController.movePlainTileMap.ClearAllTiles();
    }

    public void ClearTileMap(bool isUpper)
    {
        if(isUpper)
            skillEffectTilemap.ClearAllTiles();
        else
            skillEffectTilemap_upper.ClearAllTiles();
    }

    public void SetMaterialToObjects(BattleObject[] objects, Material material, Color color)
    {
        material.color = color;
        foreach(BattleObject obj in objects)
        {
            List<Material> materials = new List<Material>(obj.spriteRenderer.materials)
            {
                material
            };

            
            obj.spriteRenderer.materials = materials.ToArray();
        }
    }

    public void DrawCircleColored(Vector3Int start, int range, Tile tile, Color color, Predicate<Vector3Int> canDrawOnTile, bool isFullCircle, bool isUpper)
    {
        if (isFullCircle)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    Vector3Int pos = start + new Vector3Int(x, y, 0);
                    if (canDrawOnTile(pos))
                    {
                        DrawTileColored(pos, tile, color, isUpper);
                    }

                }
            }
        }
        else
        {
            

        }
        
    }

    public void RemoveMaterialFromOBjects(BattleObject[] objects, Material material)
    {
        foreach (BattleObject obj in objects)
        {
            List<Material> materials = new List<Material>(obj.spriteRenderer.materials);
            materials.Remove(material);
            obj.spriteRenderer.materials = materials.ToArray();
        }
    }
}

