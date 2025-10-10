using System;
using DefaultNamespace;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    public Resource Wood { get; set; }
    public Resource Stone { get; set; }
    public Resource Iron { get; set; }
    public Resource Copper { get; set; }
    public Resource RefinedIron { get; set; }
    public Resource RefinedCopper { get; set; }
    public Resource Oil { get; set; }
    public Resource Steel { get; set; }
    public Resource Plastic { get; set; }
    public Resource Circuits { get; set; }
    public Resource Niobium { get; set; }
    public Resource BatteryCell { get; set; }
    public Resource Bonium { get; set; }
    public Resource SuperCoolant { get; set; }

    private void Start()
    {
        Wood = new Resource();
        Stone = new Resource();
        Iron = new Resource();
        Copper = new Resource();
        RefinedIron = new Resource();
        RefinedCopper = new Resource();
        Oil = new Resource();
        Steel = new Resource();
        Plastic = new Resource();
        Circuits = new Resource();
        Niobium = new Resource();
        BatteryCell = new Resource();
        Bonium = new Resource();
        SuperCoolant = new Resource();
    }
}

public class Resource
{
    public int Amount
    {
        get => Amount;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            Amount = value; 
            OnResourceAmountChanged?.Invoke(Amount);
        }
    }

    public Action<int> OnResourceAmountChanged;

    public Resource(Action<int> onResourceAmountChanged = null, int amount = 0)
    {
        OnResourceAmountChanged = onResourceAmountChanged;
        Amount = amount;
    }
}