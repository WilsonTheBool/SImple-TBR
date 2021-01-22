using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BattleAcceptWindow : MonoBehaviour
{
    [SerializeField] Button AcceptButton;

    public event Action OnAcceptPressed;

    public void Start()
    {
        AcceptButton.onClick.AddListener(OnAccept);


    }


    public void UpdateText(string message)
    {

    }
        

    public void OnAccept()
    {
        OnAcceptPressed?.Invoke();
    }

  


}
