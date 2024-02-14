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
        
        //Random number setup
        static Random rng = new Random();
        
        
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

        Texture2D titleImg;
        Texture2D demoImg;//Ready instructions
        Texture2D blank; //blank white pixels
        Texture2D topPipe; //Top pipe
        Texture2D bottomPipe; //Bottom pipe
        
        //Button locations
        Rectangle menuRec;
        Rectangle startRec;
        Rectangle statsRec;


        Rectangle titleRec;
        Vector2 titleBirdPos;
        Rectangle demoRec;

        Vector2[] grndPos = new Vector2[2];
        
        Rectangle[] topPipeRec = new Rectangle[3];
        Rectangle[] bottomPipeRec = new Rectangle[3];
        Vector2[] topPipePos = new Vector2[3];
        Vector2[] bottomPipePos = new Vector2[3];
        
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
            titleImg = Content.Load<Texture2D>("Images/Sprites/Title");
            blank = Content.Load<Texture2D>("Images/Sprites/Fader");
            topPipe = Content.Load<Texture2D>("Images/Sprites/TopPipe");
            bottomPipe = Content.Load<Texture2D>("Images/Sprites/BottomPipe");
            

            

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
            birdAnim = new Animation(birdImg, 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 400, birdPos,
                1f, 1f, true);

            titleRec = new Rectangle(C.SCREEN_WIDTH / 2 - titleImg.Width / 2 - birdAnim.GetDestRec().Width,
                (C.SCREEN_HEIGHT - grndImg.Height) / 2 - titleImg.Height / 2, titleImg.Width, titleImg.Height);
            titleBirdPos = new Vector2(titleRec.X * 2 + titleImg.Width, (C.SCREEN_HEIGHT - grndImg.Height) / 2 - birdImg.Height / 2);

            /*for (int i = 0; i < topPipeRec.Length; i++)
            {
                topPipeRec[i] = new Rectangle(600 + ((200 + topPipe.Width) * i), -topPipe.Height + 400, topPipe.Width, topPipe.Height);
                bottomPipeRec[i] = new Rectangle(topPipeRec[i].X, topPipeRec[i].Y + topPipe.Height + 210, topPipe.Width, topPipe.Height);
                topPipePos[i] = new Vector2(topPipeRec[i].X, topPipeRec[i].Y);
                bottomPipePos[i] = new Vector2(bottomPipeRec[i].X, bottomPipeRec[i].Y);
            }*/
            Setup();
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

                    if (kb.IsKeyDown(Keys.Space) && !prevKb.IsKeyDown(Keys.Space))
                    {
                        birdAnim.TranslateTo(130, (C.SCREEN_HEIGHT - grndImg.Height) / 2 - birdImg.Height);
                        gameState = C.READY;
                    }
                        
                    Move();
                    break;
                case C.READY:
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed || kb.IsKeyDown(Keys.Space) && !prevKb.IsKeyDown(Keys.Space))
                    {
                        birdAnim.SetDuration(200);//Speed up animation
                        gameState = C.GAME;//Switch to the main game
                    }
                    Move();
                    break;
                case C.GAME:
                    Move();
                    if (birdAnim.GetDestRec().Y + birdImg.Height >= C.SCREEN_HEIGHT - grndImg.Height || birdAnim.GetDestRec().Y <= 0)
                    {
                        gameState = C.END;
                    }

                    for (int i = 0; i < 3; i++ )
                        if (birdAnim.GetDestRec().Intersects(topPipeRec[i]) ||
                            birdAnim.GetDestRec().Intersects(bottomPipeRec[i]))
                            gameState = C.END;
                    break;
                case C.END:
                    MenuBtn();
                    Move();
                    
                    break;
                
            }
            birdAnim.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code 
            _spriteBatch.Begin();
            _spriteBatch.Draw(bgImg,bgRec,Color.White); //Draw background
            //Draw pipes 
            for (int i = 0; i < 3; i++)
            {
                _spriteBatch.Draw(topPipe, topPipeRec[i],Color.White);
                _spriteBatch.Draw(bottomPipe, bottomPipeRec[i],Color.White);
            }
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
                    _spriteBatch.Draw(titleImg,titleRec,Color.White);
                    break;
                case C.READY:
                    _spriteBatch.Draw(demoImg,demoRec,Color.White);
                    break;
                case C.GAME:
                    birdAnim.Draw(_spriteBatch, Color.White);
                    
                    break;
                case C.END:
                    _spriteBatch.Draw(menuBtn,menuRec,Color.White);
                    
                    break;
                
            }
            Console.Clear();
            Console.WriteLine(gameState);
            //_spriteBatch.Draw(birdImg,birdRec,Color.White);
            birdAnim.Draw(_spriteBatch, Color.White);
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
                //Move bird
                birdSpeed += C.GRAVITY;
                birdSpeed = MathHelper.Clamp(birdSpeed, C.FLAPPOWER, C.MAX_SPEED);
                if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed || kb.IsKeyDown(Keys.Space) && !prevKb.IsKeyDown(Keys.Space))
                {
                    birdSpeed = C.FLAPPOWER;
                }
                birdAnim.Translate(0, birdSpeed);
                //Move pipes
                for (int i = 0; i < topPipeRec.Length; i++)
                {
                    topPipePos[i].X += C.SPEED * -1;
                    topPipeRec[i].X = (int)topPipePos[i].X;
                    bottomPipePos[i].X += C.SPEED * -1;
                    bottomPipeRec[i].X = (int)bottomPipePos[i].X;
                    RegenPipe();
                }
            }
            else if (gameState == C.END)
            {
                if (birdAnim.GetDestRec().Bottom >= C.SCREEN_HEIGHT - grndImg.Height)
                {
                    birdSpeed = 0;
                    birdAnim.TranslateTo(130, C.SCREEN_HEIGHT - grndImg.Height - birdImg.Height);
                }
                else
                {
                    birdSpeed += C.GRAVITY;
                    birdSpeed = MathHelper.Clamp(birdSpeed, C.FLAPPOWER, C.MAX_SPEED);
                    birdAnim.Translate(0, birdSpeed);
                }
                birdAnim.Pause();
                birdAnim.SetFrame(1);
                Console.WriteLine(birdAnim.GetDestRec());
            }
            
        }
           

        public void MenuBtn()
        {
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && birdSpeed == 0)
            {
                if (menuRec.Contains(mouse.Position))
                {
                    Setup();
                    gameState = C.MENU;
                }
            }

            if (kb.IsKeyDown(Keys.Space) && !prevKb.IsKeyDown(Keys.Space) && birdSpeed == 0)
            {
                Setup();
                gameState = C.MENU;
            }
        }
        
        //Generate new pipe
        public void RegenPipe()
        {
            for (int i = 0; i < 3; i++)
            {
                //Find the last pipe on the screen
                int lastPipePos;
                lastPipePos = i + 2;
                if (lastPipePos >= 3)
                    lastPipePos -= 3;
                
                //If a pipe leaves the screen then regenerate it 200 pixels after the last
                if (topPipeRec[i].Right <= 0)
                {
                        
                    topPipePos[i].X = topPipePos[lastPipePos].X + 200 + topPipe.Width;
                    topPipePos[i].Y = rng.Next(-topPipe.Height + 88, 0);
                    topPipeRec[i].X = (int)topPipePos[i].X;
                    topPipeRec[i].Y = (int)topPipePos[i].Y;
                    bottomPipePos[i].X = bottomPipePos[lastPipePos].X + 200 + topPipe.Width;
                    bottomPipePos[i].Y = topPipePos[i].Y + topPipe.Height + 210;
                    bottomPipeRec[i].X = (int)bottomPipePos[i].X;
                    bottomPipeRec[i].Y = (int)bottomPipePos[i].Y;
                }
            }
           
        }

        public void Setup()
        {
            //Reset bird
            birdAnim.TranslateTo((int)titleBirdPos.X,(int)titleBirdPos.Y);
            birdRec = birdAnim.GetDestRec();
            birdAnim.Resume();
            birdAnim.SetDuration(400);
            
            //Reset pipes
            for (int i = 0; i < topPipeRec.Length; i++)
            {
                //TODO: check out why it says 400px of the rightmost pipe
                topPipeRec[i] = new Rectangle(600 + ((200 + topPipe.Width) * i), rng.Next(-topPipe.Height + 88, 0), topPipe.Width, topPipe.Height);
                bottomPipeRec[i] = new Rectangle(topPipeRec[i].X, topPipeRec[i].Y + topPipe.Height + 210, topPipe.Width, topPipe.Height);
                topPipePos[i] = new Vector2(topPipeRec[i].X, topPipeRec[i].Y);
                bottomPipePos[i] = new Vector2(bottomPipeRec[i].X, bottomPipeRec[i].Y);
            }
        }


    }
}
