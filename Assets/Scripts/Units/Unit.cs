using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Damagable {

    [Header("Unit Settings")]
    
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attacksPerMinute;
    [SerializeField] private float moveSpeed;

    [Header("Unit Links")]

    [SerializeField] private UnityEngine.UI.Image selectedImage;



    private bool selected;



    protected override void Start()
    {
        base.Start();

        team.RegisterUnit(this);

        selectedImage.color = team.SelectionColor;
        selectedImage.gameObject.SetActive(false);
    }

    public override void Kill()
    {
        team.UnregisterUnit(this);

        base.Kill();
    }

    public void Select()
    {
        selected = true;
        selectedImage.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        selected = false;
        selectedImage.gameObject.SetActive(false);
    }

}
