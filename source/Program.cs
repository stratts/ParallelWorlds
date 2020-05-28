using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParallelWorlds
{
    class ParallelWorlds : Game
    {
        static void Main(string[] args)
        {
            using (var g = new ParallelWorlds())
            {
                g.Run();
            }
        }

        private SpriteBatch _spriteBatch;

        private ParallelWorlds()
        {
            var gdm = new GraphicsDeviceManager(this);

            // Typically you would load a config here...
            gdm.PreferredBackBufferWidth = 1280;
            gdm.PreferredBackBufferHeight = 720;
            gdm.IsFullScreen = false;
            gdm.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = true;
        }

        protected override void Initialize()
        {
            /* This is a nice place to start up the engine, after
            * loading configuration stuff in the constructor
            */
            base.Initialize();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            PA.Game = this;
            Classes.setClasses();
            Levels.setLevels();
        }

        protected override void LoadContent()
        {
            // Load textures, sounds, and so on in here...
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            // Clean up after yourself!
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Run game logic in here. Do NOT render anything here!
            Rooms.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Render stuff in here. Do NOT run game logic in here!
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            for (int i = PA.MainBackgrounds.Length - 1; i >= 0; i--) {
                var background = PA.MainBackgrounds[i];
                if (background == null) continue;
                if (!background.Loaded) background.Load();
                var sourceRect = new Rectangle(background.Pos.ToPoint(), new Point(256, 192));
                _spriteBatch.Draw(background.Texture, Vector2.Zero, sourceRect, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
