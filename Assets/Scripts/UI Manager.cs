using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject priorityMatrix;
    public void OnBuildingPressed()
    {
        Debug.Log("Building");
    }

    public void OnResearchPressed()
    {
        Debug.Log("Research");
    }
    public void OnNormalSpeedPressed()
    {
        
    }
    public void OnSpeedUpPressed()
    {
        
    }
    
    public void OnFurtherSpeedUpPressed()
    {
        
    }
    
    public void OnCancelPressed()
    {
        
    }
    
    public void OnDestroyPressed()
    {
        
    }
    
    public void OnPriorityPressed()
    {
        if (priorityMatrix == null)
        {
            Debug.LogError("No PriorityMatrix Game Object found");
        }
        else if (priorityMatrix != null &&  !priorityMatrix.activeInHierarchy)
        {
            priorityMatrix.SetActive(true);
        }
        else
        {   
            priorityMatrix.SetActive(false);
        }
    }
}
