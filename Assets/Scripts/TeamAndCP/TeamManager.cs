using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    [SerializeField] private Team[] teams;

    public static TeamManager Instance { get; private set; }



    private void Awake()
    {
        Instance = this;
    }

    public Team GetTeam(int id)
    {
        return teams[id];
    }

}
