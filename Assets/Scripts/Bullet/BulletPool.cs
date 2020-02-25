using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public GameObject bulletPrefab;
    public int amount;

    Queue<GameObject> bulletPool;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        bulletPool = new Queue<GameObject>();

        for(int i = 0; i < amount; i++)
        {
            GameObject tmp = Instantiate(bulletPrefab, transform);
            bulletPool.Enqueue(tmp);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            Debug.Log("No bullet!");
            return null;
        }
        
        GameObject tmp = bulletPool.Dequeue();
        return tmp;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bulletPool.Enqueue(bullet);
    }
}