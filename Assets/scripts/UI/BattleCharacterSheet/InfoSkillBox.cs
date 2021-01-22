using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Image))]
public class InfoSkillBox : InfoText
{

    Skill skill;
    Image image;

    [SerializeField]
    Sprite emptySprite;

    protected override void Start()
    {
        image = GetComponent<Image>();
        staticMessageController = StaticMessageController.staticMessageController;
    }

    public override void SetValue(IConvertToInfoText obj)
    {
        skill = obj as Skill;
        convertedObject = obj;
        image.sprite = skill.skillIcon;
    }

    

    public override void Clear()
    {
        convertedObject = null;
        skill = null;
        image.sprite = emptySprite;
    }
}
