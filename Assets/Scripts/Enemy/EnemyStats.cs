using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Demo/Stats/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int id;
    public int score;
    public int currentHealth;
    public int maxHealth;
    public int attack;
    public int resistance;
    public float speed;
    public float explosionRange;
    [Range(0, 1)]public float r;
    [Range(0, 1)]public float g;
    [Range(0, 1)]public float b;

    public void Attack(PlayerController player)
    {
        player.TakeDamage(attack);
    }

    public void TakeDamage(int damage, float R, float G, float B)
    {
        currentHealth -= (damage - resistance);

        if (r + R < 1f)
            r += R;
        else
            r = 1f;
        if (g + G < 1f)
            g += G;
        else
            g = 1f;
        if (b + B < 1f)
            b += B;
        else
            b = 1f;
    }

    public void Death(EnemyController enemy)
    {
        enemy.GetPlayer().score += score;
        UIManager.Instance.SetScore(enemy.GetPlayer().score);
    }

    public void Explosion(EnemyController enemy)
    {
        if (r < 0.5f && g < 0.5f && b < 0.5f)
            return;

        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, explosionRange);
        EnemyController[] enemies = new EnemyController[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
                enemies[i] = colliders[i].gameObject.GetComponent<EnemyController>();
        }

        if (r >= 0.5f)
        {
            enemy.redExplosion.Play();
            RedExplosion(enemies);
        }
        if (g >= 0.5f)
        {
            enemy.greenExplosion.Play();
            GreenExplosion(enemies);
            enemy.GetPlayer().GetHealth(20);
        }
        if (b >= 0.5f)
        {
            enemy.blueExplosion.Play();
            BlueExplosion(enemies);
        }
    }

    void RedExplosion(EnemyController[] enemies)
    {
        foreach(EnemyController enemy in enemies)
        {
            if (enemy != null && !enemy.Dead)
            {
                enemy.TakeDamage(30, 0.3f, 0, 0);
            }
        }
    }

    void GreenExplosion(EnemyController[] enemies)
    {
        foreach(EnemyController enemy in enemies)
        {
            if (enemy != null && !enemy.Dead)
            {
                enemy.TakeDamage(0, 0, 0.3f, 0);
            }
        }
    }

    void BlueExplosion(EnemyController[] enemies)
    {
        foreach(EnemyController enemy in enemies)
        {
            if (enemy != null && !enemy.Dead)
            {
                enemy.TakeDamage(20, 0, 0, 0.3f);
                if (enemy.frozenCount == 0)
                    enemy.SetSpeed(0f);
                enemy.frozenCount++;
                enemy.RecoverSpeed(5f, true);
            }
        }
    }
}