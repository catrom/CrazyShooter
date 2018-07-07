using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrazyShooter
{
    static class EnemySpawner
    {
        static Random rand = new Random();
        static float inverseSpawnChance = 90;      // randomtime để sản sinh enemy sau mỗi frame
        static float inverseBlackHoleChance = 600; // randomtime để sản sinh blackhole sau mỗi frame

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                // random trong khoảng (0,inverseSpawnChance) để quyết định thêm mới Seeker 
                if (rand.Next((int)inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));

                // random trong khoảng (0,inverseSpawnChance) để quyết định thêm mới Wanderer 
                if (rand.Next((int)inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));

                // random trong khoảng (0,inverseBlackHoleChance) để quyết định thêm mới BlackHole
                if (EntityManager.BlackHoleCount < 2 && rand.Next((int)inverseBlackHoleChance) == 0)
                    EntityManager.Add(new BlackHole(GetSpawnPosition()));
            }

            // giảm mức random time -> đẩy nhanh thời gian xuất hiện ngẫu nhiên của enemy -> tăng độ khó
            if (inverseSpawnChance > 30)
                inverseSpawnChance -= 0.005f;
            if (inverseBlackHoleChance > 200)
                inverseBlackHoleChance -= 0.01f;
        }

        // lấy vị trí ngẫu nhiên cho enemy
        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)GameRoot.ScreenSize.X), rand.Next((int)GameRoot.ScreenSize.Y));
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 250);

            return pos;
        }

        // reset lại thông số (độ khó) sau khi player killed
        public static void Reset()
        {
            inverseSpawnChance = 90;
            inverseBlackHoleChance = 600;
        }
    }
}
