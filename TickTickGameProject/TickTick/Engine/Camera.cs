using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
namespace Engine
{
	public class Camera
	{
		static Vector2 cameraOffset;
		public static Vector2 CameraOffset { get { return cameraOffset; } }
		static Vector2 playerPosition = new Vector2(500, 400);//desired Position of the player on the screen.
		public static void Update(Vector2 positionObject, Rectangle levelBox, Point worldSize)
		{
			cameraOffset.Y = MathHelper.Clamp(positionObject.Y - playerPosition.Y, 0, levelBox.Height - worldSize.Y);
			cameraOffset.X = MathHelper.Clamp(positionObject.X - playerPosition.X, 0, levelBox.Width - worldSize.X);
		}
	}
}
