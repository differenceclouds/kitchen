using System;
using System.IO;
using AssetManagementBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace kitchen {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public int WindowWidth { get; private set; }
		public int WindowHeight { get; private set; }

		Effect spriteEffect;

		public Game1() {
			Content.RootDirectory = "Content";
			graphics = new GraphicsDeviceManager(this) {
				PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8,
			};
		}

		protected override void Initialize() {
			WindowWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
			WindowHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

			WindowWidth = 640;
			WindowHeight = 480;

			Window.Title = "cycle";

			Window.AllowUserResizing = false;

			graphics.SynchronizeWithVerticalRetrace = false; //Vsync
			graphics.HardwareModeSwitch = false;
			graphics.PreferredBackBufferWidth = WindowWidth;
			graphics.PreferredBackBufferHeight = WindowHeight;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();
			IsFixedTimeStep = true;
			base.Initialize();
		}


		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			byte[] shaderbytecode = File.ReadAllBytes(Utility.getFullPath("Content/ShaderBin/effect.out"));
			spriteEffect = new Effect(GraphicsDevice, shaderbytecode);

			Texture2D paletteImport = Content.Load<Texture2D>("palette_pinknoise");
			testPlasma = new Plasma2(512, 512, GraphicsDevice, paletteImport);

			Texture2D waterfallTexture = Content.Load<Texture2D>("ferrari_waterfall");
			CycleRegion[] cycleRegions = {
				new(119, 126, .8f),
				new(127, 134, .6f),
				new(135, 143, .4f),
				new(175, 181),
				new(182, 188),
				new(189, 195),
				new(196, 202),
				new(203, 209),
				new(210, 216),
				new(217, 223)


			};
			ferrari_cycler = new PaletteCycler(GraphicsDevice, waterfallTexture, cycleRegions, 0.1f);

			Texture2D waterfall_yoshi_tile = Content.Load<Texture2D>("waterfall_yoshi_tile");
			yoshi_cycler = new PaletteCycler(GraphicsDevice, waterfall_yoshi_tile, new CycleRegion[] { new(9, 13) }, 0.1f);

		}

		Plasma2 testPlasma;

		PaletteCycler ferrari_cycler;
		PaletteCycler yoshi_cycler;

		protected override void Update(GameTime gameTime) {

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();


			//testPlasma.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			//spriteEffect.CurrentTechnique.Passes[0].Apply();
			Rectangle drawingArea = new(64, 128, 256, 256);




			spriteBatch.Draw(ferrari_cycler.Texture, new Vector2(0, -1), Color.White);

			//testPlasma.Draw(spriteBatch, gameTime, drawingArea, new(0, 0, testPlasma.width, testPlasma.height));

			spriteBatch.Draw(yoshi_cycler.Texture, new Vector2(128, 32), new Rectangle(0, 1, 32, 32), Color.White);
			int y = 64;
			while (y < 512) {
				spriteBatch.Draw(yoshi_cycler.Texture, new Vector2(128, y), new Rectangle(0, 33, 32, 32), Color.White);
				y += 32;
			}

			ferrari_cycler.Draw(gameTime);
			yoshi_cycler.Draw(gameTime);


			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
