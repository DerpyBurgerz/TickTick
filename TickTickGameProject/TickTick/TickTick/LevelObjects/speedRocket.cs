using System;
using Microsoft.Xna.Framework;
using Engine;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

class SpeedRocket : SpriteGameObject
{
    Level level;
    protected float bounce;
    Vector2 startPosition;
    static float speedRocketMultiplier;
    float elapsedTime;
    bool isPickedUp;
    public SpeedRocket(Level level, Vector2 startPosition) : base("Sprites/LevelObjects/spr_speedRocket", TickTick.Depth_LevelObjects)
    {
        this.level = level;
        this.startPosition = startPosition;
        speedRocketMultiplier = 1f;
        elapsedTime = 0;
        SetOriginToCenter();

        Reset();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + LocalPosition.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        localPosition.Y += bounce;

        // checks if the rocket is collected
        if (Visible && level.Player.CanCollideWithObjects && HasPixelPreciseCollision(level.Player))
        {
            Visible = false;
            isPickedUp = true;
        }

        if(isPickedUp)//when picked up, the multiplier is applied
        {
            speedRocketMultiplier = 2;
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;//the timer starts counting

            if (elapsedTime >= 3) //if statement to cancel with setting the isPickedUp bool to 0 
            { 
                speedRocketMultiplier = 1;
                isPickedUp = false;
                
            }
        }
       
 
        
    } 
    public override void Reset()
    {
        localPosition = startPosition;
        Visible = true;
        isPickedUp = false;
        elapsedTime = 0;
    }

    public static float SpeedRocketMuliplier()
    {
        return speedRocketMultiplier;
    }
}