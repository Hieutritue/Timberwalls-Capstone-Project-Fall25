using System.Collections;
using BuildingSystem;
using UnityEngine;

public class RoamingPersonalActionFurniture : PersonalActionFurniture
{
    [SerializeField] private Animals _roamingGameObject;

    public override void Constructed()
    {
        OnConstructed?.Invoke();
        StartCoroutine(EnableRoamingNextFrame());
    }

    private IEnumerator EnableRoamingNextFrame()
    {
        yield return null; // WAIT 1 FRAME
        _roamingGameObject.enabled = true;
    }

    public override void Demolished()
    {
        OnDemolished?.Invoke();
        _roamingGameObject.enabled = false;
    }
}
