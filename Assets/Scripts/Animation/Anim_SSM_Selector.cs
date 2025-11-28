using UnityEngine;

public class Anim_SSM_Selector : StateMachineBehaviour
{
    [SerializeField] private int count = 1;
    [SerializeField] private string parameterName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int selection = Random.Range(0, count);
        animator.SetInteger(parameterName, selection);
    }
}
