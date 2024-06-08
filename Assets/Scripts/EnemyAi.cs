using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    
    public NavMeshAgent agent;  // Enemy
    public Transform player;    // Player (You)
    public Animator animator;   // Reference the (skeleton) animator
    //public Time unityTime;

    // whatIsGround for a search walkpoint
    // whatIsPlayer needed to assign to player to attack
    public LayerMask whatIsGround, whatIsPlayer;
    public float MonsterHealth; // health of monster

    // Patroling Parameters
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // States Parameters
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Attack Parameters
    //bool alreadyAttacked;
    public float timeBetweenAttacks;
    private float attackTimer;

    private void Awake()
    {
        player = GameObject.Find("XR Origin (XR Rig)").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // when player not detected, walk around map
        if (!playerInSightRange && !playerInAttackRange) {
            animator.SetBool("isWalking", true);
            Patroling();
        }

        // when player is in sight, chase after player
        if (playerInSightRange && !playerInAttackRange) {
            animator.SetBool("isWalking", true);
            ChasePlayer();
        }

        // attack player when close enough
        if (playerInAttackRange && playerInSightRange) {
            animator.SetBool("isWalking", false);
            // Check if player is within attack range
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                // Check if enough time has passed since the last attack
                if (Time.time >= attackTimer)
                {
                    AttackPlayer();
                    attackTimer = Time.time + timeBetweenAttacks;
                }
            }
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    // Helper Function for Patrolling()
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    // Attack Player function
    private void AttackPlayer(){
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        // Make Enemy fixate on Player when attacking
        transform.LookAt(player);     

        // If the Enemy has not attacked yet, do the following
        /**
        if (!alreadyAttacked)
        {
            ///Attack code here
            animator.SetBool("willAttack", true);
            ///End of attack code


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }*/

        animator.SetTrigger("willAttack");
    }

    // Helper Function: every attack, reset to perform another attack
    /**
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }*/

    public void TakeDamage(int damage)
    {
        MonsterHealth -= damage;

        if (MonsterHealth <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);

        // activate a prompt to let player know they won
    }
}
