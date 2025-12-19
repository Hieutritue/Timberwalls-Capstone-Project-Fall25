using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneSkip : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private float skipDelay = 2f;
    private float timer;
    private bool isSkipping;
    private bool hasSkipped;
    [SerializeField] private MMF_Player endSceneFeedback;

    void Start()
    {
        director.stopped += OnCutsceneEnd;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < skipDelay || hasSkipped)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            SkipCutscene();
    }
    
    private void SkipCutscene()
    {
        if (isSkipping) return;

        isSkipping = true;

        // Jump timeline to the end
        director.time = director.duration;
        director.Evaluate();
        director.Stop();

        EndCutscene();
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        EndCutscene();
    }

    private void EndCutscene()
    {
        endSceneFeedback.PlayFeedbacks();
        Debug.Log("Cutscene Ended");
    }

    private void OnDestroy()
    {
        director.stopped -= OnCutsceneEnd;
    }
}
