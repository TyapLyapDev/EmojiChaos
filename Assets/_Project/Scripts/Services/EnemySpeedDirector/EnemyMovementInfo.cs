public class EnemyMovementInfo
{
    private readonly Enemy _enemy;
    private readonly float _forwardDistance;

    public EnemyMovementInfo(Enemy enemy, float forwardDistance)
    {
        _enemy = enemy;
        _forwardDistance = forwardDistance;
    }

    public Enemy Enemy => _enemy;

    public float RequiredDistance => _forwardDistance;
}