using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Damagable {

    [Header("Unit Settings")]
    
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attacksPerMinute;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float aggroCheckInterval;
    [SerializeField] private float laserDisplayTime;

    [Header("Unit Links")]

    [SerializeField] private UnityEngine.UI.Image selectedImage;
    [SerializeField] private UnityEngine.AI.NavMeshAgent navAgent;
    [SerializeField] private LineRenderer laserRenderer;

    public VoidReturnUnit OnKilled;

    private bool selected;
    private Damagable target = null;
    private float attackInterval;



    protected override void Start()
    {
        base.Start();

        team.RegisterUnit(this);

        attackInterval = 60f / attacksPerMinute;

        selectedImage.color = team.SelectionColor;
        selectedImage.gameObject.SetActive(false);

        laserRenderer.endColor = team.WeaponColor;
        laserRenderer.startColor = team.WeaponColor;
        laserRenderer.enabled = false;

        navAgent.speed = moveSpeed;

        StartCoroutine("CheckForTarget");
    }

    protected IEnumerator CheckForTarget()
    {
        yield return new WaitForSeconds(aggroCheckInterval);
        
        Damagable trg = null;

        if(target == null)
        {
            Vector3 pos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(pos, range);

            float closestDist = float.MaxValue;
            for(int i = 0; i < colliders.Length; ++i)
            {
                Damagable d = colliders[i].GetComponent<Damagable>();
                if(d != null && d.TeamID != TeamID)
                {
                    float dist = Vector3.Distance(pos, colliders[i].transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        trg = d;
                    }
                }
            }
        }

        if (trg != null)
            Attack(trg);

        StartCoroutine("CheckForTarget");
    }

    public void Attack(Damagable trg)
    {
        target = trg;
        StartCoroutine("AttackRoutine");
    }

    protected IEnumerator AttackRoutine()
    {
        if(target == null || Vector3.Distance(transform.position, target.transform.position) > range)
        {
            target = null;
            yield return null;
        }
        else
        {
            laserRenderer.SetPosition(0, transform.position);
            laserRenderer.SetPosition(1, target.transform.position);
            laserRenderer.enabled = true;
            StartCoroutine("HideLaser");

            target.Damage(damage);

            yield return new WaitForSeconds(attackInterval);

            StartCoroutine("AttackRoutine");
        }
    }

    protected IEnumerator HideLaser()
    {
        yield return new WaitForSeconds(laserDisplayTime);

        laserRenderer.enabled = false;
    }

    public override void Kill()
    {
        if (OnKilled != null)
            OnKilled(this);

        team.UnregisterUnit(this);

        base.Kill();
    }

    /// <summary>
    /// This should ONLY be called from the Team class
    /// </summary>
    public void Select()
    {
        selected = true;
        selectedImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// This should ONLY be called from the Team class
    /// </summary>
    public void Deselect()
    {
        selected = false;
        selectedImage.gameObject.SetActive(false);
    }

    public void SetDestination(Vector3 pos)
    {
        navAgent.SetDestination(pos);
    }

}
