using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    public PlayerStats playerStats_Template;
    [HideInInspector]public PlayerStats playerStats;
    public Transform bulletSpawnPoint;
    public BulletStats[] bulletStats;
    public int score;

    Rigidbody rb;
    Animator animator;
    Vector3 movement;
    int floorMask;
    int bulletStatsIndex;
    float cameraRayLength;
    float invincibleTimer;

	void Awake ()
    {
        playerStats = Instantiate(playerStats_Template);
        UIManager.Instance.Init();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        floorMask = LayerMask.GetMask("Floor");
        bulletStatsIndex = 0;
        cameraRayLength = 100f;
        invincibleTimer = 0f;
	}


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject tmp = BulletPool.Instance.GetBullet();
            if(tmp != null)
            {
                tmp.transform.position = bulletSpawnPoint.position;
                tmp.transform.forward = bulletSpawnPoint.forward;
                tmp.GetComponent<BulletController>().bulletStats = bulletStats[bulletStatsIndex];
                tmp.SetActive(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            bulletStatsIndex = (bulletStatsIndex + 1) % bulletStats.Length;
            UIManager.Instance.SetColor();
        }

        invincibleTimer += Time.deltaTime;
    }

    void FixedUpdate ()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Move(h, v);
        Turn();
        SetAnimator(h, v);
	}

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * playerStats.speed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);
    }

    void Turn()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if(Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;

            rb.MoveRotation(Quaternion.LookRotation(playerToMouse));
        }
    }

    void SetAnimator(float h,float v)
    {
        bool isWalking = h != 0f || v != 0f;
        animator.SetBool("IsWalking", isWalking);
    }

    public void TakeDamage(int damage)
    {
        if (invincibleTimer > playerStats.invincibleTime)
        {
            invincibleTimer = 0f;
            playerStats.TakeDamage(damage);

            UIManager.Instance.SetHealthSlider((float)playerStats.currentHealth / playerStats.maxHealth);

            if (playerStats.currentHealth <= 0)
                Death();
        }
    }

    public void GetHealth(int health)
    {
        playerStats.GetHealth(health);

        UIManager.Instance.SetHealthSlider((float)playerStats.currentHealth / playerStats.maxHealth);
    }

    void Death()
    {
        animator.SetTrigger("Dead");

        GameManager.Instance.PlayerDeath();
    }

    void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }
}