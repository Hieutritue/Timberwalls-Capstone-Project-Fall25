using DefaultNamespace.ColonistSystem;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject priorityMatrix;
    [SerializeField] private GameObject buildingMenu;
    [SerializeField] private GameObject researchPage;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject scheduleMenu;
    [SerializeField] private GameObject colonistDetailPanel;
    [SerializeField] private readonly float normalSpeedValue = 1;
    [SerializeField] private readonly float spedUpSpeedValue = 1.5f;
    [SerializeField] private readonly float furtherSpedUpSpeedValue = 2;
    
    
    public void OnBuildingPressed()
    {
        CheckAndOpenUIContainer(buildingMenu);
        
    }

    public void OnViewNPCDetail(Colonist colonist)
    {
        OpenColonistDetail(colonist);
    }

    public void OnExitSchedule()
    {
        if (scheduleMenu == null)
        {
            Debug.LogError("No PriorityMatrix Game Object found");
        }
        else
        {
            scheduleMenu.SetActive(false);
        }
    }

    public void OnSchedulePressed()
    {
        CheckAndOpenUIContainer(scheduleMenu);
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

    private void OpenColonistDetail(Colonist colonist)
    {
        if (colonistDetailPanel == null)
        {
            Debug.LogError($"colonistDetailPanel not found");
            return;
        }

        if (colonist == null)
        {
            Debug.LogError($"Colonist is null");
        }
        colonistDetailPanel.SetActive(true);
        loadColonistInfo(colonist);
    }

    private void loadColonistInfo(Colonist colonist)
    {
        
    }
    
}
