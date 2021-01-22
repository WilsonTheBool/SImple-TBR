using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GameObject))]
public class CharacterOutlineSetController : MonoBehaviour
{

    public static CharacterOutlineSetController outlineSetController;

    public Material blackOutline;
    public Material redOutline;
    public Material greenOutline;
    public Material whiteOutline;

    public Material normal;


    private void Start()
    {
        if(outlineSetController != null) 
        {
            Destroy(this);
        }
        else
        {
            outlineSetController = this;
        }

    }

    public enum ColorType
    {
        white,
        black,
        red,
        green,
    }

    public void SetToNormal(BattleObject obj)
    {
        if(obj != null && obj.spriteRenderer != null)
        obj.spriteRenderer.material = normal;
    }

    public void SetOutline(BattleObject obj, ColorType type)
    {
        if (obj.spriteRenderer != null)
        {
            switch (type)
            {
                case ColorType.white:
                    {
                        obj.spriteRenderer.material = whiteOutline;
                        break;
                    }
                case ColorType.black:
                    {
                        obj.spriteRenderer.material = blackOutline;
                        break;
                    }
                case ColorType.red:
                    {
                        obj.spriteRenderer.material = redOutline;
                        break;
                    }
                case ColorType.green:
                    {
                        obj.spriteRenderer.material = greenOutline;
                        break;
                    }
            }
        }
            
    }
}
