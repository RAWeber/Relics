using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State { Idle, Chasing, Attacking };
    State currentState;

    public int level = 1;
    public float moveSpeed = 3.5f;
    public float atttackDamage = 10;
    public float attackSpeed = 3;
    public float attackDistance = .5f;
    public float attackDelay = 1;
    public ParticleSystem deathEffect;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;
    bool hasTarget;

    public static event Action OnDeathStatic;

    void Awake()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        pathfinder.speed = moveSpeed;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius * transform.localScale.x;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius * target.localScale.x;
        }
    }

    protected override void Start()
    {
        base.Start();

        if (hasTarget)
        {
            currentState = State.Chasing;
            targetEntity.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    public void SetStats(float moveSpeed, int damage, float health)
    {
        pathfinder.speed = moveSpeed;
        atttackDamage = damage;
        startingHealth = health;
    }

    public override void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= health && !dead)
        {
            if (OnDeathStatic != null)
            {
                OnDeathStatic();
            }
            deathEffect.startColor = GetComponent<Renderer>().material.color;
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
        }
        base.TakeDamage(damage, hitPoint, hitDirection);
    }

    IEnumerator Attack()
    {

        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 newHeightTarget = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 dirToTarget = (newHeightTarget - transform.position).normalized;
        Vector3 attackPosition = newHeightTarget - dirToTarget * (myCollisionRadius + targetCollisionRadius);

        float percent = 0;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(atttackDamage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistance/2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    // Update is called once per frame
    void Update () {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget <= Mathf.Pow(attackDistance + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + attackDelay;
                    StartCoroutine(Attack());
                }

            }
        }
    }
}
