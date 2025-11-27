using System;
using UnityEngine;
using System.Collections;
using Pathfinding;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WanderingDestinationSetter : MonoBehaviour {
    public float Radius = 20;
    public float Delay;
    [SerializeField] private Animator _animator;
    IAstarAI ai;
    private bool isWalking = false;
    
    void Start () {
        ai = GetComponent<IAstarAI>();
        _timer = Delay;
    }

    Vector3 PickRandomPoint () {
        var point = Random.insideUnitSphere * Radius;

        point.y = 0;
        point += ai.position;
        return point;
    }

    private float _timer;
    public void Tick () {
        
        // Update the destination of the AI if
        // the AI is not already calculating a path and
        // the ai has reached the end of the path or it has no path at all
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath)) {
            _timer += Time.deltaTime;
            isWalking = false;
            _animator.SetBool(ColonistAnimationString.IS_WALKING,isWalking);
            if (_timer < Delay) return;
            
            isWalking = true;
            _animator.SetBool(ColonistAnimationString.IS_WALKING,isWalking);
            ai.destination = PickRandomPoint();
            ai.SearchPath();
            // set timer to be random in [0, Delay/2] to avoid all colonists moving at the same time
            _timer = Random.Range(0, Delay / 2f);
        }
    }
}