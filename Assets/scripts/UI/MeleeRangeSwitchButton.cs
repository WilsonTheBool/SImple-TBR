using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MeleeRangeSwitchButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BattleController battleController;

    public Image image;

    [SerializeField] private KeyCode keyCode;

    [SerializeField] [TextArea ]private string InfoText;

    [SerializeField] private Image keyImage;

    [SerializeField] private Sprite RangeSprite;
    [SerializeField] private Sprite MeleeSprite;
    
    public void OnClick()
    {
        if(!battleController.SomeoneActing)
        if(battleController.selectedCharacter is RangedCharacter && battleController.selectedCharacter.team == BattleObject.Team.player)
        {
                (battleController.selectedCharacter as RangedCharacter).ForceToMelee();

                UpdateButton(battleController.selectedCharacter);

        }
    }

    private void UpdateButton(Character ch)
    {

        if (ch is RangedCharacter && ch.team == BattleObject.Team.player)
        {
            image.enabled = true;
            keyImage.gameObject.SetActive(true);

            RangedCharacter r = ch as RangedCharacter;
            if (r.forceToMelee)
            {
                image.sprite = MeleeSprite;
            }
            else
            {
                image.sprite = RangeSprite;
            }
        }
        else
        {
            image.enabled = false;
            keyImage.gameObject.SetActive(false);
        }

    }

    private void Start()
    {
        battleController = BattleController.battleController;
        battleController.OnBattleStart += BattleController_OnBattleStart;
        battleController.ActiveCharacterChanged += BattleController_OnBattleStart;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            OnClick();
        }
    }

    private void BattleController_OnBattleStart(object sender, System.EventArgs e)
    {
        UpdateButton(battleController.selectedCharacter);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 offSet = image.rectTransform.sizeDelta / 2;
        UI_InfoTextController.InfoTextController.ShowInfoMessage(InfoText, image.rectTransform.position + offSet);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_InfoTextController.InfoTextController.RemoveInfoMessage();
    }
}
