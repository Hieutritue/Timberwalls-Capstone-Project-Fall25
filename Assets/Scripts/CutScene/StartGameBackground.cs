using System;
using UnityEngine;

public class StartGameBackground : MonoBehaviour
{
    [SerializeField] private Material _starBig;
    [SerializeField] private Material _starMedium;
    [SerializeField] private Material _starBackground;
    [SerializeField] private float scrollSpeedBig = 0.01f;
    [SerializeField] private float scrollSpeedMedium = 0.02f;
    [SerializeField] private float scrollSpeedBackground = 0.01f;
    private int _MainTexId;

    private void Start()
    {
        _MainTexId = Shader.PropertyToID("_MainTex");
    }

    void Update()
    {
        Vector2 offset = _starBig.GetTextureOffset(_MainTexId);
        offset += new Vector2(scrollSpeedBig * Time.deltaTime, 0);
        _starBig.SetTextureOffset(_MainTexId, offset);
        
        offset = _starMedium.GetTextureOffset(_MainTexId);
        offset += new Vector2(scrollSpeedMedium * Time.deltaTime, 0);
        _starMedium.SetTextureOffset(_MainTexId, offset);
        
        offset = _starMedium.GetTextureOffset(_MainTexId);
        offset += new Vector2(scrollSpeedBackground * Time.deltaTime, 0);
        _starBackground.SetTextureOffset(_MainTexId, offset);
    }
}
