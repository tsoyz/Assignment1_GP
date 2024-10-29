using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // Import SceneManagement namespace

public class EnemyChasePlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float chaseSpeed = 5f; // Speed at which the enemy will chase the player
    public float chaseRange = 10f; // The distance within which the enemy will start chasing the player
    public Transform baseTransform; // The target base for enemies to attack
    public float playerAttackDistance = 3f;
    public int damageAmount = 10; // Damage dealt to the player when touched

    private NavMeshAgent agent;
    private Animator animator; // Reference to Animator component
    private EnemyState currentState;

    public enum EnemyState { Idle, Wander, ChasePlayer, AttackBase }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Initialize Animator

        if (animator == null)
        {
            Debug.LogWarning("Animator component is missing on " + gameObject.name);
        }
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        currentState = EnemyState.Wander;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                LookForTargets();
                break;
            case EnemyState.Wander:
                Wander();
                LookForTargets();
                break;
            case EnemyState.ChasePlayer:
                ChasePlayer();
                break;
            case EnemyState.AttackBase:
                AttackBase();
                LookForTargets(); // Keep looking for the player while attacking the base
                break;
        }

        UpdateAnimation(); // Update animation based on agent speed
    }

    void LookForTargets()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= chaseRange)
            {
                Debug.Log("PLAYER detected!");
                currentState = EnemyState.ChasePlayer;
            }
        }
    }

    void Wander()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void ChasePlayer()
    {
        if (playerTransform == null) return;

        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > chaseRange * 1.5f)
        {
            currentState = EnemyState.Wander;
        }
    }

    void AttackBase()
    {
        if (baseTransform == null)
        {
            Debug.LogWarning("Base transform is not assigned.");
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(baseTransform.position);
    }

    void UpdateAnimation()
    {
        if (animator == null) return; // Prevent NullReferenceException if animator is not assigned

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        // Prevent running animation when agent is not moving
        if ((agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) || speed < 0.1f)
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}