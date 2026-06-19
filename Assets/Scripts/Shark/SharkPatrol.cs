using UnityEngine;
using UnityEngine.AI;

public class SharkPatrol : MonoBehaviour
{
    public float patrolDistance = 5f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Vector3 startPos;
    private bool moveRight = true;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

        startPos = transform.position;

        SetPatrolDestination();
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            agent.isStopped = false;

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    5f * Time.deltaTime
                );
            }

            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackRange)
            {
                agent.isStopped = true;

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                }
            }
        }
        else
        {
            agent.isStopped = false;

            if (!agent.pathPending && agent.remainingDistance <= 0.3f)
            {
                moveRight = !moveRight;
                SetPatrolDestination();
            }
        }
    }

    void SetPatrolDestination()
    {
        Vector3 targetPos = moveRight
            ? startPos + Vector3.right * patrolDistance
            : startPos + Vector3.left * patrolDistance;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(targetPos, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnEnable()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        startPos = transform.position;
        moveRight = true;

        SetPatrolDestination();
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        PlayerHealth hp = player.GetComponent<PlayerHealth>();

        if (hp != null)
        {
            hp.TakeDamage();
        }
    }
}