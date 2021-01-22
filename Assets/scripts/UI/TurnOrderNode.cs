using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;



public class TurnOrderNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnEnter;
    public event Action OnExit;

    public Character character;

    TurnOrderShowerMain main;

    public Text indexText;

    public Image portrait;

    public Image baseImage;

    public new RectTransform transform;

    public Image ReyCastImage;

    public int index;

    private Vector3 position;

    CanvasRenderer[] canvasRenderers;

    public Text nameText;

    public Text  HpText;

    float curentAlpha;

    Color curentColor;

    bool isHidden;
    public void Awake()
    {
        transform = GetComponent<RectTransform>();
        position = transform.position;

        canvasRenderers = GetComponentsInChildren<CanvasRenderer>();
    }


    public void PushDown(float ammount)
    {
        if(transform.position == position)
        transform.position -= new Vector3(0, ammount, 0);
    }

    public void ReturnOnNormalPosition()
    {
        transform.position = position;
    }

    public void SetAlpha(float alpha)
    {
        if(alpha != curentAlpha)
        {
            if (alpha == 1)
            {
                isHidden = false;
            }
            else
            {
                isHidden = true;
            }

            curentAlpha = alpha;
            foreach (CanvasRenderer renderer in canvasRenderers)
            {
                renderer.SetAlpha(alpha);
            }
        }
       
    }

    public void SetColor(Color color)
    {
        if (color != curentColor)
        {
            curentColor = color;
            foreach (CanvasRenderer renderer in canvasRenderers)
            {
                renderer.SetColor(color);
            }
        }

    }

    public void FixedUpdate()
    {
        if(character != null)
        {
            HpText.text = character.HP + " / " + character.MaxHP;

           

            nameText.text = character.Name;
        }
    }

    public void SetUp(Character ch, int index, TurnOrderShowerMain main)
    {
        character = ch;

        if(ch.team == BattleObject.Team.player)
        {
            baseImage.sprite = main.allyNodeImage;

        }
        else
        {
            baseImage.sprite = main.enemyNodeImage;
           
        }

        this.index = index;
        this.main = main;
        indexText.text = (index + 1).ToString();

        portrait.sprite = ch.icon;

        ReturnOnNormalPosition();
        if(curentAlpha != 1)
        {
            SetAlpha(1);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (character != null && !isHidden)
        {
            JumpUp();
            OpenUpNode();
            AddHighLightCharacter();
        }
    }

    void OpenUpNode()
    {

        HpText.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);

    }

    public float jumpAmmount;
    void JumpUp()
    {
        transform.position += new Vector3(0, jumpAmmount);
        ReyCastImage.rectTransform.position -= new Vector3(0, jumpAmmount);
    }

    void JumpDown()
    {
        transform.position -= new Vector3(0, jumpAmmount);
        ReyCastImage.rectTransform.position += new Vector3(0, jumpAmmount);
    }

    void CloseNode()
    {

        HpText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
       
    }

    void AddHighLightCharacter()
    {
        if(character.battleController != null)
        {
            character.battleController.AddHighlightedCharacter(character);
        }
    }

    void RemoveHighlightCahracter()
    {
        if (character.battleController != null)
        {
            character.battleController.RemoveHIghlight(character);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(character != null && !isHidden)
        {
            CloseNode();
            JumpDown();
            RemoveHighlightCahracter();

        }
        
    }
}


