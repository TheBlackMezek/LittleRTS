using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {

    [Header("Settings")]

    [SerializeField] private Color color;
    [SerializeField] private int startingMass;
    [SerializeField] private int id;



    public Color TeamColor { get { return color; } }
    public int Mass { get; private set; }
    public int ID { get { return id; } }



    private void Start()
    {
        Mass = startingMass;
    }

}
