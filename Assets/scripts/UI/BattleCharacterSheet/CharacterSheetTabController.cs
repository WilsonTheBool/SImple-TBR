using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.UI.BattleCharacterSheet
{
    public class CharacterSheetTabController: MonoBehaviour
    {
        [SerializeField]
        CharacterSheetTab[] tabs;

        void Start()
        {
            foreach(CharacterSheetTab tab in tabs)
            {
                tab.OnTabOpen += Tab_OnTabOpen;
            }
        }

        private void Tab_OnTabOpen(object sender, EventArgs e)
        {
            CloseOtherTabs(sender as CharacterSheetTab);
        }

        private void CloseOtherTabs(CharacterSheetTab tab)
        {
            if(tab != null)
            {
                foreach(CharacterSheetTab t in tabs)
                {
                    if(t != tab)
                    {
                        t.Close();
                    }
                }
            }
        }
    }
}
