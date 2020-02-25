using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Demo/Stats/BulletStats/BaseBullet", order = 1)]
public class BulletStats : ScriptableObject
{
    public int damage;
    [Range(0, 1)]public float r;
    [Range(0, 1)]public float g;
    [Range(0, 1)]public float b;
    public float speed;
    
    public void OnCollision(EnemyController enemy)
    {
        enemy.TakeDamage(damage, r, g, b);
        BulletEffect(enemy);
    }

    protected virtual void BulletEffect(EnemyController enemy) { }
}