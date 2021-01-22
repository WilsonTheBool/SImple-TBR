using UnityEngine;
using Assets.scripts.UI.BattleCharacterSheet;
using System.Collections;

public class BattleGetInfoHandler : BattleInputHandler
{
    [SerializeField]
    private CharacterSheetController Controller;

    protected override void SetUp()
    {
        base.SetUp();

        if(Controller == null)
        {
            FindObjectOfType<CharacterSheetController>();
        }

        outlineController = CharacterOutlineSetController.outlineSetController;
    }

    public override void MouseSelectedCharacterChanged_Begin(BattleObject newObj)
    {
        if(newObj != BattleController.selectedCharacter)
            outlineController.SetOutline(newObj, CharacterOutlineSetController.ColorType.black);
    }

    public override void MouseSelectedCharacterChanged_End(BattleObject oldObj)
    {
        if (oldObj != BattleController.selectedCharacter)
            outlineController.SetToNormal(oldObj);
    }

    public override void HandleInput()
    {
        Character ch = BattleController.GetObjectOnTile<Character>(tileMousePosition);
        if (ch != null)
        if (Input.GetMouseButtonDown(1))
        {
                Controller.UpdateUI(ch);
        }
    }
}
