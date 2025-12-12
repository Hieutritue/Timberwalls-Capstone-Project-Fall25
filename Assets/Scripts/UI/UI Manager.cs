using System;
using DefaultNamespace;
using DefaultNamespace.ColonistSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIManager : MonoSingleton<UIManager>
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
    [SerializeField] private TMP_Text _populationText;

    [SerializeField] private UnityEvent _onLeftClickNotOnUI;

    //Sound implementation
    [SerializeField] private SoundSource sfxSource;
    
    private void Start()
    {
        InputManager.Instance.OnMouseLeftClick += LeftClickNotOnUI;
    }
    
    private void LeftClickNotOnUI()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        
        _onLeftClickNotOnUI?.Invoke();
    }

    public void UpdatePopulationText(int currentPopulation, int maxPopulation)
    {
        if (_populationText != null)
        {
            _populationText.text = $"{currentPopulation} / {maxPopulation}";
        }
    }
    
    public void OnBuildingPressed()
    {
        CheckAndOpenUIContainer(buildingMenu);
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }

    public void OnViewNPCDetail(Colonist colonist)
    {
        OpenColonistDetail(colonist);
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
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
            sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
        }
    }

    public void OnSchedulePressed()
    {
        CheckAndOpenUIContainer(scheduleMenu);
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    public void OnResearchPressed()
    {
        CheckAndOpenUIContainer(researchPage);
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    public void OnNormalSpeedPressed()
    {
        Time.timeScale = normalSpeedValue;
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    public void OnSpeedUpPressed()
    {
        Time.timeScale = spedUpSpeedValue;
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    
    public void OnFurtherSpeedUpPressed()
    {
        Time.timeScale = furtherSpedUpSpeedValue;
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    
    public void OnCancelPressed()
    {
        BuildingSystemManager.Instance.PlacementSystem.EnterCancelMode();
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    
    public void OnDemolishRoomPressed()
    {
        BuildingSystemManager.Instance.PlacementSystem.EnterDeleteMode(PlaceableType.Room);
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    
    public void OnDemolishFurniturePressed()
    {
        BuildingSystemManager.Instance.PlacementSystem.EnterDeleteMode(PlaceableType.Furniture);
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }
    
    public void OnPriorityPressed()
    {
        CheckAndOpenUIContainer(priorityMatrix);
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
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
            sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
        }
    }
    
    public void OnIdleModePressed()
    {
        BuildingSystemManager.Instance.PlacementSystem.TransitionToIdleState();
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
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
            sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
        }
        else
        {   
            UIContainer.SetActive(false);
            sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
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
        sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_3, fadeIn:false, fadeOut:false, crossfade:false);
    }

    private void loadColonistInfo(Colonist colonist)
    {
        
    }
    
}
