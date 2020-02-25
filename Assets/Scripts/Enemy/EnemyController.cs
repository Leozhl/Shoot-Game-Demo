using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyStats enemyStats_Template;
    [HideInInspector] public EnemyStats enemyStats;
    [HideInInspector] public int speedDownCount;
    [HideInInspector] public int frozenCount;
    public GameObject model;
    public ParticleSystem redExplosion;
    public ParticleSystem greenExplosion;
    public ParticleSystem blueExplosion;

    PlayerController player;
    NavMeshAgent agent;
    Animator animator;
    Material material;
    bool isDead;
    bool isPlayerDead;
    bool isPlayerInRange;

    public bool Dead { get { return isDead; } }

	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        material = GetComponentInChildren<Renderer>().material;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }

    void OnEnable()
    {
        enemyStats = Instantiate(enemyStats_Template);
        model.SetActive(true);
        speedDownCount = 0;
        frozenCount = 0;
        SetColor(0f, 0f, 0f);
        isDead = false;
        isPlayerInRange = false;
        isPlayerDead = false;
        agent.isStopped = false;
        agent.speed = enemyStats.speed;
        StartCoroutine(Following());
        GameManager.Instance.OnPlayerDeath += OnPlayerDeath;
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    IEnumerator Following()
    {
        Vector3 playerPosition;
        while(!isPlayerDead)
        {
            playerPosition = player.transform.position;
            agent.SetDestination(playerPosition);
            if (isPlayerInRange)
                enemyStats.Attack(player);
            yield return null;
        }
    }

    public void TakeDamage(int damage, float R, float G, float B)
    {
        if (!isDead)
        {
            enemyStats.TakeDamage(damage, R, G, B);
            SetColor(enemyStats.r, enemyStats.g, enemyStats.b);

            if (enemyStats.currentHealth <= 0)
            {
                isDead = true;
                Death();
            }
        }
    }

    void SetColor(float r, float g, float b)
    {
        material.SetColor("_Color", new Color(r, g, b));
    }

    void Death()
    {
        agent.isStopped = true;
        animator.SetTrigger("Dead");
        enemyStats.Death(this);
        StopCoroutine(Following());
        isPlayerInRange = false;
        GameManager.Instance.OnPlayerDeath -= OnPlayerDeath;
        GameManager.Instance.OnGameRestart -= OnGameRestart;
    }

    public void StartSinking()
    {
        enemyStats.Explosion(this);
        Invoke("SetInactive", 3f);
        model.SetActive(false);
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        EnemyPool.Instance.ReturnEnemy(gameObject, enemyStats.id);
    }

    void OnPlayerDeath()
    {
        isPlayerDead = true;
        isPlayerInRange = false;
        agent.isStopped = true;
        StopCoroutine(Following());
        animator.SetTrigger("PlayerDead");
    }

    void OnGameRestart()
    {
        gameObject.SetActive(false);
    }

    public PlayerController GetPlayer()
    {
        return player;
    }

    public void SetSpeed(float rate)
    {
        agent.speed = enemyStats.speed * rate;
    }

    public void RecoverSpeed(float timeBeforeRecover, bool isFrozen = false)
    {
        if (!isFrozen)
            Invoke("CheckForSpeedRecovery", timeBeforeRecover);
        else
            Invoke("CheckForFrozenRecovery", timeBeforeRecover);
    }

    void CheckForSpeedRecovery()
    {
        speedDownCount--;
        if (speedDownCount == 0 && frozenCount == 0)
        {
            SetSpeed(1f);
            Debug.Log("Speed down ends.");
        }
    }

    void CheckForFrozenRecovery()
    {
        frozenCount--;
        if (frozenCount == 0)
        {
            if (speedDownCount == 0)
            {
                SetSpeed(1f);
                Debug.Log("Frozen ends.");
            }
            else
            {
                SetSpeed(0.5f);
                Debug.Log("Frozen ends.");
            }
        }
    }
}