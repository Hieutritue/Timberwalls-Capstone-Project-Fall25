using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildMenuManager : MonoBehaviour
{
    [SerializeField]private List<TabSO> tabs; //Assign in inspector
    [SerializeField]private GameObject tabPrefab;
    [SerializeField]private GameObject menuContent;
    [SerializeField] private GameObject itemWindow;
    private TabSO _currentTab;

    private void Start()
    {
        GenerateTabs();
    }

    private void GenerateTabs()
    {
        foreach (TabSO tab in tabs)
        {
            GameObject tabObj = Instantiate(tabPrefab, menuContent.transform);
            TabButton tabButton = tabObj.GetComponent<TabButton>();
            tabButton.Initialize(tab, this);
        }
    }

    public void OnClickTab(TabSO tabClicked)
    {
        _currentTab = tabClicked;
        ItemWindowManager.Instance.GenerateItems(tabClicked);
    }
    
}
