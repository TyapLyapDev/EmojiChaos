using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace EmojiChaos.Services.Movement
{
    using Entities.Bullet;

    public class BulletMovementDirector : IDisposable
    {
        private readonly float _speed;

        private readonly List<Bullet> _bullets = new();
        private readonly CompositeDisposable _disposables = new();

        public BulletMovementDirector(float speed)
        {
            if (speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(speed), speed, "�������� ������ ���� ������ ����");

            _speed = speed;
        }

        public void Run()
        {
            Observable.EveryUpdate()
                .Where(_ => _bullets.Count > 0)
                .Subscribe(_ => UpdateMovement())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            List<Bullet> bullets = _bullets;

            foreach (Bullet bullet in bullets)
                if (bullet != null)
                    bullet.Deactivated -= OnBulletDeactivated;

            _bullets.Clear();
            _disposables?.Dispose();
        }

        public void RegisterBullet(Bullet bullet)
        {
            if (bullet == null)
                throw new ArgumentNullException(nameof(bullet));

            if (_bullets.Contains(bullet))
                throw new InvalidOperationException($"{nameof(RegisterBullet)} ������� ��������� ����������� ����");

            _bullets.Add(bullet);
            bullet.Deactivated += OnBulletDeactivated;
        }

        private void UpdateMovement()
        {
            float deltaDistance = _speed * Time.deltaTime;

            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];

                if (bullet == null)
                {
                    _bullets.RemoveAt(i);
                    continue;
                }

                bullet.Move(deltaDistance);
            }
        }

        private void OnBulletDeactivated(Bullet bullet)
        {
            if (bullet == null)
                throw new ArgumentNullException(nameof(bullet));

            bullet.Deactivated -= OnBulletDeactivated;
            _bullets.Remove(bullet);
        }
    }
}