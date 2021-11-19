using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent; //current enemy
    public Transform player; //target the player
    public LayerMask groundMask, playerMask;

    public float sightRange, attackRange;
    public bool playerTargeted, playerAttacked;

    public int enemyHealth;
    public float projectileSpeed = 6f;

    // AI patrols area
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // AI attacks player
    public float timeBetweenAttacks;
    bool attacking;

    //public GameObject enemyProjectile;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Checks the range for the AI
        playerTargeted = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerAttacked = Physics.CheckSphere(transform.position, attackRange, playerMask);

        // Check states
        if (!playerTargeted && !playerAttacked) Patrol();
        if (playerTargeted && !playerAttacked) Chase();
        if (playerTargeted && playerAttacked) Attack();
    }

    void SearchWalkPoint()
    {
        // Specifies a random point in the x and z direction for the AI to roam.
        float randomZPoint = Random.Range(-walkPointRange, walkPointRange);
        float randomXPoint = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomXPoint, transform.position.y, transform.position.z + randomZPoint);

        // checks that the AI is on the ground at all times in the mesh to walk
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            walkPointSet = true;
        }
    }

    void Patrol()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        //Once reaching the end of the walkpoint, it instantly looks for a new one
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        //enemy doesnt advance to the player when attacking, so it stands still when shooting
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!attacking)
        {
            // spawn projectile at player
            //Rigidbody rigidbody = Instantiate(enemyProjectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rigidbody.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            //rigidbody.AddForce(transform.up * 2f, ForceMode.Impulse);


            attacking = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        attacking = false;
    }

    //enemy takes damage and eventually dies
    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.25f);
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
