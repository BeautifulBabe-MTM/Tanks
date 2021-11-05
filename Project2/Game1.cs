using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TanksLib;

namespace Client_Tank
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D wall;
        Texture2D tank_texture;
        Tank tank = new Tank(300, 5, 30);
        Vector2 position = Vector2.Zero;


        public static class Map
        {
            public static char[,] IntMap { set; get; }
            public static Wall[,] WallMap { set; get; }


            static Map()
            {
                IntMap = new char[20, 20]{
                    {'X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X'},
                    {'X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X'},
                    {'X','X','X',' ',' ',' ',' ',' ',' ','X','X',' ',' ',' ',' ',' ',' ','X','X','X'},
                    {'X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X'},
                    {'X',' ',' ',' ',' ','X',' ',' ',' ',' ',' ',' ',' ',' ','X',' ',' ',' ',' ','X'},
                    {'X',' ',' ',' ',' ','X',' ',' ',' ',' ',' ',' ',' ',' ','X',' ',' ',' ',' ','X'},
                    {'X',' ',' ',' ',' ','X','X','X','X',' ',' ','X','X','X','X',' ',' ',' ',' ','X'},
                    {'X',' ',' ','X','X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X','X',' ',' ','X'},
                    {'X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X'},
                    {'X',' ',' ',' ','X',' ',' ','X','X',' ',' ','X','X',' ',' ','X',' ',' ',' ','X'},
                    {'X',' ',' ',' ','X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X',' ',' ',' ','X'},
                    {'X',' ',' ',' ','X',' ',' ','X','X',' ',' ','X','X',' ',' ','X',' ',' ',' ','X'},
                    {'X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X'},
                    {'X',' ',' ','X','X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X','X',' ',' ','X'},
                    {'X',' ',' ',' ',' ','X','X','X','X',' ',' ','X','X','X','X',' ',' ',' ',' ','X'},
                    {'X',' ',' ',' ',' ','X',' ',' ',' ',' ',' ',' ',' ',' ','X',' ',' ',' ',' ','X'},
                    {'X',' ',' ',' ',' ','X',' ',' ',' ',' ',' ',' ',' ',' ','X',' ',' ',' ',' ','X'},
                    {'X','X','X',' ',' ',' ',' ',' ',' ','X','X',' ',' ',' ',' ',' ',' ','X','X','X'},
                    {'X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X'},
                    {'X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X'},
                };
                WallMap = new Wall[20, 20];
                for (int i = 0; i < IntMap.GetLength(0); i++)
                {
                    for (int j = 0; j < IntMap.GetLength(1); j++)
                    {
                        WallMap[i, j] = new Wall(new Rectangle(j * 50, i * 50, 50, 50), IntMap[i, j] == 'X' ? true : false);
                    }
                }
            }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Client client = new Client("127.0.0.1", 8000);
            client.CreateIPEndPoint();
            client.Connect();
            client.SengMsg("Conection");
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            tank_texture = Content.Load<Texture2D>(@"Texture\tank");
            wall = Content.Load<Texture2D>(@"Texture\wall");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                tank.X -= tank.Speed;
                tank.Rotation = 23.55f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                tank.X += tank.Speed;
                tank.Rotation = 7.85f;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                tank.Y -= tank.Speed;
                tank.Rotation = 0f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                tank.Y += tank.Speed;
                tank.Rotation = 15.7f;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(tank_texture, new Rectangle(tank.X, tank.Y, 40, 49), null, Color.White, tank.Rotation, new Vector2(40 / 2f, 49 / 2f), SpriteEffects.None, 0f);
            drawWalls();
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        private void drawWalls()
        {
            for (int i = 0; i < Map.IntMap.GetLength(0); i++)
            {
                for (int j = 0; j < Map.IntMap.GetLength(1); j++)
                {
                    if (Map.WallMap[i, j].IsActive == true)
                        _spriteBatch.Draw(wall, new Vector2(Map.WallMap[i, j].rec.X, Map.WallMap[i, j].rec.Y), Color.Bisque);
                }
            }
        }
    }
}