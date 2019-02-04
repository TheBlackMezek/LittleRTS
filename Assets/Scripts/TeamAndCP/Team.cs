using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {

    [Header("Settings")]

    [SerializeField] private Color color;
    [SerializeField] private Color selectionColor;
    [SerializeField] private int startingMass;
    [SerializeField] private int id;



    public Color TeamColor { get { return color; } }
    public Color SelectionColor { get { return selectionColor; } }
    public int Mass { get; private set; }
    public int ID { get { return id; } }

    private List<Unit> units = new List<Unit>();
    private List<Unit> selectedUnits = new List<Unit>();



    private void Start()
    {
        Mass = startingMass;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        for(int i = 0; i < units.Count; ++i)
        {
            units[i].CustomUpdate(dt);
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

}
