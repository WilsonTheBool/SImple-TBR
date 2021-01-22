using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnStateEnd : StateMachineBehaviour
{
    public GameObject prefab;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(prefab, animator.transform.position, new Quaternion());
    }
}
