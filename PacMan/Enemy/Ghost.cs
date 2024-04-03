using Grid.Astar;
using Grid.Food;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Enemy
{
    public class Ghost : GameObjects
    {
        public Vector2 ghostPos;

        /// <summary>
        /// pacman hp
        /// </summary>
        public static int hp = 3;
        float invisTimer;
        
        float speed;

        /// <summary>
        /// AI
        /// </summary>
        protected List<Vector2> pathNodes = new List<Vector2>();
        protected Vector2 moveTo;

        bool deadGhost;

        float pathTimer;
        float spawnTimer;
        float eatTimer;
        bool eatable;
        Vector2 respawn;

        Rectangle ghostRec;
        
        public Ghost(Vector2 ghostPos, int numberOfFrames, float frameSpeed, bool looping, Texture2D tex, SquareGrid grid, float speed) : base(speed, tex, frameSpeed, numberOfFrames, looping)
        {
            this.ghostPos = ghostPos;


            respawn = new Vector2(450, 470);

            this.speed = speed;


            moveTo = new Vector2(Player.playerPos.X, Player.playerPos.Y);
            pathTimer = 2 * 60;
            eatTimer = 7 * 60;
            eatable = false;
            spawnTimer = 5 * 60;
            deadGhost = false;

            invisTimer = 5 * 60;
           
            
        }
        public void Update(GameTime gameTime, SquareGrid grid, Player player, FoodManager food)
        {
            invisTimer--;
            pathTimer--;
            ghostRec = new Rectangle((int)ghostPos.X, (int)ghostPos.Y, AssetManager.green.Width / 5, AssetManager.green.Height);
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            animationRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

           

            switch (currentGhostState)
            {
                case ghostState.eatable:
                    eatable = true;
                    
                    if (player.playerRec.Intersects(ghostRec))
                    {
                        
                        currentGhostState = ghostState.respawn;

                        FoodManager.score += 50;
                    }
                    else if (food.eatableGhost == false)
                    {

                        currentGhostState = ghostState.toPlayer;
                    }

                    
                    if (pathNodes == null || pathNodes.Count == 0 && ghostPos.X == respawn.X && ghostPos.Y == respawn.Y || pathTimer == 0)
                    {
                            pathNodes = FindPath(grid, grid.GetSlotFromPixel(respawn, new Vector2(0, 0)));

                            if (pathNodes.Count != 0)
                            {
                                moveTo = pathNodes[0];
                                pathNodes.RemoveAt(0);
                            }


                            pathTimer = 1 * 60;

                    }
                    else
                    {
                            MoveUnit(player);

                           
                    }
                    
                   
                    break;

                case ghostState.toPlayer:
                    
                    eatable = false;
                    deadGhost = false;
                    if (pathNodes == null || pathNodes.Count == 0 && ghostPos.X == moveTo.X && ghostPos.Y == moveTo.Y || pathTimer == 0)
                    {
                        pathNodes = FindPath(grid, grid.GetSlotFromPixel(Player.playerPos, new Vector2(0, 0)));

                        if (pathNodes.Count != 0)
                        {
                            moveTo = pathNodes[0];
                            pathNodes.RemoveAt(0);
                        }


                        pathTimer = 1 * 60;

                    }
                    
                    
                    else
                    {
                        MoveUnit(player);


                    }
                    if (food.eatableGhost == true)
                    {

                        currentGhostState = ghostState.eatable;

                    }

                    if (player.playerRec.Intersects(ghostRec) && invisTimer <= 0)
                    {
                        invisTimer = 5 * 60;
                        hp -= 1;



                    }

                    break;

                case ghostState.respawn:
                    eatable = false;
                    deadGhost = true;
                    if (ghostPos == respawn)
                    {
                        
                        currentGhostState = ghostState.toPlayer;

                    }
                    else
                    {
                        MoveUnit(player);
                    }
                    break;
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
        public void Draw(SpriteBatch spriteBatch)
        {
            if (deadGhost == false && eatable == false)
            {
                //spriteBatch.Draw(AssetManager.empty, ghostRec, Color.Red);

                spriteBatch.Draw(tex, ghostPos, animationRect, Color.White, 0, Vector2.Zero, 1f, spriteEffect, 1f);
            }
            else if(ghostPos != respawn && deadGhost == true)
            {
                spriteBatch.Draw(AssetManager.ghostEyes, ghostPos, Color.White);
            }

            if(eatable == true)
            {
                spriteBatch.Draw(AssetManager.ghostFear, ghostPos, animationRect, Color.White, 0, Vector2.Zero, 1f, spriteEffect, 1f);
            }
            
        }
        public virtual List<Vector2> FindPath(SquareGrid grid, Vector2 _endSlot)
        {
            pathNodes.Clear();

            Vector2 tempStartSlot = grid.GetSlotFromPixel(ghostPos, Vector2.Zero);

            List<Vector2> tempPath = grid.GetPath(tempStartSlot, _endSlot/*, true*/);

            if (tempPath == null && tempPath.Count == 0)
            {

            }

            return tempPath;
        }
        public virtual void MoveUnit(Player player)
        {
            if(deadGhost == false)
            {
                if (ghostPos.X != moveTo.X || ghostPos.Y != moveTo.Y)
                {
                    ghostPos += Movement.RadialMovement(moveTo, ghostPos, speed);
                }
                else if (pathNodes.Count > 0)
                {
                    moveTo = pathNodes[0];
                    pathNodes.RemoveAt(0);

                    ghostPos += Movement.RadialMovement(moveTo, ghostPos, speed);
                }
            }
            else if(deadGhost == true /*|| eatable == true*/)
            {
                if (ghostPos.X != respawn.X || ghostPos.Y != respawn.Y)
                {
                    ghostPos += Movement.RadialMovement(respawn, ghostPos, speed);
                }
                else if (pathNodes.Count > 0)
                {
                    respawn = pathNodes[0];
                    pathNodes.RemoveAt(0);

                    ghostPos += Movement.RadialMovement(respawn, ghostPos, speed);
                }
            }
            
        }
        public static void Lives(SpriteBatch spriteBatch)
        {
            if(hp == 3)
            {
                spriteBatch.Draw(AssetManager.lives, new Vector2(0, 850), Color.White);
                spriteBatch.Draw(AssetManager.lives, new Vector2(120, 850), Color.White);
                spriteBatch.Draw(AssetManager.lives, new Vector2(240, 850), Color.White);

                spriteBatch.Draw(AssetManager.trophy, new Vector2(400, 850), Color.White);
            }
            else if (hp == 2)
            {
                spriteBatch.Draw(AssetManager.lives, new Vector2(0, 850), Color.White);
                spriteBatch.Draw(AssetManager.lives, new Vector2(120, 850), Color.White);

                spriteBatch.Draw(AssetManager.trophy, new Vector2(400, 850), Color.White);

            }
            else if (hp == 1)
            {
                spriteBatch.Draw(AssetManager.lives, new Vector2(0, 850), Color.White);

                spriteBatch.Draw(AssetManager.trophy, new Vector2(400, 850), Color.White);

            }

        }
    }
}
