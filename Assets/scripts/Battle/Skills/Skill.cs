using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour, IConvertToInfoText
{
    
    public Character owner;

    public string SkillName;

    [TextArea]
    public string SkillDiscriprion;

    public Sprite skillIcon;

    public SkillButtonController controller;

    protected BattleController battleController;

    public virtual void Start()
    {
        owner = GetComponentInParent<Character>();
        battleController = BattleController.battleController;


    }

    public virtual bool CanActivate()
    {
        return false;
    }

    public TraitBase.TraitType GetInfoType()
    {
        return TraitBase.TraitType.other;
    }

    public string GetInfoValueDiscription()
    {
        return SkillDiscriprion;
    }

    public string GetInfoValueName()
    {
        return SkillName;
    }
}
