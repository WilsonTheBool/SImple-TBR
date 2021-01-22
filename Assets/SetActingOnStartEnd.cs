using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActingOnStartEnd : StateMachineBehaviour
{
    private BattleController battle;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        battle = FindObjectOfType<BattleController>();

        battle.AddActor();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        battle.RemoveActor();
    }
}
