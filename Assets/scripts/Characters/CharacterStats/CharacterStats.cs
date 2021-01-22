using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CharacterStats : MonoBehaviour
{
    //HpBar 

    private Vector3 statsOFfSet;
    private Vector3 hugeOFfSet;

    public  Character owner;

    private TurnOrderController orderController;

    HpBar hpBar;

    public Text hpText;

    public Text turnText;

    public Text NameText;

    public GameObject turnHolder;

    public void SetUp(Vector3 offSet, Vector3 hugeOffset, Character owner, TurnOrderController turnOrderController)
    {
        statsOFfSet = offSet;
        this.hugeOFfSet = hugeOffset;
        this.owner = owner;
        orderController = turnOrderController;
        StartUp();
        owner.AddStats(this);
    }

    private void Awake()
    {   
        hpBar = GetComponentInChildren<HpBar>();
    }

    private void StartUp()
    {
        owner.OnAfterTakeDamage += Owner_OnAfterTakeDamage;

        orderController.TurnOrderUpdated += OrderController_TurnOrderUpdated;

        owner.OnDeath += Owner_OnDeath;

        if (owner.isBigModel)
        {
            NameText.gameObject.transform.Translate(hugeOFfSet);
            hpText.gameObject.transform.Translate(hugeOFfSet);
        }

        UpdateTurnText();
        UpdateHpText();
        UpdateHpBar();
        UpdateName();

        HideToShort();
    }

    private void Owner_OnDeath(object sender, EventArgs e)
    {

        StartCoroutine(WhaitForHpBar());
    }

    private IEnumerator WhaitForHpBar()
    {
        while (hpBar.isWhiteActive)
        {
            yield return new WaitForFixedUpdate();
        }

        HideAllStats();
    }

    public void HideAllStats()
    {
        
        RectTransform[] objs = GetComponentsInChildren<RectTransform>();

        foreach(RectTransform obj in objs)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public void HideToShort()
    {
        NameText.gameObject.SetActive(false);
        hpText.gameObject.SetActive(false);
        turnHolder.gameObject.SetActive(false);
    }

    public void ShowAllStats()
    {
        NameText.gameObject.SetActive(true);
        hpText.gameObject.SetActive(true);
        turnHolder.gameObject.SetActive(true);

    }

    private void Update()
    {
        transform.position = owner.transform.position + statsOFfSet;
    }

    private void OrderController_TurnOrderUpdated()
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        int index = orderController.GetIndexOFCharacter(owner);

        if(index < 0)
        {
            turnText.text = "";
        }
        else
        {
            turnText.text = index.ToString();
        }
    }

    private void Owner_OnAfterTakeDamage(object sender, Character.TakeDamageEventArgs e)
    {
        UpdateHpBar();
        UpdateHpText();
    }

    private void UpdateHpText()
    {
        if(owner.HP >= 0)
        {
            hpText.text = owner.HP.ToString() + " / " + owner.MaxHP.ToString();
        }
        else
        {
            hpText.text = "0" + " / " + owner.MaxHP.ToString();
        }
        
    }

    private void UpdateHpBar()
    {
        hpBar.SetHpBar(owner.HP, owner.MaxHP);

    } 
    private void UpdateName()
    {
        NameText.text = owner.Name;

    }
}

