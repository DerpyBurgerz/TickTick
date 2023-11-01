using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
namespace Engine
{
	public class Camera
	{
		static Vector2 cameraOffset;
		public static Vector2 CameraOffset { get { return cameraOffset; } }
		
		public static void Update(Vector2 position)
		{
			cameraOffset.X = position.X - 100;
		}
	}
	
}
