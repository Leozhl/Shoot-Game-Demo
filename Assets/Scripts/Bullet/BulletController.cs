using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [HideInInspector]public BulletStats bulletStats;

    Rigidbody rb;
    float timer;
    Vector3 movement;
    Material bulletMaterial;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timer = 0f;
        bulletMaterial = GetComponent<Renderer>().material;
    }

    void OnEnable()
    {
        bulletMaterial.SetColor("_Color", new Color(bulletStats.r, bulletStats.g, bulletStats.b));
        StartCoroutine(Firing());
    }

    IEnumerator Firing()
    {
        while(timer < 5f)
        {
            movement = transform.forward * bulletStats.speed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);
            timer += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            bulletStats.OnCollision(enemy);
        }
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        StopCoroutine(Firing());
        timer = 0f;

        BulletPool.Instance.ReturnBullet(gameObject);
    }
}