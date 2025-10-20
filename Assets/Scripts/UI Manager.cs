using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject priorityMatrix;
    [SerializeField] private GameObject buildingMenu;
    [SerializeField] private GameObject researchPage;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject tutorialMenu;
    private readonly float normalSpeedValue = 1;
    private readonly float spedUpSpeedValue = 1.5f;
    private readonly float furtherSpedUpSpeedValue = 2;
    public void OnBuildingPressed()
    {
        CheckAndOpenUIContainer(buildingMenu);
    }

    public void OnResearchPressed()
    {
        CheckAndOpenUIContainer(researchPage);
    }
    public void OnNormalSpeedPressed()
    {
        Time.timeScale = normalSpeedValue;
    }
    public void OnSpeedUpPressed()
    {
        Time.timeScale = spedUpSpeedValue;
    }
    
    public void OnFurtherSpeedUpPressed()
    {
        Time.timeScale = furtherSpedUpSpeedValue;
    }
    
    public void OnCancelPressed()
    {
        
    }
    
    public void OnDestroyPressed()
    {
        
    }
    
    public void OnPriorityPressed()
    {
        CheckAndOpenUIContainer(priorityMatrix);
    }
    
    public void OnPriorityExitPressed()
    {
        if (priorityMatrix == null)
        {
            Debug.LogError("No PriorityMatrix Game Object found");
        }
        else
        {
            priorityMatrix.SetActive(false);
        }
    }
    private void CheckAndOpenUIContainer(GameObject UIContainer)
    {
        if (UIContainer == null)
        {
            Debug.LogError($"No {UIContainer.name} Game Object found");
        }
        else if (UIContainer != null &&  !UIContainer.activeInHierarchy)
        {
            UIContainer.SetActive(true);
        }
        else
        {   
            UIContainer.SetActive(false);
        }
    }
    
}
