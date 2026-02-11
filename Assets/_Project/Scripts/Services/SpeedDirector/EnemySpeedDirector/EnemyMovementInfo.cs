public class EnemyMovementInfo
{
    private readonly Enemy _enemy;
    private float _distance;

    public EnemyMovementInfo(Enemy enemy, float distance)
    {
        _enemy = enemy;
        _distance = distance;
    }

    public Enemy Enemy => _enemy;

    public float Distance => _distance;

    public void SetDistance(float distance) =>
        _distance = distance;
}