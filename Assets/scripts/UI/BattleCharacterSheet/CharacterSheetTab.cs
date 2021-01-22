using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.scripts.UI.BattleCharacterSheet
{
    public class CharacterSheetTab: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        Sprite ClosedTab;

        [SerializeField]
        Sprite OpenTab;

        [SerializeField]
        GameObject TabContent;

        [SerializeField]
        bool isOpen;

        Image image;

        [SerializeField]
        Color OpenColor;

        [SerializeField]
        Color ClosedColor;


        public event EventHandler OnTabOpen;

        public void Start()
        {
            image = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isOpen)
            {
                Open();
            }
        }

        public void Open()
        {
            if (!isOpen)
            {
                image.sprite = OpenTab;
                image.color = OpenColor;
                TabContent.SetActive(true);
                isOpen = true;
                OnTabOpen?.Invoke(this, null);

            }
            
        }

        public void Close()
        {
            if (isOpen)
            {
                image.sprite = ClosedTab;
                image.color = ClosedColor;
                isOpen = false;
                TabContent.SetActive(false);
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isOpen)
                image.color = OpenColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!isOpen)
                image.color = ClosedColor;
        }
    }
}
