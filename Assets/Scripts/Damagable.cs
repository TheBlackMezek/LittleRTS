using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour {

    [Header("Damagable Settings")]

    public int TeamID;
    [SerializeField] private float maxHP;
    [SerializeField] private float healPerSecond;

    [Header("Damagable Links")]

    [SerializeField] private Transform healthBar;
    [SerializeField] private UnityEngine.UI.Image teamColorImage;



    public float HP { get; private set; }

    protected Team team;



    protected virtual void Start()
    {
        team = TeamManager.Instance.GetTeam(TeamID);

        HP = maxHP;
        teamColorImage.color = team.TeamColor;
    }

    public virtual void CustomUpdate(float dt)
    {
        HP += dt * healPerSecond;
        if (HP > maxHP)
            HP = maxHP;

        Vector3 scale = healthBar.localScale;
        scale.x = HP / maxHP;
        healthBar.localScale = scale;
    }

    public virtual void Damage(float amt)
    {
        HP -= amt;
        if (HP <= 0f)
            Kill();
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

}
