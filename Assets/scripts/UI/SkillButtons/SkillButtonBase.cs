using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.TextCore;

public class SkillButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SkillsUI_Controller controller;

    [SerializeField] Image UpperImage;
    [SerializeField] Image LowerImage;

    public ActiveSkill skill;

    [SerializeField] Image skillImage;

    [SerializeField] Sprite normelSprite;
    [SerializeField] Sprite onCooldownSprite;
    [SerializeField] Sprite AciveSprite;
    [SerializeField] Sprite onHoverSprite;

    [SerializeField] Sprite defaultSprite;

    public buttonState state;

    public KeyCode keyCode;
    [SerializeField] Text keyCodeText;
    public void Start()
    {
        keyCodeText.text = GetKeyString(keyCode);

        controller = GetComponentInParent<SkillsUI_Controller>();
    }

    public void Update()
    {
        CheckButtonInput();
    }

    private void CheckButtonInput()
    {
        if (Input.GetKeyUp(keyCode))
        {
            Activate();
            
        }
    }

    private void Activate()
    {

        
        if (skill != null)
        {
            controller.ButtonPressed?.Invoke(skill);
            skill.controller.OnClick();
        }
    }

    private string GetKeyString(KeyCode keyCode)
    {
        return ((int)keyCode - (int)KeyCode.Alpha0).ToString();
    }

    public enum buttonState
    {
        
        normal, active, inCooldown, disabled
    }

    public void SetNewSkill(ActiveSkill skill)
    {
        this.skill = skill;
        skillImage.sprite = skill.skillIcon;
        
        if (!skillImage.gameObject.activeSelf)
        {
            skillImage.gameObject.SetActive(true);
        }
        
    }

  

    public void SetDefault()
    {
        this.skill = null;
        skillImage.sprite = defaultSprite;
        skillImage.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Activate();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ShowSkillInfo();
        SetHovered();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        HideInfo();
        SetNotHovered();
    }

    private void ShowSkillInfo()
    {
        Vector3 offSet = (skillImage.rectTransform).sizeDelta / 2;

        StaticMessageController.staticMessageController.ShowMessage(this, this.transform.position, skill.SkillDiscriprion);
        
    }

    private void SetHovered()
    {
        isSelected = true;
        if (!lastActive)
        {
            UpperImage.gameObject.SetActive(true);
            UpperImage.sprite = onHoverSprite;
        }

        
    }

    bool isSelected = false;

    private void SetNotHovered()
    {

        isSelected = false;
        
        if (!lastActive)
        {
            UpperImage.gameObject.SetActive(false);
        }

    }

    private void HideInfo()
    {
        StaticMessageController.staticMessageController.HideMessage(this);

    }

    private bool lastActive = false;
    public void SetToActive(bool isActive)
    {
        if(lastActive != isActive)
        {
            if (isActive)
            {
                UpperImage.sprite = AciveSprite;
                UpperImage.gameObject.SetActive(true);
            }
            else
            {
                if (!isSelected)
                {
                    UpperImage.gameObject.SetActive(false);
                }
                
            }
            lastActive = isActive;
        }
    }

    public void SetToNormal()
    {

    }

    [SerializeField] TMPro.TextMeshProUGUI cooldownText;
    public void SetToCooldown(int cooldown)
    {
        if (skill.isCooldown())
        {
            if (!LowerImage.gameObject.activeSelf)
            {
                LowerImage.gameObject.SetActive(true);
            }

            cooldownText.text = cooldown.ToString();


        }
        else
        {
            LowerImage.gameObject.SetActive(false);
        }
        
    }

   
}
