﻿using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

partial class Level : GameObjectList
{
    public const int TileWidth = 72;
    public const int TileHeight = 55;

    Tile[,] tiles;
    List<WaterDrop> waterDrops;

    public Player Player { get; private set; }
    public int LevelIndex { get; private set; }

    SpriteGameObject goal;
    BombTimer timer;

    static public int seconds;

    bool completionDetected;

    public Level(int levelIndex, string filename)
    {
        LevelIndex = levelIndex;

        // load the background
        GameObjectList backgrounds = new GameObjectList();
        for (int i = 0; i < 3 ; i++)
        {
			for (int j = 0; j < 3; j++)
			{
				SpriteGameObject backgroundSky = new SpriteGameObject("Sprites/Backgrounds/spr_sky", TickTick.Depth_Background);
				backgroundSky.LocalPosition = new Vector2((1 - i) * backgroundSky.Width, 825 - backgroundSky.Height + (j*backgroundSky.Height));
				backgrounds.AddChild(backgroundSky);
			}
		}

        AddChild(backgrounds);

        // load the rest of the level
        LoadLevelFromFile(filename);

        // add the timer
        timer = new BombTimer();
        AddChild(timer);

        // add mountains in the background
        for (int i = 0;; i++)//This creates an infinite loop with int i which increments each loop.
        {
            SpriteGameObject mountain = new SpriteGameObject(
                "Sprites/Backgrounds/spr_mountain_" + (ExtendedGame.Random.Next(2) + 1),
                TickTick.Depth_Background + 0.01f * (float)ExtendedGame.Random.NextDouble(), parralaxFactor: 0.3f);

            mountain.LocalPosition = new Vector2(mountain.Width * (i-4) * 0.4f, 
                BoundingBox.Height - mountain.Height);

            backgrounds.AddChild(mountain);
            //When the drawn mountain is farther right than the screen is, the loop breaks.
            if ((i-4) * (mountain.Width*0.4) > BoundingBox.Width)
                break;
        }

        // add clouds
        for (int i = 0; i < 6; i++)
            backgrounds.AddChild(new Cloud(this));
    }

    public Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(0, 0,
                tiles.GetLength(0) * TileWidth,
                tiles.GetLength(1) * TileHeight);
        }
    }

    public BombTimer Timer { get { return timer; } }

    //method to make seconds accessible in BombTimer.cs
    static public int TimeLeft()
    {
        return seconds;
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(x * TileWidth, y * TileHeight);
    }

    public Point GetTileCoordinates(Vector2 position)
    {
        return new Point((int)Math.Floor(position.X / TileWidth), (int)Math.Floor(position.Y / TileHeight));
    }

    public Tile.Type GetTileType(int x, int y)
    {
        // If the x-coordinate is out of range, treat the coordinates as a wall tile.
        // This will prevent the character from walking outside the level.
        if (x < 0 || x >= tiles.GetLength(0))
            return Tile.Type.Wall;

        // If the y-coordinate is out of range, treat the coordinates as an empty tile.
        // This will allow the character to still make a full jump near the top of the level.
        if (y < 0 || y >= tiles.GetLength(1))
            return Tile.Type.Empty;

        return tiles[x, y].TileType;
    }

    public Tile.SurfaceType GetSurfaceType(int x, int y)
    {
        // If the tile with these coordinates doesn't exist, return the normal surface type.
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
            return Tile.SurfaceType.Normal;

        // Otherwise, return the actual surface type of the tile.
        return tiles[x, y].Surface;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // check if we've finished the level
        if (!completionDetected && AllDropsCollected && Player.HasPixelPreciseCollision(goal))
        {
            completionDetected = true;
            ExtendedGameWithLevels.GetPlayingState().LevelCompleted(LevelIndex);
            Player.Celebrate();

            // stop the timer
            timer.Running = false;
        }

        // check if the timer has passed
        else if (Player.IsAlive && timer.HasPassed)
        {
            Player.Explode();
        }

		Camera.Update();
	}

    /// <summary>
    /// Checks and returns whether the player has collected all water drops in this level.
    /// </summary>
    bool AllDropsCollected
    {
        get
        {
            foreach (WaterDrop drop in waterDrops)
                if (drop.Visible)
                    return false;
            return true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        completionDetected = false;
    }
}

