using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpBar : MonoBehaviour
{
    public Image redBar;
    public Image whiteBar;


    public bool isWhiteActive = false;

    [SerializeField][Range(0,2)]
    private float whitechangeSpeed = 0.1f;
    [SerializeField]
    [Range(0, 2)]
    private float waitTime = 0.2f;

    [SerializeField]
    private float targetHp = 0;

    [SerializeField]
    private float targetFill;

   public void SetHpBar(float hp, float maxHp)
    {
        if(targetHp < hp)
        {
            SetOnHPAdd(hp, maxHp);
        }
        else
        {
            SetOnHPRemoved(hp, maxHp);
        }
        
    }

    private void SetOnHPAdd(float hp, float maxHp)
    {
       
        targetHp = hp;
        targetFill = hp / maxHp;
        whiteBar.fillAmount = targetFill;

        StartCoroutine(ChangeWhiteBar_OnAdd());
    }

    
    private void SetOnHPRemoved(float hp, float maxHp)
    {
        if (hp < 0)
        {
            hp = 0;
        }

        targetHp = hp;
        targetFill = hp / maxHp;
        if (maxHp > 0)
        {
            redBar.fillAmount = targetFill;
        }
        else
        {
            redBar.fillAmount = 0;
        }

        
        StartCoroutine(ChangeWhiteBar_OnRemove());
    }

    private IEnumerator ChangeWhiteBar_OnRemove()
    {


        while (isWhiteActive)
        {
            yield return new WaitForFixedUpdate();
        }

        

        isWhiteActive = true;

        yield return new WaitForSeconds(waitTime);

        while (whiteBar.fillAmount > targetFill)
        {
            whiteBar.fillAmount -= whitechangeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        whiteBar.fillAmount = targetFill;
       isWhiteActive = false;
    }

    private IEnumerator ChangeWhiteBar_OnAdd()
    {
        while (isWhiteActive)
        {
            yield return new WaitForFixedUpdate();
        }

        isWhiteActive = true;

        yield return new WaitForSeconds(waitTime);

        while (redBar.fillAmount < targetFill)
        {
            redBar.fillAmount += whitechangeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        redBar.fillAmount = targetFill;
       isWhiteActive = false;
    }
}
