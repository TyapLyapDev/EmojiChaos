using System;

public class BulletInitializer : IDisposable
{
    private readonly Pool<Bullet> _pool;

    public BulletInitializer(Pool<Bullet> pool)
    {
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));

        _pool.Created += OnBulletCreated;
    }

    public void Dispose()
    {
        if (_pool != null)
            _pool.Created -= OnBulletCreated;
    }

    private void OnBulletCreated(Bullet bullet)
    {
        if (bullet == null)
            throw new ArgumentNullException(nameof(bullet));

        bullet.Initialize();
    }
}