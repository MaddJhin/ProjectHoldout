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
    private NavMeshObstacle obstacle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        actionRadius = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        stats = GetComponent<UnitStats>();
        action = GetComponent<BruteStun>();
        vision = GetComponent<UnitSight>();
        obstacle = GetComponent<NavMeshObstacle>();
        elapsedTime = 0f;
    }

    void Update()
    {
        // Update target location
        targetLoc = vision.actionTarget.transform.position;

        // If unit as at the target, stop moving and block other units
        if (vision.targetDistance <= agent.stoppingDistance)
        {
            agent.Stop();
            agent.enabled = false;
            obstacle.enabled = true;

            if (stats.attackSpeed < elapsedTime)
            {
                elapsedTime = 0f;
                Attack();
            }
        }

        else
        {
            obstacle.enabled = false;
            agent.enabled = true;
            Move();
        }

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
