using UnityEngine;

[CreateAssetMenu(fileName = "GreenBulletStats", menuName = "Demo/Stats/BulletStats/GreenBullet", order = 3)]
public class GreenBulletStats : BulletStats, IGreenBullet
{
    public int healingAmount;

    protected override void BulletEffect(EnemyController enemy)
    {
        base.BulletEffect(enemy);
        Heal(enemy.GetPlayer());
    }

    public void Heal(PlayerController player)
    {
        player.GetHealth(healingAmount);
    }
}