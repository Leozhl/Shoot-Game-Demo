using UnityEngine;

[CreateAssetMenu(fileName = "BlueBulletStats", menuName = "Demo/Stats/BulletStats/BlueBullet", order = 4)]
public class BlueBulletStats : BulletStats, IBlueBullet
{
    public float speedDownTime;
    public float speedDownRate;

    protected override void BulletEffect(EnemyController enemy)
    {
        base.BulletEffect(enemy);
        SpeedDown(enemy);
    }

    public void SpeedDown(EnemyController enemy)
    {
        if (enemy.speedDownCount == 0)
            enemy.SetSpeed(speedDownRate);
        enemy.speedDownCount++;
        enemy.RecoverSpeed(speedDownTime);
    }
}
