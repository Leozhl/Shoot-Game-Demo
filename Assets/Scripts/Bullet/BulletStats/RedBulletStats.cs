using UnityEngine;

[CreateAssetMenu(fileName = "RedBulletStats", menuName = "Demo/Stats/BulletStats/RedBullet", order = 2)]
public class RedBulletStats : BulletStats, IRedBullet
{
    protected override void BulletEffect(EnemyController enemy)
    {
        base.BulletEffect(enemy);
    }
}
