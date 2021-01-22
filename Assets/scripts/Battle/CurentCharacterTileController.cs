using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BattleController))]
public class CurentCharacterTileController : MonoBehaviour
{
    BattleController battleController;

    [SerializeField]
    GameObject tilePrefab;
    void Awake()
    {
        battleController = GetComponent<BattleController>();

        battleController.ActiveCharacterChanged += BattleController_ActiveCharacterChanged;
        battleController.OnBattleStart += BattleController_ActiveCharacterChanged;
        battleController.SomeActionStarted += BattleController_SomeActionStarted;
        battleController.SomeActionStarted += BattleController_SomeActionStarted;
    }

    private void BattleController_SomeActionStarted(object sender, System.EventArgs e)
    {
        HideTile();
    }

    private void BattleController_ActiveCharacterChanged(object sender, System.EventArgs e)
    {
        ShowTile();
    }

    void ShowTile()
    {
        

        Character ch = battleController.selectedCharacter;
        if(ch != null)
        {
            CharacterOutlineSetController.outlineSetController?.SetOutline(ch, CharacterOutlineSetController.ColorType.white);

            tilePrefab.transform.position = ch.tileMap.CellToWorld(ch.TilePosition);
            tilePrefab.gameObject.SetActive(true);
        }

    }

    void HideTile()
    {
        Character ch = battleController.selectedCharacter;

        CharacterOutlineSetController.outlineSetController.SetToNormal(ch);

        tilePrefab.gameObject.SetActive(false);
    }
}
