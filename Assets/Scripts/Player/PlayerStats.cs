using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Demo/Stats/Playerstats")]
public class PlayerStats : ScriptableObject
{
    public int currentHealth;
    public int maxHealth;
    public int currentBulletAmount;
    public int maxBulletAmount;
    public float speed;
    public float invincibleTime;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;      
    }

    public void GetHealth(int health)
    {
        if (currentHealth + health <= maxHealth)
            currentHealth += health;
        else
            currentHealth = maxHealth;
    }
}