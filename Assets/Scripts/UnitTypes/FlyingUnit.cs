using UnityEngine;
using System.Collections;

public class FlyingUnit : MonoBehaviour 
{
    [SerializeField]
    public float height = 30f;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator anim;
    private UnitStats stats;
    private UnitSight vision;
    private Vector3 targetLoc;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        stats = GetComponent<UnitStats>();
        vision = GetComponent<UnitSight>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ensures that the flyer remains at a specified height
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        // Update the target location
        targetLoc = vision.actionTarget.transform.position;
        Move();
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc);
    }
}
