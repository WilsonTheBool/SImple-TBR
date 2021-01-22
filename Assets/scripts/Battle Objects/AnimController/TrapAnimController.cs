using UnityEngine;
using System.Collections;

public class TrapAnimController : MonoBehaviour
{
    private Animator anim;

    private PushTrapObject owner;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        owner = GetComponent<PushTrapObject>();

        owner.OnActivate += Owner_OnActivate;
    }

    private void Owner_OnActivate(object sender, System.EventArgs e)
    {
        anim.SetTrigger("Activate");
    }
}
