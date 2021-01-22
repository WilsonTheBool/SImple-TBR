using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class SkillsUI_Controller : MonoBehaviour
{
    BattleController BattleController;

    public Image portrait;

    public RectTransform[] transforms;

    public SkillButtonBase[] skills;

    public Image selectImage;

    public Text CharacterInfo;

    public Action<Skill> ButtonPressed;


    public void UpdateUI(Character character)
    {

        portrait.sprite = character.icon;

        CharacterInfo.text = character.Name;

        int i;

        //Destroy not valid buttons
        for (i = 0; i < skills.Length; i++)
        {
            if (skills[i] != null)
            {
                skills[i].SetDefault();

            }
        }

        int count = 0;
        if (character.team == BattleObject.Team.player)
        {

            foreach (ActiveSkill skill in character.GetSkillsOfType<ActiveSkill>())
            {
               
               skill.controller.AddToButton(skills[count]);
                count++;
            }
        }
        

        
    }

    

    private void Start()
    {
        BattleController = FindObjectOfType<BattleController>();
        BattleController.ActiveCharacterChanged += BattleController_ActiveCharacterChanged;
        BattleController.OnBattleStart += BattleController_OnBattleStart;

        
    }

    private void BattleController_OnBattleStart(object sender, EventArgs e)
    {
        UpdateUI(BattleController.actingCharacter);
    }

    public void ShowInfoMessage(string message)
    {

    }

    private void BattleController_ActiveCharacterChanged(object sender, System.EventArgs e)
    {
        UpdateUI(BattleController.actingCharacter);
    }

   
}
