using Grid.Astar;
using Grid.Enemy;
using Grid.Food;
using Grid.HighScore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Grid
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public int startTimer;
        bool pressed = false;

        Vector2 startPosText;
       
        FoodManager food = new FoodManager();
        /// <summary>
        /// Grid
        /// </summary>
        SquareGrid grid;
        Vector2 offset;

        public static Player player;

        Ghost green;
        Ghost pink;
        Ghost red;

        public static Tiles[,] tileArray;
        public static List<string> stringList = new List<string>();
        string srText;

        public enum GameState { start, inGame, end }
        public static GameState currentGameState = GameState.start;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 950;
            graphics.PreferredBackBufferHeight = 950;
        }

        protected override void Initialize()
        {            
            startTimer = 4 * 60;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            AssetManager.Load(Content);
            offset = new Vector2(0, 0);
            grid = new SquareGrid(new Vector2(32, 32), new Vector2(0,0), new Vector2(950, 950));
            AssetManager.normalEffect = Content.Load<Effect>("File");

            StreamReader sr = new StreamReader("Map.txt");
            srText = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                stringList.Add(sr.ReadLine());
            }
            sr.Close();

            tileArray = new Tiles[stringList[0].Length, stringList.Count];
            for (int i = 0; i < tileArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileArray.GetLength(1); j++)
                {
                    Vector2 tilePosition = new Vector2(AssetManager.empty.Width * i, AssetManager.empty.Height * j);

                    
                    if ((stringList[j][i] == '-'))
                    {
                        tileArray[i, j] = new Tiles(AssetManager.empty, tilePosition, false);
                        
                    }
                    else if (stringList[j][i] == 'f')
                    {
                        tileArray[i, j] = new Tiles(AssetManager.empty, tilePosition, false);
                        food.LoadFood("f", tilePosition);
                    }
                    else if (stringList[j][i] == 'b')
                    {
                        tileArray[i, j] = new Tiles(AssetManager.empty, tilePosition, false);
                        food.LoadFood("b", new Vector2(AssetManager.empty.Width * i - 10, AssetManager.empty.Height * j - 10));
                    }
                    else if (stringList[j][i] == 'p')
                    {
                        tileArray[i, j] = new Tiles(AssetManager.empty, tilePosition, false);
                        pink = new Ghost(tilePosition, 5, 100f, true, AssetManager.pink, grid, 1.2f);
                       
                    }
                    else if (stringList[j][i] == 'g')
                    {
                        green = new Ghost(tilePosition, 5, 100f, true, AssetManager.green, grid, 1.4f);
                        tileArray[i, j] = new Tiles(AssetManager.empty, tilePosition, false);
                    }
                   
                    else if (stringList[j][i] == 'r')
                    {
                        red = new Ghost(tilePosition, 5, 100f, true, AssetManager.red, grid, 1.6f);
                        tileArray[i, j] = new Tiles(AssetManager.empty, tilePosition, false);
                    }
                    else if(stringList[j][i] == 'w')
                    {
                        Vector2 tempLoc = grid.GetSlotFromPixel(new Vector2(AssetManager.empty.Width * i, AssetManager.empty.Height * j), Vector2.Zero);
                        GridLocation location = grid.GetSlotFromLocation(tempLoc);
                       
                        if (location != null && !location.filled && !location.impassable)
                        {
                            location.SetToFilled(false);

                            tileArray[i, j] = new Tiles(AssetManager.wall, tilePosition, true);

                        }
                    }
                    else if(stringList[j][i] == 'u')
                    {
                        tileArray[i, j] = new Tiles(AssetManager.empty, tilePosition, false);
                        player = new Player(tilePosition, 100f, AssetManager.pacman, 100f, 3, true);
                    }                                                           
                }
            }           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(grid != null)
            {
                grid.Update(offset);
            }
            
            if(Keyboard.GetState().IsKeyDown(Keys.G))
            {
                grid.showGrid = !grid.showGrid;
            }

            food.Update(gameTime, player);
               
            switch(currentGameState)
            {
                case GameState.start:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        pressed = true;

                    }
                    else if (pressed == true)
                    {
                        startTimer -= 1;

                    }
                    if (startTimer <= 0)
                    {
                        currentGameState = GameState.inGame;
                    }
                    break;

                case GameState.inGame:
                   
                   if(Ghost.hp <= 0)
                   {
                        currentGameState = GameState.end;
                   }

                 player.Update(gameTime, grid);

                 //ghost
                 green.Update(gameTime, grid, player, food);
                 red.Update(gameTime, grid, player, food);
                 pink.Update(gameTime, grid, player, food);
 
                    break;

                case GameState.end:
                    break;
                   
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(48,48,94));
            spriteBatch.Begin(SpriteSortMode.Immediate);
            grid.DrawGrid(offset, spriteBatch);

            switch (currentGameState)
            {
                case GameState.start:
                    for (int i = 0; i < stringList.Count; i++)
                    {
                        for (int j = 0; j < stringList[i].Length; j++)
                        {
                            Vector2 drawTilePosStart = new Vector2(32 * j, 32 * i);
                            if (stringList[i][j] == 'w')
                            {
                                spriteBatch.Draw(AssetManager.wall, drawTilePosStart, Color.White);
                            }
                            else if(stringList[i][j] == 'u')
                            {
                                spriteBatch.Draw(AssetManager.pacmanStart, drawTilePosStart, Color.White);
                            }
                            else if(stringList[i][j] == 'g')
                            {
                                spriteBatch.Draw(AssetManager.greenStart, drawTilePosStart, Color.White);
                            }
                            else if (stringList[i][j] == 'r')
                            {
                                spriteBatch.Draw(AssetManager.redStart, drawTilePosStart, Color.White);
                            }
                            else if (stringList[i][j] == 'p')
                            {
                                spriteBatch.Draw(AssetManager.pinkStart, drawTilePosStart, Color.White);
                            }
                        }
                    }
                    startPosText = new Vector2(350, 350);
                    food.Draw(spriteBatch);
                    if(startTimer == 4 * 60)
                    {
                        spriteBatch.Draw(AssetManager.startScreen, new Vector2(330, 200), Color.White);
                        spriteBatch.Draw(AssetManager.spaceStart, new Vector2(330, 500), Color.White);
                    }
                    else if (startTimer >= 3 * 60 && startTimer < 4 * 60)
                    {
                        spriteBatch.Draw(AssetManager.ready, startPosText, Color.White);
                    }
                    else if(startTimer >= 2 * 60 && startTimer < 4 * 60)
                    {
                        spriteBatch.Draw(AssetManager.set, startPosText, Color.White);
                    }
                    else if(startTimer >= 1 * 60 && startTimer < 4 * 60)
                    {
                        spriteBatch.Draw(AssetManager.go, startPosText, Color.White);
                    }
                   
                    break;

                case GameState.inGame:

                    for (int i = 0; i < stringList.Count; i++)
                    {
                        for (int j = 0; j < stringList[i].Length; j++)
                        {
                            Vector2 drawTilePosStart = new Vector2(32 * j, 32 * i);
                            if (stringList[i][j] == 'w')
                            {
                                spriteBatch.Draw(AssetManager.wall, drawTilePosStart, Color.White);
                            }

                            if(startTimer > 0)
                            {
                                if (stringList[i][j] == 'u')
                                {
                                    spriteBatch.Draw(AssetManager.pacmanStart, drawTilePosStart, Color.White);
                                }
                                else if (stringList[i][j] == 'g')
                                {
                                    spriteBatch.Draw(AssetManager.greenStart, drawTilePosStart, Color.White);
                                }
                                else if (stringList[i][j] == 'r')
                                {
                                    spriteBatch.Draw(AssetManager.redStart, drawTilePosStart, Color.White);
                                }
                                else if (stringList[i][j] == 'p')
                                {
                                    spriteBatch.Draw(AssetManager.pinkStart, drawTilePosStart, Color.White);
                                }

                                food.Draw(spriteBatch);
                            }
                            
                        }
                    }
                    if (startTimer <= 1 * 60)
                    {
                        player.Draw(spriteBatch);
                        green.Draw(spriteBatch);
                        red.Draw(spriteBatch);
                        pink.Draw(spriteBatch);
                        food.Draw(spriteBatch);
                    }
                    Ghost.Lives(spriteBatch);
                    spriteBatch.DrawString(AssetManager.font, ": " + FoodManager.score, new Vector2(500, 900), Color.Red);
                    
                    break;

                case GameState.end:
                    spriteBatch.Draw(AssetManager.startScreen, new Vector2(330, 200), Color.White);
                    spriteBatch.Draw(AssetManager.trophy, new Vector2(380, 550), Color.White);
                    spriteBatch.DrawString(AssetManager.font, ": " + FoodManager.score, new Vector2(470, 600), Color.Red);
                    break;

                
            }

            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public static bool GetTileAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / 32, (int)vec.Y / 32].wall;
        }
       
    }
}