using UnityEngine;
using System.Collections;

public class StaticMessageController : MonoBehaviour
{
    public static StaticMessageController staticMessageController;

    private StaticMessage staticMessage;

    [SerializeField]
    private Canvas mainCanvas;

    public  StaticMessage StaticMessage 
    {
        get
        {
            if (staticMessage != null)
            {
                return staticMessage;
            }
            else
            {
                staticMessage = Instantiate(messagePrefab, mainCanvas.transform).GetComponent<StaticMessage>();
                return staticMessage;
            }
        }
        private set
        {
            staticMessage = value;
        }
    }

    [SerializeField]
    private GameObject messagePrefab;

    [SerializeField]
    Color positiveColor;    
    [SerializeField]
    Color negativeColor;    
    [SerializeField]
    Color regularColor;

    private System.Object LastCaller;

    private void Awake()
    {
        if (staticMessageController != null)
        {
            Destroy(this);
        }
        else
        {
            staticMessageController = this;
        }
    }

    public void ShowMessage(System.Object caller, Vector3 position, string message)
    {
        LastCaller = caller;
        StaticMessage.transform.position = position;
        StaticMessage.Open(message);
    }

    public void HideMessage(System.Object caller)
    {
        if(caller == LastCaller)
            StaticMessage.Close();
    }

    public Color GetInfoTextColorByType(TraitBase.TraitType type)
    {
        switch (type)
        {
            case TraitBase.TraitType.positive:
                {
                    return positiveColor;
                }
            case TraitBase.TraitType.negative:
                {
                    return negativeColor;
                }
            case TraitBase.TraitType.other:
                {
                    return regularColor;
                }
        }

        return regularColor;
    }
}
