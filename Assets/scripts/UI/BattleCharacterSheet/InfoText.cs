using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class InfoText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    protected IConvertToInfoText convertedObject;

    protected StaticMessageController staticMessageController;

    Text text;

    public bool isValid()
    {
        if(convertedObject != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void Start()
    {
        text = GetComponent<Text>();
        staticMessageController = StaticMessageController.staticMessageController;
    }

    public virtual void Clear()
    {
        text.text = "";
        convertedObject = null;
    }

    public virtual void SetValue(IConvertToInfoText obj)
    {
        convertedObject = obj;
        text.text = obj.GetInfoValueName();
        text.color = staticMessageController.GetInfoTextColorByType(obj.GetInfoType());
    }

    [SerializeField]
    Vector3 offSet;

    public void ShowInfo()
    {
        
        staticMessageController.ShowMessage(this, this.transform.position + offSet, convertedObject.GetInfoValueDiscription());

    }

    public void HideInfo()
    {
        staticMessageController.HideMessage(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isValid())
            ShowInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isValid())
            HideInfo();
    }
}
