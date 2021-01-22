using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.Debug
{
    class PrintSkillsInButon: MonoBehaviour
    {
        SkillsUI_Controller controller;

        Text text;

        private void Start()
        {
            controller = FindObjectOfType<SkillsUI_Controller>();
            text = GetComponent<Text>();
        }

        private void FixedUpdate()
        {
            text.text = "";
            if(controller.skills != null)
            {
                foreach (var skill in controller.skills)
                {
                    if(skill.skill != null)
                    {
                        text.text += skill.skill.SkillName + ";";
                        text.text += skill.skill.owner.Name + "\n";
                    }
                    else
                    {
                        text.text += "NULL\n";
                    }
                    
                }
            }
            else
            {
                text.text = "Contorller.skills == null";
            }
            
        }
    }
}
