using Grid.Astar;
using Grid.Enemy;
using Grid.Food;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace Grid
{
    public class Player : GameObjects
    {
        
        bool moving = false;
        float rotation;
        public static Vector2 playerPos;
        public Rectangle playerRec;
        float wakaTimer;

        public Player(Vector2 playerPos, float speed, Texture2D tex, float frameSpeed, int numberOfFrames, bool looping) : base(speed, tex, frameSpeed, numberOfFrames, looping)
        {
            this.speed = speed;
            Player.playerPos = playerPos;

           rotation = 0;
            wakaTimer = 1f * 60;
        }
        public void Update(GameTime gameTime, SquareGrid grid)
        {
            playerRec = new Rectangle((int)playerPos.X, (int)playerPos.Y, AssetManager.pacman.Width / 3, AssetManager.pacman.Height);
            elapsedTime += (float)gameTime.ElapsedGameTime.Milliseconds;
            animationRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);


            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
               
                currentMoveState = MoveState.movingLeft;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                
                currentMoveState = MoveState.movingRight;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                
                currentMoveState = MoveState.movingUp;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                
                currentMoveState = MoveState.movingDown;
            }

            if (!moving)
            {
                switch (currentMoveState)
                {
                    case MoveState.movingLeft:
                        rotation = MathHelper.ToRadians(0);
                        ChangeDirection(new Vector2(-1, 0));

                    spriteEffect = SpriteEffects.FlipHorizontally;

                        break;

                    case MoveState.movingRight:
                        rotation = MathHelper.ToRadians(0);
                        ChangeDirection(new Vector2(1, 0));

                    spriteEffect = SpriteEffects.None;

                        break;

                    case MoveState.movingUp:
                        rotation = MathHelper.ToRadians(-90);
                        ChangeDirection(new Vector2(0, -1));

                        spriteEffect = SpriteEffects.None;
                        break;

                    case MoveState.movingDown:
                        spriteEffect = SpriteEffects.FlipHorizontally;
                        rotation = MathHelper.ToRadians(-90);
                        ChangeDirection(new Vector2(0,1));

                        break;
                }
            }
            else
            {
                playerPos += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Check if we are near enough to the destination
                if (Vector2.Distance(playerPos, destination) < 1)
                {
                    playerPos = destination;
                    moving = false;
                }
            }

            if (moving == true)
            {
                wakaTimer--;
                if(wakaTimer <= 0)
                {
                    AssetManager.waka.Play();
                    wakaTimer = 0.9f * 60;
                }
                if (elapsedTime >= frameTime)
                {
                    if (currentFrame >= numberOfFrames - 1)
                    {
                        currentFrame = 0;

                    }
                    else
                    {
                        currentFrame++;
                    }
                    elapsedTime = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(AssetManager.empty, playerRec, Color.Red);

            spriteBatch.Draw(tex, playerPos + new Vector2(16, 16), animationRect, Color.White, rotation, new Vector2(16, 16), 1, spriteEffect, 1);
        }
        public void ChangeDirection(Vector2 dir)
        {
            direction = dir;
            Vector2 newDestination = playerPos + direction * 32.0f;

            //Check if we can move in the desired direction, if not, do nothing
            if (!Game1.GetTileAtPosition(newDestination))
            {
                destination = newDestination;
                moving = true;
            }
        }
        
    }
}
