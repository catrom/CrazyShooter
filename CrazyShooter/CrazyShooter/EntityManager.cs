using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrazyShooter
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullet> bullets = new List<Bullet>();
        static List<BlackHole> blackHoles = new List<BlackHole>();

        public static IEnumerable<BlackHole> BlackHoles { get { return blackHoles; } }

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>(); // entity mới

        public static int Count { get { return entities.Count; } }
        public static int BlackHoleCount { get { return blackHoles.Count; } }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
            {
                entities.Add(entity);
                if (entity is Bullet)
                    bullets.Add(entity as Bullet);
                else if (entity is Enemy)
                    enemies.Add(entity as Enemy);
                else if (entity is BlackHole)
                    blackHoles.Add(entity as BlackHole);
            }
            else
                addedEntities.Add(entity);
        }

        public static void Update()
        {
            isUpdating = true;

            // xử lý đụng độ
            HandleCollisions();

            // update cho từng entity
            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            // cập nhật lại các danh sách entities, bullets, enemies, blackHoles
            // Note: Nếu thay đổi trực tiếp ở các list trên trong quá trình lặp sẽ gây ra vòng lặp vô hạn
            // Do đó cần một list addedEntities để lưu những entities mới và tiến hành cập nhật list cũ như sau.
            foreach (var entity in addedEntities)
            {
                entities.Add(entity);
                if (entity is Bullet)
                    bullets.Add(entity as Bullet);
                else if (entity is Enemy)
                    enemies.Add(entity as Enemy);
                else if (entity is BlackHole)
                    blackHoles.Add(entity as BlackHole);
            }

            addedEntities.Clear();

            // loại bỏ những entities bị expired.
            entities = entities.Where(x => !x.IsExpired).ToList();
            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
            blackHoles = blackHoles.Where(x => !x.IsExpired).ToList();
        }

        // xử lý đụng độ
        static void HandleCollisions()
        {
            // xử lý đụng độ giữa enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }

            // bullets và enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        enemies[i].WasShot();
                        bullets[j].IsExpired = true;
                    }
                }

            // player và enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(PlayerShip.Instance, enemies[i]))
                {
                    KillPlayer();
                    break;
                }
            }

            // enemies và bullets với black holes
            for (int i = 0; i < blackHoles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                    if (enemies[j].IsActive && IsColliding(blackHoles[i], enemies[j]))
                        enemies[j].WasShot();

                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(blackHoles[i], bullets[j]))
                    {
                        bullets[j].IsExpired = true;
                        blackHoles[i].WasShot();
                    }
                }

                // playership với blackhole
                if (IsColliding(PlayerShip.Instance, blackHoles[i]))
                {
                    KillPlayer();
                    break;
                }
            }
        }

        private static void KillPlayer()
        {
            
            PlayerShip.Instance.Kill();
            enemies.ForEach(x => x.WasShot());
            blackHoles.ForEach(x => x.Kill());
            EnemySpawner.Reset();
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        // lấy danh sách entity nằm trong vùng radius
        public static IEnumerable<Entity> GetNearbyEntities(Vector2 position, float radius)
        {
            return entities.Where(x => Vector2.DistanceSquared(position, x.Position) < radius * radius);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }
    }
}