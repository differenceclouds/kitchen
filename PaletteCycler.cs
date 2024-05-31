using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kitchen {

	public struct CycleRegion {
		public readonly int cycleStart;
		public readonly int cycleEnd;
		public readonly float rateMultiplier;
		public CycleRegion(int _cycleStart, int _cycleEnd, float _rateMultiplier = 1) {
			cycleStart = _cycleStart;
			cycleEnd = _cycleEnd;
			rateMultiplier = _rateMultiplier;
		}
	}


	public class PaletteCycler {

		public Texture2D texture { get; }
		public Color[] palette { get; }
		Color[] textureData;
		Color[] paletteBuffer;
		Color terminateColor = new Color(0, 255, 0);

		int[] texturePaletteIndicies;

		int texturesize;
		int palettesize;

		CycleRegion[] cycleRegions;
		float[] timers; //seperate array to keep cycleRegions immutable

		public Rectangle TextureBounds { get; }


		public PaletteCycler(Texture2D _texture, CycleRegion[] _cycleRegions, float cycleRate, Rectangle? _textureBounds = null) {
			texture = _texture;
			TextureBounds = _textureBounds ?? new Rectangle(0, 1, texture.Width, texture.Height - 1);

			texturesize = texture.Width * texture.Height;

			textureData = new Color[texturesize];
			texturePaletteIndicies = new int[texturesize];

			texture.GetData(textureData);

			palettesize = 0;
			for (int i = 0; i < texturesize; i++) {
				palettesize = i;
				if (textureData[i] == terminateColor)
					break;
			}

			palette = new Color[palettesize];
			paletteBuffer = new Color[palettesize];

			palette = textureData[0..palettesize];


			for (int i = 0; i < texturesize; i++) {
				for(int j = 0; j < palettesize; j++) {
					if(textureData[i] == palette[j]) {
						texturePaletteIndicies[i] = j;
						break;
					}
				}
			}

			rate = cycleRate;
			cycleRegions = _cycleRegions;
			timers = new float[cycleRegions.Length];
		}


		//float time = 0;
		float rate;


		public void Update(GameTime gameTime) {
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			for (int r = 0; r < timers.Length; r++) {
				CycleRegion region = cycleRegions[r];
				timers[r] += elapsed;
				if (timers[r] > rate / region.rateMultiplier) {
					timers[r] = 0;

					int cycleStart = region.cycleStart;
					int cycleLength = region.cycleEnd - cycleStart;
					
					palette.CopyTo(paletteBuffer, 0);

					for (int i = cycleStart; i < cycleStart + cycleLength; i++) {
						int newindex = Utility.wrapIndex(i - cycleStart - 1, cycleLength);
						palette[i] = paletteBuffer[newindex + cycleStart];
					}

					for (int i = 0; i < texturesize; i++) {
						textureData[i] = palette[texturePaletteIndicies[i]];
					}
					texture.SetData(textureData);
				}
			}

		}


	}
}
