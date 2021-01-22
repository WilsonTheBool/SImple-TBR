using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour
{

    public static CursorController cursorController;

    BattleController battleController;

    public Texture2D Cursor_sword;
    public Texture2D Cursor_crossbow;
    public Texture2D Cursor_hand;
    public Texture2D Cursor_default;
    public Texture2D Cursor_Move;
    public Texture2D Cursor_wait;
    public Texture2D Cursor_info;



    void Start()
    {
        battleController = FindObjectOfType<BattleController>();

        if (cursorController != null)
        {
            Destroy(this);
        }
        else
        {
            cursorController = this;
        }
    }

   

    public enum CursorType
    {
        def, sword, crossbow, hand, boots, wait, info,
    }

    public void SetCursor(CursorType cursorType)
    {
        Cursor.SetCursor(GetTexture(cursorType), Vector2.zero, CursorMode.ForceSoftware);
        
    }

    private Texture2D GetTexture(CursorType type)
    {
        switch (type)
        {
            case CursorType.info:
                {
                    return Cursor_info;
                }

            case CursorType.sword:
                {
                    return Cursor_sword;
                }
            case CursorType.crossbow:
                {
                    return Cursor_crossbow;

                }
            case CursorType.hand:
                {
                    return Cursor_hand;
                }
            case CursorType.boots:
                {
                    return Cursor_Move;
                }
            case CursorType.wait:
                {
                    return Cursor_wait;
                }
            default:
                {
                    return Cursor_default;
                }
        }
    }

    
}
