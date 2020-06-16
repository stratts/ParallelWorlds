using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParallelWorlds
{
    class ParallelWorlds : Game
    {
        private int _scale = 2;
        private int _width = 400;
        private int _height = 240;
        private Point _windowSize;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D _topScreen;
        private RenderTarget2D _bottomScreen;

        private SpriteFont _smallFont;
        private Texture2D _dark;

        private Thread _gameThread = new Thread(new ThreadStart(Rooms.main));

        public Queue<Action> LoadQueue = new Queue<Action>();

        static void Main(string[] args)
        {
            using (var g = new ParallelWorlds())
            {
                g.Run();
            }
        }

        private ParallelWorlds()
        {
            var gdm = new GraphicsDeviceManager(this);

            _windowSize = new Point(_width * _scale, _height * _scale * 2);
            gdm.PreferredBackBufferWidth = _windowSize.X;
            gdm.PreferredBackBufferHeight = _windowSize.Y;
            gdm.IsFullScreen = false;
            gdm.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = true;

            Content.RootDirectory = Defines.ROOTPATH;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _topScreen = CreateRenderTarget();
            _bottomScreen = CreateRenderTarget();

            Defines.ScreenSize = new Point(_width, _height);

            PA.Game = this;
            _gameThread.Start();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _dark = new Texture2D(GraphicsDevice, _width, _height);
            var colors = new Color[_width * _height];
            Array.Fill(colors, Color.Black);
            _dark.SetData(colors);

            _smallFont = Content.Load<SpriteFont>("smallFont");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            _gameThread.Interrupt();
        }

        protected override void Update(GameTime gameTime)
        {
            while (LoadQueue.Count > 0)
            {
                LoadQueue.Dequeue().Invoke();
            }
            Pad.Update(Keyboard.GetState());
            WaitHandle.SignalAndWait(PA.TriggerUpdate, PA.Updated);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRenderTarget(_topScreen);
            GraphicsDevice.Clear(Color.Black);
            DrawScreen(PA.TopScreen, gameTime);

            GraphicsDevice.SetRenderTarget(_bottomScreen);
            GraphicsDevice.Clear(Color.Black);
            DrawScreen(PA.BottomScreen, gameTime);

            GraphicsDevice.SetRenderTarget(null);

            var rectSize = new Point(_width * _scale, _height * _scale);
            var topRect = new Rectangle(Point.Zero, rectSize);
            var bottomRect = new Rectangle(new Point(0, _height * _scale), rectSize);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(_topScreen, topRect, Color.White);
            _spriteBatch.Draw(_bottomScreen, bottomRect, Color.White);

            _spriteBatch.End();

            if (PA.QueueClearText) PA.ClearText();

            base.Draw(gameTime);
        }

        private RenderTarget2D CreateRenderTarget()
        {
            var renderTarget = new RenderTarget2D(GraphicsDevice, _width, _height);
            return renderTarget;
        }

        private void DrawScreen(PA.Screen screen, GameTime gameTime)
        {
            // Render backgrounds
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);

            for (int i = screen.Backgrounds.Length - 1; i >= 0; i--)
            {
                var background = screen.Backgrounds[i];
                if (background == null) continue;
                if (!background.Visible) continue;
                if (!background.Loaded) background.Load();
                var sourceRect = new Rectangle(background.Pos.ToPoint(), new Point(_width, _height));
                _spriteBatch.Draw(background.Texture, Vector2.Zero, sourceRect, Color.White);
            }

            _spriteBatch.End();

            // Render sprites
            _spriteBatch.Begin();

            foreach (var sprite in screen.Sprites)
            {
                if (sprite == null) continue;
                if (!sprite.Loaded) sprite.Load();

                var w = sprite.Size.X;
                var h = sprite.Size.Y;
                var sheetSize = new Point(sprite.Texture.Width / w, sprite.Texture.Height / h);
                var frame = new Point(sprite.Frame % sheetSize.X, sprite.Frame / sheetSize.X);
                var sourceRect = new Rectangle(new Point(frame.X * w, frame.Y * h), new Point(w, h));

                var effects = SpriteEffects.None;
                if (sprite.Flip) effects = SpriteEffects.FlipHorizontally;

                _spriteBatch.Draw(sprite.Texture, sprite.Pos + sprite.Offset.ToVector2(), sourceRect, Color.White, 0,
                    Vector2.Zero, 1, effects, 0);
            }

            // Render text
            foreach (var text in screen.Text)
            {
                if (text.Centered)
                {
                    Point box = _smallFont.MeasureString(text.Content).ToPoint();
                    var lines = text.Content.Split("\n");
                    Point linePos = new Point(_width / 2 - box.X / 2, _height / 2 - box.Y / 2);
                    foreach (var line in lines)
                    {
                        Point size = _smallFont.MeasureString(line).ToPoint();
                        int offset = box.X / 2 - size.X / 2;
                        var pos = new Vector2(linePos.X + offset, linePos.Y);
                        _spriteBatch.DrawString(_smallFont, line, pos, Color.White);
                        linePos.Y += size.Y;
                    }

                }
                else
                {
                    _spriteBatch.DrawString(_smallFont, text.Content, text.Pos, Color.White);
                }
            }

            // Adjust brightness
            if (screen.Brightness < 1)
                _spriteBatch.Draw(_dark, Vector2.Zero, new Color(Color.White, 1 - screen.Brightness));

            _spriteBatch.End();
        }
    }
}
