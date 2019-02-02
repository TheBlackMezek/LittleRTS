using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Damagable {

    [Header("Basic Settings")]
    
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attacksPerMinute;
    [SerializeField] private float moveSpeed;



    protected override void Start()
    {
        base.Start();

        team.RegisterUnit(this);
    }

    public override void Kill()
    {
        team.UnregisterUnit(this);

        base.Kill();
    }

}
