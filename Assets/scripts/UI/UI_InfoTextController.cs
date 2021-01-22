using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_InfoTextController : MonoBehaviour
{
    public static UI_InfoTextController InfoTextController;

    //public InfoText

    public BattleAcceptWindow AcceptWindowPrefab;

    public Vector3 AcceptWindowPosition;

    private void Start()
    {
        if(InfoTextController == null)
        {
            InfoTextController = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public BattleAcceptWindow SpawnAcceptWindow()
    {
        return null;
        //return Instantiate<BattleAcceptWindow>(AcceptWindowPrefab, AcceptWindowPosition, new Quaternion(), this.transform);
    }

    public void ShowInfoMessage(string text, Vector3 position)
    {
        //if (!InfoText.gameObject.activeSelf)
        //{
        //    InfoText.gameObject.SetActive(true);
        //}

        //InfoText.UpdateThis(text);
        //InfoText.transform.position = position;
    }
    
    public void RemoveInfoMessage()
    {
        //if (InfoText.gameObject.activeSelf)
        //{
        //    InfoText.gameObject.SetActive(false);
        //}
    }
}
