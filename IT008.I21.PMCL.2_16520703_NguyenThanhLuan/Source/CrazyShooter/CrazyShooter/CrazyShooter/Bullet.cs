using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrazyShooter
{
    class Bullet : Entity
    {
        private static Random rand = new Random();

        public Bullet(Vector2 position, Vector2 velocity)
        {
            image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            // xoá các viên đạn ra khỏi màn hình
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
            {
                IsExpired = true;
            }
        }
    }
}
