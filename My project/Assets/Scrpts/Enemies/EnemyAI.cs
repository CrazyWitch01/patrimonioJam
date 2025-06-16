using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsGround,WhatIsPlayer;

    public Vector3 WalkPoint;
    public bool IsWalkPointSet;
    public float WalkPointRange;
    public float Health;
    public float SpeedNormal = 11f;


    //Ataques
    public float TimeBetweenAttacks;
    public bool AlreadyAttacked;
    public GameObject projectile;
    public bool PuedeAtacar = true;


    //states

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, enemyFreezePos;

    //animaciones

    private Animator animaciones;

    //hurtbox
    public GameObject Hurtbox;
    private BoxCollider hurtboxCollider;
    public bool canBeDamaged = true;

    //Audio
    AudioSource AudioSourceEnemigo;
    [SerializeField] AudioClip[] AudioAtaques;
    [SerializeField] AudioClip[] AudioHit;
    [SerializeField] AudioClip AudioMuerte;

    //particulas
    public ParticleSystem PartHit;
    public ParticleSystem PartMuerte;

    private void Awake()
    {
        AudioSourceEnemigo = GetComponent<AudioSource>();
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        animaciones = GetComponentInChildren<Animator>();
        hurtboxCollider = Hurtbox.GetComponent<BoxCollider>();
        hurtboxCollider.isTrigger = true;
    }

    private void ChasePlayer()
    {
        enemyFreezePos = false;
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        if (!PuedeAtacar)
        {
            Debug.Log("NoPuedeAtacar");
            animaciones.SetBool("IsAttacking", false);
            return;
        }
        agent.SetDestination(transform.position);
        animaciones.SetBool("IsAttacking", true);
        animaciones.SetBool("IsRunning", false);

        transform.LookAt(player);

        if (!AlreadyAttacked)
        {
            AudioClip clipAtaque = AudioAtaques[Random.Range(0, AudioAtaques.Length)];
            AudioSourceEnemigo.PlayOneShot(clipAtaque);


            //ataque proyectil
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 28f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            //xd
            AlreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
            enemyFreezePos = true;
        }
    }
    private void ResetAttack()
    {
        AlreadyAttacked = false;


    }

    private void Patrolling()
    {
        enemyFreezePos = false;
        if (!IsWalkPointSet) SearchWalkPoint();

        if (IsWalkPointSet)
        {
            agent.SetDestination(WalkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - WalkPoint;


        if (distanceToWalkPoint.magnitude < 1f)
        {
            IsWalkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + randomX, transform.position.y ,transform.position.z + randomZ);

        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround))
        {
            IsWalkPointSet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyFreezePos)
        {
            agent.speed = 0f;
        }
        else
        {
            agent.speed = SpeedNormal;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
            animaciones.SetBool("IsRunning", false);
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            animaciones.SetBool("IsRunning", true);
        }
        if (playerInAttackRange && playerInSightRange && PuedeAtacar)
        {
            AttackPlayer();
        }
        else
        {
            animaciones.SetBool("IsAttacking", false);
        }

    }
    void LateUpdate()
    {
        Vector3 currentEuler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, currentEuler.y, currentEuler.z);
    }

    public void TakeDamage(int damage)
    {
        if (!canBeDamaged)
        {
            return; 
        }
        canBeDamaged = false;
        Invoke(nameof(CanBeDamaged), 1f);
        Health -= damage;
        animaciones.SetTrigger("IsHit");
        PuedeAtacar = false;
        Invoke(nameof(EnableAttack), 2f);
        if (Health > 0)
        {
            AudioClip clipHit = AudioHit[Random.Range(0, AudioHit.Length)];
            AudioSourceEnemigo.PlayOneShot(clipHit);
            PartHit.Play();
            Destroy(PartHit.gameObject, 3f);
        }

        if (Health <= 0)
        {
            canBeDamaged = false;
            animaciones.SetBool("IsDead",true);
            AudioSourceEnemigo.PlayOneShot(AudioMuerte);
            Invoke(nameof(DestroyEnemies),2f);
            PartMuerte.Play();
            Destroy(PartMuerte.gameObject, 2f);
        }
    }


    private void DestroyEnemies()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,sightRange);
    }

    private void EnableAttack()
    {
        PuedeAtacar=true;
    }
    private void CanBeDamaged()
    {
        canBeDamaged =true;
    }
}
