using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Animation2D;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Animation = GameUtility.Animation;

namespace Flappy
{

    public class Game1 : Game
    {
        
        //Mouse and keyboard states
        KeyboardState kb;
        KeyboardState prevKb;
        MouseState mouse;
        MouseState prevMouse;
        
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D birdImg;

        Rectangle birdRec;
        Vector2 birdPos;
        Animation birdAnim;
        float birdSpeed = C.SPEED;
        
        //Backgrounds
        Texture2D bgImg;
        Texture2D grndImg;

        Rectangle bgRec;
        Rectangle[] grndRec = new Rectangle[2];
        
        //Sprites
        //Button images
        Texture2D menuBtn;
        Texture2D startBtn;
        Texture2D statsBtn;

        Texture2D demoImg;//Ready instructions
        Texture2D blank; //blank white pixels
        
        //Button locations
        Rectangle menuRec;
        Rectangle startRec;
        Rectangle statsRec;

        Rectangle demoRec;
        

        Vector2[] grndPos = new Vector2[2];
        
        int gameState = 1; //Store gamestate

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            //Set screen resolution from constants.cs
            _graphics.PreferredBackBufferWidth = C.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = C.SCREEN_HEIGHT;

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //Load background images
            bgImg = Content.Load<Texture2D>("Images/Backgrounds/Background");
            grndImg = Content.Load<Texture2D>("Images/Backgrounds/Ground");
            
            //Set background position
            bgRec = new Rectangle(0, 0, C.SCREEN_WIDTH, C.SCREEN_HEIGHT);
            grndRec[0] = new Rectangle(0, C.SCREEN_HEIGHT - grndImg.Height, grndImg.Width, grndImg.Height);
            grndRec[1] = new Rectangle(grndImg.Width, C.SCREEN_HEIGHT - grndImg.Height, grndImg.Width, grndImg.Height);
            
            grndPos[0] = new Vector2(grndRec[0].X, grndRec[0].Y);
            grndPos[1] = new Vector2(grndRec[1].X, grndRec[1].Y);
            
            //Load sprites
            //Menu buttons
            menuBtn = Content.Load<Texture2D>("Images/Sprites/MenuBtn");
            startBtn = Content.Load<Texture2D>("Images/Sprites/StartBtn");
            statsBtn = Content.Load<Texture2D>("Images/Sprites/StatsBtn");
            
            //Load sprite images
            birdImg = Content.Load<Texture2D>("Images/Sprites/Bird");
            demoImg = Content.Load<Texture2D>("Images/Sprites/ClickInstruction");
            blank = Content.Load<Texture2D>("Images/Sprites/Fader");

            //birdAnim = new Animation(birdImg, 0, 4, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 400, birdPos,
                //1f, 1f, true);

            //Set button position
            menuRec = new Rectangle((C.SCREEN_WIDTH / 2) - (menuBtn.Width / 2), (int)(C.SCREEN_HEIGHT * 0.70F), menuBtn.Width,
                menuBtn.Height);
            startRec = new Rectangle((C.SCREEN_WIDTH / 3) - (startBtn.Width / 2), (int)(C.SCREEN_HEIGHT * 0.70F),
                startBtn.Width, startBtn.Height);
            statsRec = new Rectangle((C.SCREEN_WIDTH / 3) * 2 - (statsBtn.Width / 2), (int)(C.SCREEN_HEIGHT * 0.70F),
                statsBtn.Width, startBtn.Height);
            
            //Sprite positions
            demoRec = new Rectangle(C.SCREEN_WIDTH / 2 - demoImg.Width / 2, C.SCREEN_HEIGHT / 2 - demoImg.Height / 2,
                demoImg.Width, demoImg.Height);
            
            birdRec = new Rectangle(130, (C.SCREEN_HEIGHT - grndImg.Height) / 2 - birdImg.Height, birdImg.Height, birdImg.Height);
            birdPos = new Vector2(birdRec.X, birdRec.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            //Update mouse and keyboard
            prevKb = kb;
            prevMouse = mouse;
            kb = Keyboard.GetState();
            mouse = Mouse.GetState();

            
            
            switch (gameState)
            {
                case C.STATS:
                    MenuBtn();
                    Move();
                    break;
                case C.MENU:
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        if (startRec.Contains(mouse.Position))
                            gameState = C.READY;
                        if (statsRec.Contains(mouse.Position))
                            gameState = C.STATS;
                    }
                    Move();
                    break;
                case C.READY:
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed || kb.IsKeyDown(Keys.Space) && !prevKb.IsKeyDown(Keys.Space))
                    {
                        gameState = C.GAME;
                    }
                    Move();
                    break;
                case C.GAME:
                    MenuBtn();
                    Move();
                    break;
                case C.END:
                    MenuBtn();
                    
                    break;
                
            }
            //birdAnim.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code 
            _spriteBatch.Begin();
            _spriteBatch.Draw(bgImg,bgRec,Color.White); //Draw background
            _spriteBatch.Draw(grndImg,grndRec[0],Color.White); //Draw ground
            _spriteBatch.Draw(grndImg,grndRec[1],Color.White); //Draw ground

            
            switch (gameState)
            {
                case C.STATS:
                    _spriteBatch.Draw(menuBtn,menuRec,Color.White);
                    break;
                case C.MENU:
                    _spriteBatch.Draw(startBtn,startRec,Color.White);
                    _spriteBatch.Draw(statsBtn,statsRec,Color.White);
                    break;
                case C.READY:
                    _spriteBatch.Draw(demoImg,demoRec,Color.White);
                    break;
                case C.GAME:
                    //DEBUG
                    _spriteBatch.Draw(menuBtn,menuRec,Color.White);
                    break;
                case C.END:
                    _spriteBatch.Draw(menuBtn,menuRec,Color.White);
                    break;
                
            }
            Console.Clear();
            Console.WriteLine(gameState);
            _spriteBatch.Draw(birdImg,birdRec,Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Move()
        {
            if (gameState != C.END)
            {
                //Move ground
                for (int i = 0; i < grndRec.Length; i++)
                {
                    grndPos[i].X += C.SPEED * -1;
                    if (grndPos[i].X + (float)grndImg.Width <= 0)
                    {
                        grndPos[i].X = grndImg.Width;
                        grndRec[i].X = (int)grndPos[i].X;
                    }
                    else
                    {
                        grndRec[i].X = (int)grndPos[i].X;
                    }
                }
                
            }

            if (gameState == C.GAME)
            {
                birdSpeed += C.GRAVITY;

                MathHelper.Clamp(birdSpeed, C.FLAPPOWER, C.MAX_SPEED);
                
                
                if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed || kb.IsKeyDown(Keys.Space) && !prevKb.IsKeyDown(Keys.Space))
                {
                    birdSpeed = C.FLAPPOWER;
                }
                birdPos.Y += birdSpeed; 
                birdRec.Y = (int)birdPos.Y;
                
            }
        }

        public void MenuBtn()
        {
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                if (menuRec.Contains(mouse.Position))
                    gameState = C.MENU;
            }
        }
        
    }
}
