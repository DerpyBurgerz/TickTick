using Microsoft.Xna.Framework;
namespace Engine
{
	public class Camera
	{
		static Vector2 cameraOffset, cameraBorder;
		static float damping = 0.8f;
		public static Vector2 CameraOffset { get { return cameraOffset; } }
		static Vector2 desiredPosition = new Vector2(720, 600);
		static GameObject followedObject;
		static Rectangle levelBox;
		static Point worldSize;
		public static void Update()
		{
			//CameraBorder is a Vector2 with the max position for the camera.
			cameraBorder = new Vector2(levelBox.Width - worldSize.X, levelBox.Height - worldSize.Y);
			//Smoothstep makes the camera go smooth, and Clamp prevents it from going outside of the level.
			//The damping variable can be changed to change the smoothness of the camera.
			cameraOffset = Vector2.Clamp((Vector2.SmoothStep(followedObject.LocalPosition - desiredPosition, cameraOffset, damping)), Vector2.Zero, cameraBorder);
		}
		public static void SetCameraObject(GameObject gameObject, Rectangle LevelBox, Point WorldSize)
		{
			followedObject = gameObject;
			levelBox = LevelBox;
			worldSize = WorldSize;
		}
		public static void Reset()
		{
			followedObject = null;
			cameraOffset = Vector2.Zero;
		}
	}
}