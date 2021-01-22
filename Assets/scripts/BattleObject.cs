using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleObject : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public Tilemap tileMap;

    public Team team;

    public enum Team
    {
        player, enemy, other
    }

    [HideInInspector]
    public BattleController battleController;

    public bool canWalkOn;
    public Vector3Int TilePosition
    {
        
        get
        {
            if(tileMap != null)
            {
                return tileMap.WorldToCell(transform.position);
            }
            else
            {
                throw new Exception("Tilemap not added", new ArgumentNullException());
            }
        }
    }

    public event EventHandler PositionCahnged;

    private Vector3Int lastPos = new Vector3Int(0,0,0);

    protected virtual void SetUp()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Start()
    {
        battleController = BattleController.battleController;
        
    }

    protected virtual void FixedUpdate()
    {
        if(lastPos != TilePosition)
        {
            PositionCahnged?.Invoke(this,null);
        }

        lastPos = TilePosition;
    }

    public bool IsOpositeTeam(Team team1, Team team2)
    {
        if (team1 == Team.other || team2 == Team.other)
        {
            return false;
        }

        if (team1 == Team.player && team2 == Team.player)
        {
            return false;
        }

        if (team1 == Team.enemy && team2 == Team.enemy)
        {
            return false;
        }

        return true;
    }
}


