using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private string currentState;
    private GameObject player;

    public Vector3 LastKnowPos { get; set; }
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }

    public int currentHealth;
    public int maxHealth;
    public int healthDamageAmount;

    public Path path;
    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight = 1.5f;
    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;
    [SerializeField]
    private bool playerInSight = false;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    void Update()
    {
        bool canSeePlayer = CanSeePlayer();
        currentState = stateMachine.activeState.ToString();

        if (canSeePlayer != playerInSight)
        {
            playerInSight = canSeePlayer;
            Debug.Log("Player visibility changed: " + playerInSight);
        }
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = (player.transform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                if (angleToPlayer <= fieldOfView / 2)
                {
                    Vector3 rayOrigin = transform.position + Vector3.up * eyeHeight;
                    Ray ray = new Ray(rayOrigin, targetDirection);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);

                        if (hitInfo.transform.gameObject == player)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void EnemyDamage()
    {
        currentHealth -= healthDamageAmount;

        if (currentHealth <= 0)
        {
            // Award a point when the enemy is destroyed
            ScoreManager.instance.AddPoint();
            Destroy(gameObject);
        }
    }
}
