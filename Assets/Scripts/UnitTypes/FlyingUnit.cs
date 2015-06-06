using UnityEngine;
using System.Collections;

public class FlyingUnit : MonoBehaviour 
{

    [SerializeField]
    public GameObject targetLoc;
    public float height = 30f;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator anim;
    private UnitStats stats;
    private UnitSight vision;

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
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        Move();
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc.transform.position);
    }
}
