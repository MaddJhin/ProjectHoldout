using UnityEngine;
using System.Collections;

public class BruteUnit : MonoBehaviour 
{
    private NavMeshAgent agent;
    private SphereCollider actionRadius;
    private Animator anim;
    private UnitStats stats;
    private BruteStun action;
    private UnitSight vision;
    private float elapsedTime;
    private Vector3 targetLoc;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        actionRadius = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        stats = GetComponent<UnitStats>();
        action = GetComponent<BruteStun>();
        vision = GetComponent<UnitSight>();
        elapsedTime = 0f;
    }

    void Update()
    {
        // Update target location
        targetLoc = vision.actionTarget.transform.position;

        // Attack speed timer
        if (stats.attackSpeed < elapsedTime && vision.targetDistance < agent.stoppingDistance)
        {
            elapsedTime = 0f;
            Attack();
        }


        else
            Move();

        elapsedTime += Time.deltaTime;
    }

    void Attack()
    {
        agent.Stop();
        Debug.Log(vision.actionTarget);
        action.Slam(vision.actionTarget);
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc);
    }
}
