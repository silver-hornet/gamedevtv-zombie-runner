using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    // How far is the enemy from the target? Initialized at a gigantic number, otherwise it would be initialized at 0 by default,
    // which would make the enemy think that it's right on top of the player.

    bool isProvoked = false;
    EnemyHealth health;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (health.IsDead())
        {
            enabled = false;
            navMeshAgent.enabled = false;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
            ChaseTarget();
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    void EngageTarget()
    {
        FaceTarget();

        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
       else if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized; // .normalized returns a magnitude of 1, since we're interested in the direction and not the magnitude
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed); // slerp is spherical interpolation, which allows for smooth rotation between two vectors
    }

    void OnDrawGizmosSelected()
    {
        // Display the change range radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
