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
		AssetManager assetManager = AssetManager.CreateFileAssetManager(getFullPath("Assets"));
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Effect spriteEffect;


		// this will be the FixedUpdate frequency, we set it to 30 FPS
		private float fixedUpdateDelta = (int)(1000 / (float)30);

		// helper variables for the fixed update
		private float previousT = 0;
		private float accumulator = 0.0f;
		private float maxFrameTime = 250;

		// this value stores how far we are in the current frame. For example, when the 
		// value of ALPHA is 0.5, it means we are halfway between the last frame and the 
		// next upcoming frame.
		private float ALPHA = 0;

		public static string getFullPath(string relativePath) {
			string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
			return System.IO.Path.Combine(baseDirectory, relativePath);
		}



		public int windowWidth;
		public int windowHeight;



		public Game1() {
			Content.RootDirectory = "Content";
			graphics = new GraphicsDeviceManager(this) {
				PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8,
			};

		}

		protected override void Initialize() {
			windowWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
			windowHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

			windowWidth = 640;
			windowHeight = 480;

			Window.Title = "cycle";

			Window.AllowUserResizing = false;

			graphics.SynchronizeWithVerticalRetrace = false; //Vsync
			graphics.HardwareModeSwitch = false;
			graphics.PreferredBackBufferWidth = windowWidth;
			graphics.PreferredBackBufferHeight = windowHeight;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();
			IsFixedTimeStep = true;
			//TargetElapsedTime = TimeSpan.FromMilliseconds(33);
			base.Initialize();
		}


		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			byte[] shaderbytecode = File.ReadAllBytes(Utility.getFullPath("Content/ShaderBin/effect.out"));
			spriteEffect = new Effect(GraphicsDevice, shaderbytecode);

			Texture2D paletteImport = Content.Load<Texture2D>("palette_pinknoise");
			testPlasma = new Plasma2(windowWidth, windowHeight, GraphicsDevice, paletteImport);

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
			ferrari_cycler = new PaletteCycler(waterfallTexture, cycleRegions, 0.1f);

			Texture2D waterfall_yoshi_tile = Content.Load<Texture2D>("waterfall_yoshi_tile");
			yoshi_cycler = new PaletteCycler(waterfall_yoshi_tile, new CycleRegion[] { new(9, 13) }, 0.1f);

		}

		Plasma2 testPlasma;

		PaletteCycler ferrari_cycler;
		PaletteCycler yoshi_cycler;

		protected override void Update(GameTime gameTime) {

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();


			//testPlasma.Update(gameTime);
			ferrari_cycler.Update(gameTime);
			yoshi_cycler.Update(gameTime);
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
			Rectangle drawingArea = new(0, 0, windowWidth, windowHeight);
			//testPlasma.Draw(spriteBatch, gameTime, drawingArea, new(0, 0, testPlasma.width, testPlasma.height));


			//cycler.Draw(spriteBatch, gameTime);
			spriteBatch.Draw(ferrari_cycler.texture, new Vector2(0, -1), Color.White);
			spriteBatch.Draw(yoshi_cycler.texture, new Vector2(64, 32), new Rectangle(0, 1, 32, 32), Color.White);
			for (int i = 64; i < 512; i += 32) {
				spriteBatch.Draw(yoshi_cycler.texture, new Vector2(64, i), new Rectangle(0, 33, 32, 32), Color.White);
			}


			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
