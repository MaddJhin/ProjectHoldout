using UnityEngine;
using System.Collections;

public class BruteUnit : MonoBehaviour {

    [SerializeField]
    public GameObject targetLoc;

    private NavMeshAgent agent;
    private SphereCollider actionRadius;
    private Animator anim;
    private UnitStats stats;
    private BruteStun action;
    private UnitSight vision;
    private float elapsedTime;

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
        // Attack speed timer
        if (vision.targetInRange && stats.attackSpeed < elapsedTime)
        {
            elapsedTime = 0f;
            Attack();
        }

        else if (vision.actionTarget == null)
            Move();

        elapsedTime += Time.deltaTime;
    }

    void Attack()
    {
        agent.Stop();
        action.Slam(vision.actionTarget);
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc.transform.position);
    }
}
