using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaticMessage : MonoBehaviour
{
    [SerializeField]
    Text text;


    public void Open(string message)
    {
        text.text = message;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
