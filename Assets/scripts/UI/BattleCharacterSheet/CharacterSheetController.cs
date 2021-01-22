using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.UI.BattleCharacterSheet
{
    public class CharacterSheetController: MonoBehaviour
    {
        private BattleController battleController;

        public Character owner;

        [SerializeField]
        private Text NameText;

        [SerializeField]
        private Text HPText;

        [SerializeField]
        private Text LevelText;

        [SerializeField]
        private Text ExpText;

        [SerializeField]
        private Text DefText;

        [SerializeField]
        private Text AttackText;

        [SerializeField]
        private Text SpeedText;

        [SerializeField]
        private Text InitiativeText;


        [SerializeField]
        private Image CharacterIcon;

        [SerializeField]
        private InfoSkillBox[] SkillsInfoTexts;

        [SerializeField]
        private InfoText[] TraitsInfoTexts;

        [SerializeField]
        private InfoText[] ItemsInfoTexts;

        private void Start()
        {
            battleController = BattleController.battleController;

            //CloseInfoWindow();
        }

        private void UpdateStats(Character ch)
        {
            HPText.text = ch.HP.ToString() + " / " + ch.MaxHP.ToString();
            LevelText.text = ch.Level.ToString();
            ExpText.text = ch.CurentEXP.ToString() + " / " + ch.MaxEXP.ToString();

            DefText.text = ch.Def.ToString();
            AttackText.text = ch.Damage.ToString();
            SpeedText.text = ch.speed.ToString();
            InitiativeText.text = ch.Initiative.ToString();
        }

        /// <summary>
        /// НЕ ДОДЕЛАНЫЙ
        /// </summary>
        public void UpdateUI(Character ch)
        {
            gameObject.SetActive(false);

            CharacterIcon.sprite = ch.icon;
            
            NameText.text = ch.Name;

            UpdateStats(ch);
            UpdateSkillsUI(ch);
            UpdateTraitsUI(ch);

            gameObject.SetActive(true);
        }

        private void UpdateSkillsUI(Character ch)
        {
            int i;
            for (i = 0; i < ch.skills.Count; i++)
            {
                SkillsInfoTexts[i].SetValue(ch.skills[i]);
            }

            for (; i < SkillsInfoTexts.Length; i++)
            {
                SkillsInfoTexts[i].Clear();
            }

            
        }

        private void UpdateTraitsUI(Character ch)
        {
            int i;
            for (i = 0; i < ch.traits.Count; i++)
            {
                TraitsInfoTexts[i].SetValue(ch.traits[i]);
            }

            for (; i < TraitsInfoTexts.Length; i++)
            {
                TraitsInfoTexts[i].Clear();
            }
        }

        public void CloseInfoWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
