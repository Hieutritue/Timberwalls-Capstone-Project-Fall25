using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    private TabSO tabData;
    private BuildMenuManager menuManager;

    public void Initialize(TabSO data, BuildMenuManager manager)
    {
        tabData = data;
        menuManager = manager;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.tabName;
        
        // Add click listener
        GetComponentInChildren<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        menuManager.OnClickTab(tabData);
    }
}
