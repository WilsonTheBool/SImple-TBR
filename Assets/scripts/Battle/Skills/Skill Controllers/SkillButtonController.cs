using System;
using System.Collections.Generic;
using UnityEngine;


public class SkillButtonController: MonoBehaviour
{
    protected SkillButtonBase button;

    protected ActiveSkill skill;

    protected BattleController battleController;

    protected MainBattleInputManager mainInput;

    [SerializeField]
    protected BattleInputHandler skillInputHandler;

    private void Start()
    {
        mainInput = MainBattleInputManager.MainInputManager;
    }

    public virtual void SetUp(ActiveSkill skill)
    {
        this.skill = skill;
        battleController = BattleController.battleController;
        battleController.ActiveCharacterChanged += BattleController_ActiveCharacterChanged;
    }

    private void BattleController_ActiveCharacterChanged(object sender, EventArgs e)
    {
        
        if(skill.owner != sender as Character && button != null)
        {
            UnsubFromButton();
            button = null;
        }
    }

    protected void UnsubFromButton()
    {
        //skill = null;
    }

    public virtual void OnClick()
    {
        if (!battleController.SomeoneActing)
        {
            HandleClick();
        }
    }

    protected virtual void HandleClick()
    {

    }

    public virtual void AddToButton(SkillButtonBase button)
    {
        this.button = button;
        
        button.SetNewSkill(skill);
    }
}

