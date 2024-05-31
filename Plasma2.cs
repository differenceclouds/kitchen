using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.MathF;
namespace kitchen {

	public class Plasma2 {
		public readonly int width, height;

		Texture2D texture;

		Color[] palette, buffer;
		int[] plasma;

		public Plasma2(int width, int height, GraphicsDevice graphicsDevice, Texture2D paletteImport = null) {
			this.width = width;
			this.height = height;
			texture = new(graphicsDevice, width, height, false, SurfaceFormat.Color);

			if (paletteImport != null) {
				palette = new Color[paletteImport.Width * paletteImport.Height];
				paletteImport.GetData(palette);
			} else {
				palette = new Color[360];
				for (int i = 0; i < palette.Length; i++) {
					palette[i] = HSVtoRGB((i) % 360, 1, 1);
				}
			}

			plasma = new int[width * height];
			buffer = new Color[width * height];
			

			float gray = 128.0f; // h

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
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
			//for (int i = 0; i < width * height; i++) {
			//	buffer[i] = palette[(plasma[i]) % palette.Length];
			//}
			//texture.SetData(buffer);
		}

		int n = 0;

		float rate = 0.1f;
		float time = 0;
		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle destination, Rectangle source) {
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (time > rate) {
				time = 0;

				var bufferSpan = new Span<Color>(buffer);
				var paletteSpan = new Span<Color>(palette);
				var plasmaSpan = new Span<int>(plasma);
				for (int i = 0; i < width * height; i++) {
					bufferSpan[i] = paletteSpan[(plasmaSpan[i] + n) % palette.Length];
				}

				n++;

				texture.SetData(bufferSpan.ToArray());
			}
			spriteBatch.Draw(texture, destination, source, Color.White);
		}

		/// <summary>
		/// Takes a colour in HSV colour space and returns it in RGB space as an XNA Color object.
		/// </summary>
		/// <param name="H">Hue takes values between 0 and 360.</param>
		/// <param name="S">Saturation takes values between 0 and 1</param>
		/// <param name="V">Value takes values between 0 and 1</param>
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
