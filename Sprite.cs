using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kitchen
{
	public class Sprite
	{
		private Texture2D texture;



		// we are updating the rectangle in the FixedUpdate() loop
		// but we also interpolate it's position
		Vector2 currentPosition;
		Vector2 previousPosition = Vector2.Zero;
		float movementSpeed;

		Vector2 velocity = Vector2.Zero;
		float friction = 0.7f;


		public Sprite(Texture2D texture, Vector2 initialPosition, float movementSpeed = 6.6f)
		{
			currentPosition = initialPosition;
			this.texture = texture;
			this.movementSpeed = movementSpeed;
		}


		KeyboardState keyboardState;
		private void MoveObject()
		{
			if (keyboardState.IsKeyDown(Keys.Left)) {
				velocity.X -= movementSpeed;
			}
			if (keyboardState.IsKeyDown(Keys.Right)) {
				velocity.X += movementSpeed;
			}
			if (keyboardState.IsKeyDown(Keys.Up)) {
				velocity.Y -= movementSpeed;
			}
			if (keyboardState.IsKeyDown(Keys.Down)) {
				velocity.Y += movementSpeed;
			}
		}

		public void Update()
		{
			
		}

		public void FixedUpdate()
		{
			keyboardState = Keyboard.GetState();
			previousPosition = currentPosition;
			MoveObject();
			currentPosition += velocity;
			//velocity *= friction;
			velocity = Vector2.Zero;
		}


		public void Draw(SpriteBatch spriteBatch, float ALPHA)
		{
			Vector2 drawposition = Vector2.Lerp(previousPosition, currentPosition, ALPHA);
			spriteBatch.Draw(texture, currentPosition, Color.White);
			spriteBatch.Draw(texture, currentPosition, Color.White);
		}
	}
}
