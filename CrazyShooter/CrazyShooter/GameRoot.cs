using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CrazyShooter
{
    public class GameRoot : Microsoft.Xna.Framework.Game
    {
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool paused = false;


        // constructor GameRoot
        public GameRoot()
        {
            // khởi tạo
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // thiết lập kích thước buffer
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // thêm PlayerShip vào EntityManager 
            EntityManager.Add(PlayerShip.Instance);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Sound.Music);
        }

        // Load hình ảnh, âm thanh
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);
            Sound.Load(Content);
        }

        // Update sau mỗi frame
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            Input.Update();
            
            if (Input.WasKeyPressed(Keys.P))
                paused = !paused;

            if (!paused)
            {
                EntityManager.Update();
                EnemySpawner.Update();
                PlayerStatus.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // clear buffer
            GraphicsDevice.Clear(Color.Black);

            // vẽ danh sách các entity
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.Draw(Art.Background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();

            // xuất thông tin người chơi
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            spriteBatch.DrawString(Art.Font, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
            spriteBatch.DrawString(Art.Font, "Score: " + PlayerStatus.Score, new Vector2(ScreenSize.X - Art.Font.MeasureString("Score: " + PlayerStatus.Score).X - 5, 5), Color.White);
            spriteBatch.DrawString(Art.Font, "Multiplier: " + PlayerStatus.Multiplier, new Vector2(ScreenSize.X - Art.Font.MeasureString("Multiplier: " + PlayerStatus.Multiplier).X - 5, 35), Color.White);
            
            if (PlayerStatus.IsGameOver)
            {
                string text = "Game Over\n" +
                    "Your Score: " + PlayerStatus.Score + "\n" +
                    "High Score: " + PlayerStatus.HighScore;

                Vector2 textSize = Art.Font.MeasureString(text);
                spriteBatch.DrawString(Art.Font, text, ScreenSize / 2 - textSize / 2, Color.White);
            }

            // con trỏ chuột
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);

            spriteBatch.End();
        }
    }
}
