﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CrazyShooter
{
    static class Art
    {
        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Wanderer { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static Texture2D BlackHole { get; private set; }
        public static Texture2D Background { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static void Load(ContentManager content)
        {
            Background = content.Load<Texture2D>("Art/Background");
            Player = content.Load<Texture2D>("Art/Player");
            Seeker = content.Load<Texture2D>("Art/Seeker");
            Wanderer = content.Load<Texture2D>("Art/Wanderer");
            Bullet = content.Load<Texture2D>("Art/Bullet");
            Pointer = content.Load<Texture2D>("Art/Pointer");
            BlackHole = content.Load<Texture2D>("Art/Black Hole");

            Font = content.Load<SpriteFont>("Font");
        }
    }
}
