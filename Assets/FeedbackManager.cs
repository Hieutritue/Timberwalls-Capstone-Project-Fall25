using DefaultNamespace;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbackManager : MonoSingleton<FeedbackManager>
{
    public MMF_Player ColonistExiledFeedback;
    public MMF_Player BuildingConstructedFeedback;
    public MMF_Player ResourceGatheredFeedback;
    public MMF_Player ButtonClickFeedback;
    public MMF_Player ButtonClickSmallFeedback;
    public MMF_Player BuildingPlacedFeedback;
    public MMF_Player CancelFeedback;
    public void PlayParticleFeebackAt(Transform transform)
    {
        
    }
}
