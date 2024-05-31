using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kitchen {

	public struct CycleRegion {
		public readonly int cycleStart;
		public readonly int cycleLength;
		public readonly float rateMultiplier;
		public float time;
		public CycleRegion(int _cycleStart, int _cycleLength, float _rateMultiplier = 1) {
			cycleStart = _cycleStart;
			cycleLength = _cycleLength;
			rateMultiplier = _rateMultiplier;
			time = 0;
		}
	}


	public class PaletteCycler {

		public Texture2D texture { get; }
		public Color[] palette { get; }
		Color[] textureData;
		Color[] paletteBuffer;
		Color terminateColor = new Color(0, 255, 0);

		int[] textureColorIndicies;

		int texturesize;
		int palettesize;

		CycleRegion[] cycleRegions;




		public PaletteCycler(Texture2D _texture, CycleRegion[] _cycleRegions, float cycleRate) {
			texture = _texture;

			texturesize = texture.Width * texture.Height;

			textureData = new Color[texturesize];
			textureColorIndicies = new int[texturesize];

			texture.GetData(textureData);

			palettesize = 0;
			for (int i = 0; i < texturesize; i++) {
				palettesize = i;
				if (textureData[i] == terminateColor)
					break;
			}

			palette = new Color[palettesize];
			paletteBuffer = new Color[palettesize];
			for (int i = 0; i < palettesize; i++) {
				palette[i] = textureData[i];
			}


			for (int i = 0; i < texturesize; i++) {
				for(int j = 0; j < palettesize; j++) {
					if(textureData[i] == palette[j]) {
						textureColorIndicies[i] = j;
						break;
					}
				}
			}

			rate = cycleRate;
			cycleRegions = _cycleRegions;
		}


		float time = 0;
		float rate;


		public void Update(GameTime gameTime) {
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			for (int r = 0; r < cycleRegions.Length; r++) {
				CycleRegion region = cycleRegions[r];
				cycleRegions[r].time += elapsed;
				if (cycleRegions[r].time > rate / region.rateMultiplier) {
					int cycleStart = region.cycleStart;
					int cycleLength = region.cycleLength - cycleStart;
					cycleRegions[r].time = 0;
					palette.CopyTo(paletteBuffer, 0);

					for (int i = cycleStart; i < cycleStart + cycleLength; i++) {
						int newindex = Utility.wrapIndex(i - cycleStart - 1, cycleLength);
						palette[i] = paletteBuffer[newindex + cycleStart];
					}

					for (int i = 0; i < texturesize; i++) {
						textureData[i] = palette[textureColorIndicies[i]];
					}
					texture.SetData(textureData);
				}
			}

		}

		public void UpdateSingle(GameTime gameTime) {
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			while (time > rate) {
				time -= rate;

				palette.CopyTo(paletteBuffer, 0);

				for (int r = 0; r < cycleRegions.Length; r += 1) {
					int cycleStart = cycleRegions[r].cycleStart;
					int cycleLength = cycleRegions[r].cycleLength;
					for (int i = cycleStart; i < cycleStart + cycleLength; i++) {
						palette[i] = paletteBuffer[((i - cycleStart + 1) % cycleLength) + cycleStart];
					}
				}

				for (int i = 0; i < texturesize; i++) {
					textureData[i] = palette[textureColorIndicies[i]];
				}
				texture.SetData(textureData);
			}
		}


	}
}
