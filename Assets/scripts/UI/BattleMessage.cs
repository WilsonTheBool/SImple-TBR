using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleMessage: MonoBehaviour
{
    public Text text;
    public CanvasRenderer render;

    private void Start()
    {
        StartCoroutine(GoingUp());
    }

    public Vector3 textTargetPos;
    public float moveSpeed;
    public float alphaSpeeed;
    public float standTime;


    private float timeLeft;
    IEnumerator GoingUp()
    {
        Vector3 target = transform.position + textTargetPos;
        float alpha = render.GetAlpha();
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if(timeLeft < standTime)
            {
                timeLeft += Time.deltaTime;
            }
            else
            {
                render.SetAlpha(alpha -= alphaSpeeed * Time.deltaTime);
            }
            
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
}

