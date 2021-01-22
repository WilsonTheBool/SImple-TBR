using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NoTargetSkillController : SkillButtonController
{
    private bool isInfoWindowActive;

    private BattleAcceptWindow window;

    public BattleInputHandler NoTargetSkillInput;

    private NoTargetSkill noTargetSkill;

    private MainBattleInputManager mainInput;

    private SkillsUI_Controller controller;

    private void Start()
    {
        mainInput = FindObjectOfType<MainBattleInputManager>();
        controller = FindObjectOfType<SkillsUI_Controller>();

        
    }

    public override void SetUp(ActiveSkill skill)
    {
        base.SetUp(skill);

        print(skill is NoTargetSkill);

        noTargetSkill = skill as NoTargetSkill;
    }

    protected override void HandleClick()
    {
        if(skill.CanActivate())
        if (isInfoWindowActive)
        {
            Activate();
            RemoveAcceptWindow();
        }
        else
        {
            SpawnAcceptWindow();
        }
    }

    //do the skill action
    protected virtual void Activate()
    {
        mainInput.SetToSwithching();
        noTargetSkill.Activate(null);
    }

    private void SpawnAcceptWindow()
    {
        window = UI_InfoTextController.InfoTextController.SpawnAcceptWindow();
        window.UpdateText(noTargetSkill.InfoWindowText);
        window.OnAcceptPressed += HandleClick;
        isInfoWindowActive = true;
        mainInput.SetInputPermanent(NoTargetSkillInput);
    }

    public void RemoveAcceptWindow()
    {
        isInfoWindowActive = false;
        window.OnAcceptPressed -= HandleClick;
        Destroy(window.gameObject);
        window = null;
        mainInput.SetToSwithching();
    }

    public override void AddToButton(SkillButtonBase button)
    {
        base.AddToButton(button);
        UpdateButton();
    }

    private void UpdateButton()
    {
        if (button != null)
        {

            button.SetToCooldown(skill.cooldown);

        }



    }
}

