using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.MathF;
namespace kitchen
{
	public class Plasma
	{
		public int Width {
			get {
				return width;
			}
		}
		int width;
		public int Height {
			get {
				return height;
			}
		}
		int height;

		int x, y;
		float sec;
		float dx, dy, dv;

		Texture2D texture;

		Color[] palette;
		int[] plasma;
		Color[] buffer;

		Texture2D paletteImport;

		public Plasma(int width, int height, GraphicsDevice graphicsDevice, Texture2D paletteImport = null) {
			this.width = width;
			this.height = height;
			this.paletteImport = paletteImport;
			texture = new(graphicsDevice, width, height, false, SurfaceFormat.Color);


			palette = new Color[paletteImport.Width];

			if (paletteImport != null) {
				paletteImport.GetData(palette);
			}

			//palette = new Color[2];
			//palette[0] = Color.Black;
			//palette[1] = Color.White;

			plasma = new int[width * height];
			buffer = new Color[width * height];

			//palette = new Color[360];
			//for (int i = 0; i < palette.Length; i++) {
			//	palette[i] = HSVtoRGB((i) % 360, 1, 1);
			//	//Color color = new();
			//	//color.R = (byte)(128.0f + 128 * Sin((float)(PI * i / 32.0)));
			//	//color.G = (byte)(128.0f + 128 * Sin((float)(PI * i / 64.0)));
			//	//color.B = (byte)(128.0f + 128 * Sin((float)(PI * i / 128.0)));
			//	//palette[i] = color;
			//}


			float gray = 128.0f; // h

			for (y = 0; y < height; y++) {
				for (x = 0; x < width; x++) {
					int value = (int)(
						gray + (gray * Sin(x / 30.0f))
					  + gray + (gray * Sin(y / 50.0f))
					  + gray + (gray * Sin((Sqrt(((x - width / 2.0f) * (x - width / 2.0f) + (y - height / 2.0f) * (y - height / 2.0f))) / 20.37f)))
					  + gray + (gray * Sin((Sqrt(x * x + y * y) / 30.4f)))
					) / 8;
					plasma[(y * width) + x] = value;
				}
			}

			texture.SetData(plasma);

		}


		int t;
		float rate;

		public void Update(GameTime gameTime) {
			int prevT = t;
			t = (int)(gameTime.TotalGameTime.TotalSeconds * 30.0f);

			//rate -= gameTime.ElapsedGameTime.Seconds;

			//for (y = 0; y < height; y++)
			//	for (x = 0; x < width; x++) {
			//		buffer[(y * width) + x] = palette[(plasma[(y * width) + x] + paletteShift) % palette.Length];
			//	}


			if (prevT != t) {
				for (int i = 0; i < width * height; i++) {
					buffer[i] = palette[(plasma[i] + t) % palette.Length];
				}
				texture.SetData(buffer);
			} 
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle destination, Rectangle source) {
			spriteBatch.Draw(texture, destination, source, Color.White);
		}



		/// <summary>
		/// Takes a colour in HSV colour space and returns it in RGB space as an XNA Color object.
		/// </summary>
		/// <param name="H">'Hue' between 0 and 360.</param>
		/// <param name="S">'Saturation' between 0 and 1</param>
		/// <param name="V">'Value' between 0 and 1</param>
		/// <returns></returns>
		public static Color HSVtoRGB(int H, double S, double V) {
			double C = V * S;
			double X = C * (1 - Math.Abs((H / 60.0) % 2 - 1));
			double m = V - C;

			int r, g, b;
			if (H < 60) {
				r = (int)((C + m) * 255);
				g = (int)((X + m) * 255);
				b = (int)(m * 255);
			} else if (H < 120) {
				r = (int)((X + m) * 255);
				g = (int)((C + m) * 255);
				b = (int)(m * 255);
			} else if (H < 180) {
				r = (int)(m * 255);
				g = (int)((C + m) * 255);
				b = (int)((X + m) * 255);
			} else if (H < 240) {
				r = (int)(m * 255);
				g = (int)((X + m) * 255);
				b = (int)((C + m) * 255);
			} else if (H < 300) {
				r = (int)((X + m) * 255);
				g = (int)(m * 255);
				b = (int)((C + m) * 255);
			} else {
				r = (int)((C + m) * 255);
				g = (int)(m * 255);
				b = (int)((X + m) * 255);
			}

			return new Color(r, g, b);
		}




	}
}
