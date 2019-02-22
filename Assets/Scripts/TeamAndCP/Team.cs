using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {

    [Header("Settings")]

    [SerializeField] private Color color;
    [SerializeField] private Color selectionColor;
    [SerializeField] private Color progressBarColor;
    [SerializeField] private Color weaponColor;
    [SerializeField] private int startingMass;
    [SerializeField] private int id;
    [SerializeField] private float baseEnergyPerSecond;
    [SerializeField] private float startingEnergy;
    [SerializeField] private float maxEnergy;

    public VoidReturnFloat OnEnergyChanged;

    public Color TeamColor { get { return color; } }
    public Color SelectionColor { get { return selectionColor; } }
    public Color ProgressBarColor { get { return progressBarColor; } }
    public Color WeaponColor { get { return weaponColor; } }
    public int Mass { get; private set; }
    public int ID { get { return id; } }

    /// <summary>
    /// Do NOT set this directly, always set through Energy
    /// </summary>
    private float energy;
    public float Energy
    {
        get { return energy; }
        private set
        {
            energy = Mathf.Clamp(value, 0f, maxEnergy);
            if(OnEnergyChanged != null)
                OnEnergyChanged(energy);
        }
    }

    private List<Unit> units = new List<Unit>();
    private List<Unit> selectedUnits = new List<Unit>();
    private List<Building> buildings = new List<Building>();

    private float energyTimer = 1f;
    private float energyPerSecond;



    private void Start()
    {
        Mass = startingMass;
        Energy = startingEnergy;
        CalculateEnergyPerSecond();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        energyTimer -= dt;
        if(energyTimer <= 0f)
        {
            energyTimer += 1f;
            Energy += energyPerSecond;
        }

        for(int i = 0; i < units.Count; ++i)
        {
            units[i].CustomUpdate(dt);
        }
        for(int i = 0; i < buildings.Count; ++i)
        {
            buildings[i].CustomUpdate(dt);
        }
    }

    public void RegisterUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void RegisterBuilding(Building b)
    {
        buildings.Add(b);
    }

    public void UnregisterBuilding(Building b)
    {
        buildings.Remove(b);
    }

    public void SetUnitsDestination(Vector3 pos)
    {
        for (int i = 0; i < selectedUnits.Count; ++i)
            selectedUnits[i].SetDestination(pos);
    }

    public void SelectUnit(Unit unit)
    {
        unit.Select();
        selectedUnits.Add(unit);
    }

    public void SelectUnitsInBounds(Bounds bounds, Camera cam)
    {
        for(int i = 0; i < units.Count; ++i)
        {
            if (bounds.Contains(cam.WorldToViewportPoint(units[i].transform.position)))
            {
                if (!selectedUnits.Contains(units[i]))
                    selectedUnits.Add(units[i]);
                units[i].Select();
            }
        }
    }

    public void DeselectAllUnits()
    {
        for (int i = 0; i < selectedUnits.Count; ++i)
            selectedUnits[i].Deselect();

        selectedUnits.Clear();
    }



    private void CalculateEnergyPerSecond()
    {
        energyPerSecond = baseEnergyPerSecond;
    }

}
