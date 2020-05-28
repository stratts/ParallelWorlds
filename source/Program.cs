using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        private int _width = 256;
        private int _height = 192;
        private SpriteBatch _spriteBatch;

        private ParallelWorlds()
        {
            var gdm = new GraphicsDeviceManager(this);

            // Typically you would load a config here...
            gdm.PreferredBackBufferWidth = 256;
            gdm.PreferredBackBufferHeight = 192;
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
            Pad.Update(Keyboard.GetState());
            Rooms.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Render backgrounds
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);

            for (int i = PA.MainBackgrounds.Length - 1; i >= 0; i--) {
                var background = PA.MainBackgrounds[i];
                if (background == null) continue;
                if (!background.Loaded) background.Load();
                var sourceRect = new Rectangle(background.Pos.ToPoint(), new Point(_width, _height));
                _spriteBatch.Draw(background.Texture, Vector2.Zero, sourceRect, Color.White);
            }

            _spriteBatch.End();

            // Render sprites
            _spriteBatch.Begin();

            foreach (var sprite in PA.Sprites) {
                if (sprite == null) continue;
                if (!sprite.Loaded) sprite.Load();

                var w = sprite.Size.X;
                var h = sprite.Size.Y;
                var sheetSize = new Point(sprite.Texture.Width / w, sprite.Texture.Height / h);
                var frame = new Point(sprite.Frame % sheetSize.X, sprite.Frame / sheetSize.X);
                var sourceRect = new Rectangle(new Point(frame.X * w, frame.Y * h), new Point(w, h));

                var effects = SpriteEffects.None;
                if (sprite.Flip) effects = SpriteEffects.FlipHorizontally;

                _spriteBatch.Draw(sprite.Texture, sprite.Pos, sourceRect, Color.White, 0,
                    Vector2.Zero, 1, effects, 0);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
