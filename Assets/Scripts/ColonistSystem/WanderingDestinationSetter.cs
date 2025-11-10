using UnityEngine;
using System.Collections;
using Pathfinding;
using UnityEngine.Serialization;

public class WanderingDestinationSetter : MonoBehaviour {
    public float Radius = 20;
    public float Delay;
    IAstarAI ai;

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
            if (_timer < Delay) return;
            ai.destination = PickRandomPoint();
            ai.SearchPath();
            // set timer to be random in [0, Delay/2] to avoid all colonists moving at the same time
            _timer = Random.Range(0, Delay / 2f);
        }
    }
}