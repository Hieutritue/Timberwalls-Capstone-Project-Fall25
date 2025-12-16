using DefaultNamespace;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbackManager : MonoSingleton<FeedbackManager>
{
    public MMF_Player ColonistExiledFeedback;
    public MMF_Player BuildingConstructedFeedback;
    public MMF_Player ResourceGatheredFeedback;

    public void PlayParticleFeebackAt(Transform transform)
    {
        
    }
}
