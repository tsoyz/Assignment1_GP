//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyFSM : MonoBehaviour
//{
//    public enum EnemyState { Idle, Wander, ChasePlayer, AttackBase }
//    public EnemyState currentState;

//    public Sight sightSensor; // Reference to Sight sensor component
//    private Transform playerTransform;
//    public Transform baseTransform; // The target base for enemies to attack
//    public float playerAttackDistance = 3f;

//    private NavMeshAgent agent;
//    private Vector3 wanderTarget;
//    private float wanderRadius = 10f;
//    private float wanderTimer = 5f;
//    private float timer;

//    private Animator animator; // Reference to Animator component

//    private void Awake()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>(); // Initialize Animator
//    }

//    public void SetPlayer(Transform player)
//    {
//        playerTransform = player;
//    }

//    private void Start()
//    {
//        timer = wanderTimer;
//        if (sightSensor == null)
//        {
//            Debug.LogError("Sight sensor is not assigned in EnemyFSM on " + gameObject.name);
//        }

//        // If the enemy is a skeleton, set the state to attack the base initially
//        if (gameObject.CompareTag("Skeleton") && baseTransform != null)
//        {
//            currentState = EnemyState.AttackBase;
//        }
//    }

//    private void Update()
//    {
//        switch (currentState)
//        {
//            case EnemyState.Idle:
//                LookForTargets();
//                Wander();
//                break;
//            case EnemyState.Wander:
//                Wander();
//                LookForTargets();
//                break;
//            case EnemyState.ChasePlayer:
//                ChasePlayer();
//                break;
//            case EnemyState.AttackBase:
//                AttackBase();
//                LookForTargets(); // Keep looking for the player while attacking the base
//                break;
//        }

//        UpdateAnimation(); // Update animation based on agent speed
//    }

//    void LookForTargets()
//    {
//        if (sightSensor != null && sightSensor.detectedObject != null)
//        {
//            if (sightSensor.detectedObject.CompareTag("Player"))
//            {
//                Debug.Log("PLAYER detected!");
//                playerTransform = sightSensor.detectedObject.transform; // Update playerTransform to detected player
//                currentState = EnemyState.ChasePlayer;
//            }
//        }
//    }

//    void Wander()
//    {
//        timer += Time.deltaTime;

//        if (timer >= wanderTimer)
//        {
//            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
//            randomDirection += transform.position;
//            NavMeshHit hit;
//            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
//            {
//                wanderTarget = hit.position;
//                agent.SetDestination(wanderTarget);
//            }
//            timer = 0;
//        }

//        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
//        {
//            currentState = EnemyState.Idle;
//        }
//    }

//    void ChasePlayer()
//    {
//        if (playerTransform == null) return;

//        agent.isStopped = false;
//        agent.SetDestination(playerTransform.position);

//        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
//        if (distanceToPlayer > playerAttackDistance * 1.5f || sightSensor.detectedObject == null)
//        {
//            currentState = EnemyState.AttackBase; // Return to attacking base if player is out of range
//        }
//    }

//    void AttackBase()
//    {
//        if (baseTransform == null)
//        {
//            Debug.LogWarning("Base transform is not assigned.");
//            return;
//        }

//        agent.isStopped = false;
//        agent.SetDestination(baseTransform.position);

//        // Transition to chase player if the player is detected while attacking the base
//        LookForTargets();
//    }

//    void UpdateAnimation()
//    {
//        float speed = agent.velocity.magnitude;
//        animator.SetFloat("Speed", speed);

//        // Prevent running animation when agent is not moving
//        if ((agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) || speed < 0.1f)
//        {
//            animator.SetFloat("Speed", 0);
//        }
//    }
//}
