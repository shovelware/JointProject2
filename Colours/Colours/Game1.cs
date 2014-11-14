using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Colours
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D tex_player,
                  tex_enemy_tiny, tex_enemy_small, tex_enemy_medium, tex_enemy_large, tex_enemy_huge, tex_enemy_xbox,
                  tex_wep_none, tex_hair_centre, tex_hair_ver, tex_hair_hor,
                  tex_col_wht, tex_col_red, tex_col_grn, tex_col_blu, tex_col_cyn, tex_col_mgn, tex_col_ylo, tex_col_blk, tex_col_gry,
                  tex_trans_wht, tex_trans_red, tex_trans_grn, tex_trans_blu, tex_trans_cyn, tex_trans_mgn, tex_trans_ylo, tex_trans_blk, tex_trans_gry,
                  tex_wep_a, tex_wep_a_f, tex_wep_a_i, tex_wep_a_e, tex_wep_a_x, tex_wep_a_f1,
                  tex_wep_b, tex_wep_b_f, tex_wep_b_i, tex_wep_b_e, tex_wep_b_x, tex_wep_b_x_i, tex_wep_b_x_i1, tex_wep_b_x_i2,
                  tex_wep_c, tex_wep_c_f, tex_wep_c_i, tex_wep_c_e, tex_wep_c_x, 
                  tex_wep_d, tex_wep_d_f, tex_wep_d_i, tex_wep_d_e, tex_wep_d_x, 
                  tex_hair_a, tex_hair_a1,
                  tex_hair_b,
                  tex_hair_c,
                  tex_hair_d,
                  tex_splash, tex_menubuttons, tex_pause, tex_pause_over, tex_howto, tex_scorebg, tex_gameover;

        SoundEffect snd_atatat, snd_spawn, snd_death, snd_explode, snd_bgm, snd_hit;
        SoundEffectInstance snd_explode_instance, snd_bgm_instance;

        bool music = true;
        bool sfx = true;

        SpriteFont font;

        List<Enemy> enemyList = new List<Enemy>();//Enemy list
        List<Bullet> bulletList = new List<Bullet>();//Projectile List
        Player[] hiscores = new Player[5] {  new Player (896, "Hitagi"), new Player(1024, "Suruga"), new Player(512, "Hanekawa"), new Player (880, "Araragi"), new Player(120, "Sengoku" )}; // used to store high scores

        String currentFile = "Hiscores.txt";

        Color grm = new Color(127, 127, 127, 255);

        UI ui1;
        Player player1;
        string newname = "";

        Weapon[] weapons = new Weapon[5];
        Texture2D[] enemytex = new Texture2D[6];

        MouseState mouseStatePrev, mouseState;
        KeyboardState keyStatePrev, keyState;

        Keys[] oldKeys = new Keys[0];

        const int screenwidth = 640;
        const int screenheight = 896;

        Vector2 midpoint;
        Vector2 mousePos;
        Vector2 mousePosPrev;
        Vector2 mouseLeftPrev;
        Vector2 mouseRightPrev;
        bool select;

        const float SPAWNTIMERPREMAX = 5;
        float spawntimerpremax = 5;
        float spawntimer;
        bool spawn = true;
        
        const byte SPLASH = 0, MENU = 1, GAME = 2, PAUSE = 3, GAMEOVER = 4;

        byte gameMode = SPLASH;
        int lasthit = -1;
        int pausetimer;

        bool drawScores;
        bool drawHow;
        bool takename;
        bool debug = false;
        bool debugmsg = true;

        int saved;
        int loaded;
        bool excepio;
        bool excepfnf;
        FileNotFoundException excepfnfproblem;


        Rectangle menu1, menu2, menu3, menu4, noticeRect;
        Rectangle rectRed, rectGreen, rectBlue, rectCyan, rectMagenta, rectYellow, rectClear, rectBlack, rectWhite;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = screenheight;
            graphics.PreferredBackBufferWidth = screenwidth;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            this.IsMouseVisible = false;
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("squarefont");

            tex_player = Content.Load<Texture2D>("PlayerEdgeFill");
            tex_wep_none = Content.Load<Texture2D>("Wep_None");

            tex_enemy_tiny = Content.Load<Texture2D>("Enemy_Tiny");
            tex_enemy_small = Content.Load<Texture2D>("Enemy_Small");
            tex_enemy_medium = Content.Load<Texture2D>("Enemy_Medium");
            tex_enemy_large = Content.Load<Texture2D>("Enemy_Large");
            tex_enemy_huge = Content.Load<Texture2D>("Enemy_Huge");
            tex_enemy_xbox = Content.Load<Texture2D>("Enemy_Xbox");

            tex_wep_a = Content.Load<Texture2D>("Wep_Atatat");
            tex_wep_a_f = Content.Load<Texture2D>("Wep_Atatat_Fire");
            tex_wep_a_i = Content.Load<Texture2D>("Wep_Atatat_Impact");
            //tex_wep_a_e = Content.Load<Texture2D>("Wep_Atatat_Effect");
            tex_wep_a_x = Content.Load<Texture2D>("Wep_Atatat_X");
            tex_wep_a_f1 = Content.Load<Texture2D>("Wep_Atatat_Fire1");

            tex_wep_b = Content.Load<Texture2D>("Wep_Boom");
            tex_wep_b_f = Content.Load<Texture2D>("Wep_Boom_Fire");
            //tex_wep_b_i = Content.Load<Texture2D>("Wep_Boom_Impact");
            //tex_wep_b_e = Content.Load<Texture2D>("Wep_Boom_Effect");
            tex_wep_b_x = Content.Load<Texture2D>("Wep_Boom_X");
            tex_wep_b_x_i = Content.Load<Texture2D>("Wep_Boom_X_I");
            tex_wep_b_x_i1 = Content.Load<Texture2D>("Wep_Boom_X_I1");
            tex_wep_b_x_i2 = Content.Load<Texture2D>("Wep_Boom_X_I2");

            tex_wep_c = Content.Load<Texture2D>("Wep_Coil");
            tex_wep_c_f = Content.Load<Texture2D>("Wep_Coil_Fire");
            //tex_wep_c_i = Content.Load<Texture2D>("Wep_Coil_Impact");
            //tex_wep_c_e = Content.Load<Texture2D>("Wep_Coil_Effect");
            tex_wep_c_x = Content.Load<Texture2D>("Wep_Coil_X");

            tex_wep_d = Content.Load<Texture2D>("Wep_Depleted");
            tex_wep_d_f = Content.Load<Texture2D>("Wep_Depleted_Fire");
            tex_wep_d_i = Content.Load<Texture2D>("Wep_Depleted_Impact");
            tex_wep_d_e = Content.Load<Texture2D>("Wep_Depleted_Effect");
            tex_wep_d_x = Content.Load<Texture2D>("Wep_Depleted_X");

            tex_hair_centre = Content.Load<Texture2D>("X_Centre");
            tex_hair_hor = Content.Load<Texture2D>("X_Hor");
            tex_hair_ver = Content.Load<Texture2D>("X_Ver");

            tex_col_wht = Content.Load<Texture2D>("Col_White");
            tex_col_red = Content.Load<Texture2D>("Col_Red");
            tex_col_grn = Content.Load<Texture2D>("Col_Green");
            tex_col_blu = Content.Load<Texture2D>("Col_Blue");
            tex_col_cyn = Content.Load<Texture2D>("Col_Cyan");
            tex_col_ylo = Content.Load<Texture2D>("Col_Yellow");
            tex_col_mgn = Content.Load<Texture2D>("Col_Magenta");
            tex_col_blk = Content.Load<Texture2D>("Col_Black");
            tex_col_gry = Content.Load<Texture2D>("Col_Grey");

            tex_trans_wht = Content.Load<Texture2D>("Trans_White");
            tex_trans_red = Content.Load<Texture2D>("Trans_Red");
            tex_trans_grn = Content.Load<Texture2D>("Trans_Green");
            tex_trans_blu = Content.Load<Texture2D>("Trans_Blue");
            tex_trans_cyn = Content.Load<Texture2D>("Trans_Cyan");
            tex_trans_ylo = Content.Load<Texture2D>("Trans_Yellow");
            tex_trans_mgn = Content.Load<Texture2D>("Trans_Magenta");
            tex_trans_blk = Content.Load<Texture2D>("Trans_Black");
            tex_trans_gry = Content.Load<Texture2D>("Trans_Grey");

            Texture2D[] uicontent = new Texture2D[] { tex_hair_centre, tex_hair_hor, tex_hair_ver, tex_col_wht, tex_col_red, tex_col_grn, tex_col_blu, tex_col_cyn, tex_col_mgn, tex_col_ylo, tex_col_blk, tex_col_gry };

            tex_splash = Content.Load<Texture2D>("Splash");
            tex_menubuttons = Content.Load<Texture2D>("MenuButtons");
            tex_pause = Content.Load<Texture2D>("PauseScreen");
            tex_pause_over = Content.Load<Texture2D>("PauseOverlay");
            tex_howto = Content.Load<Texture2D>("Howto");
            tex_scorebg = Content.Load<Texture2D>("ScoreBG");
            tex_gameover = Content.Load<Texture2D>("GameOver");

            snd_atatat = Content.Load<SoundEffect>("Atatat");
            snd_bgm = Content.Load<SoundEffect>("BGM");
            snd_death = Content.Load<SoundEffect>("Death");
            snd_explode = Content.Load<SoundEffect>("Explode");
            snd_spawn = Content.Load<SoundEffect>("Spawn");
            snd_hit = Content.Load<SoundEffect>("Hit");

            snd_bgm_instance = snd_bgm.CreateInstance();
            snd_bgm_instance.IsLooped = true;

            snd_explode_instance = snd_explode.CreateInstance();


            snd_bgm_instance.Volume = 0.05f;
            snd_explode_instance.Volume = 0.1f;

            midpoint = new Vector2(screenwidth / 2, screenheight / 2);
            menu1 = new Rectangle(256, 532, 128, 32);
            menu2 = new Rectangle(256, 596, 128, 32);
            menu3 = new Rectangle(256, 660, 128, 32);
            menu4 = new Rectangle(256, 725, 128, 32);
            noticeRect = new Rectangle(20, 63, 600, 320);

            rectRed= new Rectangle(210, 0, 220, 400);
            rectGreen= new Rectangle(0, 448, 210, 448);
            rectBlue= new Rectangle(430, 448, 210, 448);
            rectCyan= new Rectangle(210, 496, 220, 400);
            rectMagenta= new Rectangle(430, 0, 210, 448);
            rectYellow= new Rectangle(0, 0, 210, 448);
            rectClear = new Rectangle(210, 400, 220, 96);
            rectBlack= new Rectangle(210, 400, 220, 48);
            rectWhite= new Rectangle(210, 448, 220, 48);

            ui1 = new UI(spriteBatch, font, uicontent);
            weapons[0] = new WeaponZ(ui1, tex_wep_none);
            weapons[1] = new WeaponA(ui1, tex_wep_a, tex_wep_a_x);

            weapons[2] = new WeaponB(ui1, tex_wep_b, tex_wep_b_x, new Texture2D[3]{tex_wep_b_x_i, tex_wep_b_x_i1, tex_wep_b_x_i2});
            weapons[3] = new WeaponC(ui1, tex_wep_c, tex_wep_c_x);//use effect constructor
            weapons[4] = new WeaponD(ui1, tex_wep_d, tex_wep_d_x);
            player1 = new Player(tex_player, weapons, ui1);

            enemytex[0] = tex_enemy_tiny;
            enemytex[1] = tex_enemy_small;
            enemytex[2] = tex_enemy_medium;
            enemytex[3] = tex_enemy_large;
            enemytex[4] = tex_enemy_huge;
            enemytex[5] = tex_enemy_xbox;

            weapons[0].ChangeLevel(3, 0);
            weapons[1].ChangeLevel(3, 0);
            weapons[2].ChangeLevel(3, 0);
            weapons[3].ChangeLevel(3, 0);
            weapons[4].ChangeLevel(3, 0);
            player1.ChangeWeapon(3, 1);
            player1.ChangeColour(1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            CheckInput(2);
            SoundControl();

            switch (gameMode)
            {
                case SPLASH:
                    break;
                case MENU:
                    TakeName();
                    break;
                case GAME:
                    CoolWeapons();
                    SpawnEnemies();
                    Collisions();
                    MoveItems();
                    Cleanup();
                    break;
                case PAUSE:
                    break;
                case GAMEOVER:
                    break;
            }

            base.Update(gameTime);
        }

        private void ResetGame()
        {
            debug = false;
            enemyList.Clear();
            bulletList.Clear();
            lasthit = -1;
            spawntimerpremax = SPAWNTIMERPREMAX;
            player1.Reset();
            spawn = true;
        }

        private void SoundControl()
        {
            {
                if (music)
                {
                    snd_bgm_instance.Play();
                }

                else if (!music)
                {
                    snd_bgm_instance.Pause();
                }
            }
        }

        /// <summary>
        /// Checks for input.
        /// </summary>
        /// <param name="mode">0 = Mouse, 1 = Keyboard, 2 = Both</param>
        private void CheckInput(byte mode)
        {
            mouseStatePrev = mouseState;
            mouseState = Mouse.GetState();

            mousePosPrev = mousePos;
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            keyStatePrev = keyState;
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape) && keyStatePrev.IsKeyUp(Keys.Escape))
            {
                this.Exit();
            }

            if (mode == 0 || mode == 2)
            {

                if (gameMode == SPLASH)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && mouseStatePrev.LeftButton == ButtonState.Released)
                    {
                        gameMode = MENU;
                    }
                }

                if (gameMode == MENU)
                {
                    if (mouseState.LeftButton == ButtonState.Released && mouseStatePrev.LeftButton == ButtonState.Pressed)
                    {
                        MenuClick(new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8));
                    }
                }

                if (gameMode == GAME)
                {

                    if (mousePos.X > -50 && mousePos.X < 690)
                    {
                        if (!select)
                        {
                            player1.Move((int)mousePos.X, 1);
                        }
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed && mouseStatePrev.LeftButton == ButtonState.Released)
                    {
                        mouseLeftPrev.X = mouseState.X;
                        mouseLeftPrev.Y = mouseState.Y;

                        if (player1.Fire())
                        {
                            Fire();
                        }
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed && mouseStatePrev.LeftButton == ButtonState.Pressed)
                    {
                        if (player1.Refire())
                        {
                            Fire();
                        }

                        if (player1.Colour == 7 || player1.Colour == 0)
                        {
                            player1.CurrentWeapon.InstaCool();
                        }
                    }

                    if (mouseState.RightButton == ButtonState.Pressed && mouseStatePrev.RightButton == ButtonState.Released)
                    {
                        mouseRightPrev = mousePos;
                        Mouse.SetPosition((int)midpoint.X, (int)midpoint.Y);
                        select = true;
                    }


                    if (mouseState.RightButton == ButtonState.Released && mouseStatePrev.RightButton == ButtonState.Pressed)
                    {
                        if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectRed))
                        {
                            player1.ChangeColour(1);
                        }


                        else if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectGreen))
                        {
                            player1.ChangeColour(2);
                        }


                        else if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectBlue))
                        {
                            player1.ChangeColour(3);
                        }


                        else if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectCyan))
                        {
                            player1.ChangeColour(4);
                        }


                        else if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectMagenta))
                        {
                            player1.ChangeColour(5);
                        }


                        else if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectYellow))
                        {
                            player1.ChangeColour(6);
                        }

                        else if (!debug)
                        {
                            if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectClear))
                            {
                            }
                        }

                        else if (debug)
                        {

                            if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectBlack))
                            {
                                player1.ChangeColour(7);
                            }

                            else if (new Rectangle((int)mousePos.X - 4, (int)mousePos.Y - 4, 8, 8).Intersects(rectWhite))
                            {
                                player1.ChangeColour(0);
                            }
                        }


                        Mouse.SetPosition((int)mouseRightPrev.X, (int)mouseRightPrev.Y);
                        select = false;

                    }
                }

                if (gameMode == PAUSE)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && mouseStatePrev.LeftButton == ButtonState.Released)
                    {
                        gameMode = GAME;
                    }
                }

                if (gameMode == GAMEOVER)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && mouseStatePrev.LeftButton == ButtonState.Released)
                    {
                        gameMode = MENU;
                        drawScores = false;
                    }
                }

                if (mode == 1 || mode == 2)
                {

                    if (gameMode == SPLASH)
                    {
                        if (keyState.IsKeyDown(Keys.Space) && keyStatePrev.IsKeyUp(Keys.Space))
                        {
                            gameMode = MENU;
                        }

                        if (keyState.IsKeyDown(Keys.M) && keyStatePrev.IsKeyUp(Keys.M))
                        {
                            music = !music;
                        }

                        if (keyState.IsKeyDown(Keys.N) && keyStatePrev.IsKeyUp(Keys.N))
                        {
                            sfx = !sfx;
                        }
                    }

                    if (gameMode == MENU)
                    {
                        if (!takename)
                        {
                            if (keyState.IsKeyDown(Keys.M) && keyStatePrev.IsKeyUp(Keys.M))
                            {
                                music = !music;
                            }

                            if (keyState.IsKeyDown(Keys.N) && keyStatePrev.IsKeyUp(Keys.N))
                            {
                                sfx = !sfx;
                            }

                            if (keyState.IsKeyDown(Keys.S) && keyStatePrev.IsKeyUp(Keys.S))
                            {
                                SaveScores();
                            }

                            if (keyState.IsKeyDown(Keys.L) && keyStatePrev.IsKeyUp(Keys.L))
                            {
                                LoadScores(currentFile);
                            }

                            if (keyState.IsKeyDown(Keys.K) && keyStatePrev.IsKeyUp(Keys.K))
                            {
                                LoadScores("Hiscoresload.txt");
                            }

                            if (keyState.IsKeyDown(Keys.J) && keyStatePrev.IsKeyUp(Keys.J))
                            {
                                LoadScores("Hiscoressort.txt");
                            }


                        }
                    }


                    if (gameMode == GAME)
                    {
                        if (keyState.IsKeyDown(Keys.M) && keyStatePrev.IsKeyUp(Keys.M))
                        {
                            music = !music;
                        }

                        if (keyState.IsKeyDown(Keys.N) && keyStatePrev.IsKeyUp(Keys.N))
                        {
                            sfx = !sfx;
                        }

                        if (keyState.IsKeyDown(Keys.D1) && keyStatePrev.IsKeyUp(Keys.D1))
                        {
                            player1.ChangeWeapon(3, 1);
                        }

                        if (keyState.IsKeyDown(Keys.D2) && keyStatePrev.IsKeyUp(Keys.D2) && debug)
                        {
                            player1.ChangeWeapon(3, 2);
                        }

                        if (keyState.IsKeyDown(Keys.D3) && keyStatePrev.IsKeyUp(Keys.D3) && debug)
                        {
                            player1.ChangeWeapon(3, 3);
                        }

                        if (keyState.IsKeyDown(Keys.D4) && keyStatePrev.IsKeyUp(Keys.D4) && debug)
                        {
                            player1.ChangeWeapon(3, 4);
                        }

                        if (keyState.IsKeyDown(Keys.D0) && keyStatePrev.IsKeyUp(Keys.D0))
                        {
                            player1.ChangeWeapon(3, 0);
                        }

                        if (keyState.IsKeyDown(Keys.F) && keyStatePrev.IsKeyUp(Keys.F) && debug)
                        {
                            player1.CycleColour();
                        }

                        if (keyState.IsKeyDown(Keys.E) && keyStatePrev.IsKeyUp(Keys.E) && debug)
                        {
                            //Cycles Weapon Levels

                            player1.ChangeWepLevel(2, 0);
                        }
                        
                        if (keyState.IsKeyDown(Keys.V) && keyStatePrev.IsKeyUp(Keys.V) && debug)
                        {
                            spawn = !spawn;
                        }

                        if (keyState.IsKeyDown(Keys.R) && keyStatePrev.IsKeyUp(Keys.R) && debug)
                        {
                            //Resets weapon levels
                            for (int i = 0; i < 4; i++)
                            {
                                if (weapons[i].Level == -1)
                                {
                                    weapons[i].ChangeLevel(3, 0);
                                }
                            }
                        }

                        if (keyState.IsKeyDown(Keys.T) && keyStatePrev.IsKeyUp(Keys.T) && debug)
                        {
                            if (sfx)
                            {
                                SoundEffectInstance spawn_instance = snd_spawn.CreateInstance();
                                spawn_instance.Volume = 0.1f;
                                spawn_instance.Play();
                            }
                            enemyList.Add(new Enemy(ui1, enemytex, 64, 8, 9999, 9999, 0));
                        }

                        if (keyState.IsKeyDown(Keys.Y) && keyStatePrev.IsKeyDown(Keys.Y) && debug)
                        {
                            if (sfx)
                            {
                                SoundEffectInstance spawn_instance = snd_spawn.CreateInstance();
                                spawn_instance.Volume = 0.1f;
                                spawn_instance.Play();
                            }
                            enemyList.Add(new Enemy(ui1, enemytex, 64, 8, 9999, 9999, 0));
                        }

                        if (keyState.IsKeyDown(Keys.U) && keyStatePrev.IsKeyDown(Keys.U) && debug)
                        {
                            if (sfx)
                            {
                                SoundEffectInstance spawn_instance = snd_spawn.CreateInstance();
                                spawn_instance.Volume = 0.1f;
                                spawn_instance.Play();
                            }
                            enemyList.Add(new Enemy(ui1, enemytex, 64, 8, 9999, 99999, 0));
                        }

                        if (keyState.IsKeyDown(Keys.Z) && keyStatePrev.IsKeyUp(Keys.Z) && debug)
                        {
                            player1.Death();
                        }

                        if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.Back))
                        {
                            if (!debug)
                            {
                                debug = true;
                            }
                        }

                        if (keyState.IsKeyDown(Keys.Back) && keyStatePrev.IsKeyUp(Keys.Back) && debug)
                        {
                            debug = false;
                        }

                        if (keyState.IsKeyDown(Keys.H) && keyStatePrev.IsKeyUp(Keys.H) && debug)
                        {
                            debugmsg = !debugmsg;
                        }

                        if (keyState.IsKeyDown(Keys.Space) && keyStatePrev.IsKeyUp(Keys.Space))
                        {
                                gameMode = PAUSE;
                        }
                    }

                    if (gameMode == PAUSE || gameMode == GAMEOVER)
                    {
                        if (keyState.IsKeyDown(Keys.Q) && keyStatePrev.IsKeyUp(Keys.Q))
                        {
                            gameMode = MENU;
                        }

                        if (keyState.IsKeyDown(Keys.M) && keyStatePrev.IsKeyUp(Keys.M))
                        {
                            music = !music;
                        }

                        if (keyState.IsKeyDown(Keys.N) && keyStatePrev.IsKeyUp(Keys.N))
                        {
                            sfx = !sfx;
                        }

                    }
                }
            }
        }

        private void SpawnEnemies()
        {
            if (spawn)
            {
                if (spawntimer <= 0)
                {
                    enemyList.Add(new Enemy(ui1, enemytex, 64, 8, 9999, 9999, 0));
                    if (sfx)
                    {
                        SoundEffectInstance spawn_instance = snd_spawn.CreateInstance();
                        spawn_instance.Volume = 0.1f;
                        spawn_instance.Play();
                    }
                    spawntimer = spawntimerpremax * 60;

                    if (spawntimerpremax > 1)
                    {
                        spawntimerpremax -= 0.125f;
                    }
                }
                spawntimer--;
            }
        }

        private void Fire()
        {
            switch (player1.CurrentWeapon.Number)
            {
                case 1:
                    if (player1.CurrentWeapon.Level != -1)
                    {
                        switch (player1.CurrentWeapon.Level)
                        {
                            case 0:
                                bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f, tex_wep_a_i, 0));
                                if (sfx)
                                {
                                    SoundEffectInstance atatat_instance = snd_atatat.CreateInstance();
                                    atatat_instance.Volume = 0.1f;
                                    atatat_instance.Play();
                                }
                                break;
                            case 1:
                                bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f, tex_wep_a_i, 0));
                                bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f, tex_wep_a_i, 1));
                                if (sfx)
                                {
                                    SoundEffectInstance atatat_instance = snd_atatat.CreateInstance();
                                    atatat_instance.Volume = 0.1f;
                                    atatat_instance.Play();
                                    SoundEffectInstance atatat1_instance = snd_atatat.CreateInstance();
                                    atatat1_instance.Volume = 0.1f;
                                    atatat1_instance.Play();
                                }
                                break;
                            case 2:
                                bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f1, tex_wep_a_i, 0));
                                bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f1, tex_wep_a_i, 1));
                                if (sfx)
                                {
                                    SoundEffectInstance atatat_instance = snd_atatat.CreateInstance();
                                    atatat_instance.Volume = 0.1f;
                                    atatat_instance.Play();
                                    SoundEffectInstance atatat1_instance = snd_atatat.CreateInstance();
                                    atatat1_instance.Volume = 0.1f;
                                    atatat1_instance.Play();
                                }
                                break;
                        }
                    }
                    break;
                case 2:
                    if (player1.CurrentWeapon.Level != -1)
                    {
                        //switch (player1.CurrentWeapon.Level)
                        //{
                        //    case 0:
                        //        bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f, tex_wep_a_i, 0));
                        //        break;
                        //    case 1:
                        //        bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f, tex_wep_a_i, 0));
                        //        bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f, tex_wep_a_i, 1));
                        //        break;
                        //    case 2:
                        //        bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f1, tex_wep_a_i, 0));
                        //        bulletList.Add(new Bullet(player1.CurrentWeapon.Level, player1.Colour, mousePos, player1.Pos, tex_wep_a_f1, tex_wep_a_i, 1));
                        //        break;
                        //}
                    }
                    break;
                case 3:
                    break;
                case 4:
                    break;

            } 
        }

        private void CoolWeapons()
        {
            try
            {
                for (int i = 0; i <= 4; i++)
                {
                    weapons[i].Cool();
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
        }

        private void MoveItems()
        {
            foreach (Bullet b in bulletList)
            {
                b.Move();
            }


            foreach (Enemy e in enemyList)
            {
                e.Move();
            }

        }

        private void SortScores()
        {
            int pass, current;
            for (pass = 0; pass < 5; pass++)
            {
                int min = pass;
                for (current = pass + 1; current < 5; current++)
                {
                    if (hiscores[current].Score > hiscores[min].Score)
                    {
                        min = current;
                    }
                }
                Player temp = hiscores[pass];
                hiscores[pass] = hiscores[min];
                hiscores[min] = temp;
            }
        }

        private void UpdateScores()
        {
            for (int i = 0; i < 5; i++)
            {
                if (player1.Score > hiscores[i].Score)
                {
                    Player playercopy = new Player(player1.Score, player1.Name);
                    Player tempplayer = hiscores[i];//copy overwritten player
                    hiscores[i] =  playercopy;//insert player score
                    if (i != 4)//if player is not last score
                    {
                        for (int j = 4; j >= i; j--)
                        {
                            if (j > i)
                            {
                                hiscores[j] = hiscores[j - 1];
                            }

                            if (j == i)
                            {
                                hiscores[j] = tempplayer;
                            }
                            
                        }
                    }
                    break;
                    
                }
            }
        }

        private void Collisions()
        {
            //bullets and enemies

            foreach (Bullet b in bulletList)
            {
                foreach (Enemy e in enemyList)
                {
                    if (b.GetRect().Intersects(e.GetRect()))
                    {
                        if (!b.Hit)
                        {
                            if (sfx)
                            {
                                SoundEffectInstance hit_instance = snd_hit.CreateInstance();
                                hit_instance.Volume = 0.1f;
                                hit_instance.Play();
                            }
                            b.HitObject();
                            e.Hit(b.Damage, b.Colour);
                            lasthit = enemyList.IndexOf(e);
                        }
                    }
                }
            }

            ////bullets and enemy avoids
            //foreach (Bullet b in bulletList)
            //{
            //    foreach (Enemy e in enemyList)
            //    {
            //        if (b.GetRect().Intersects(e.GetAvoidLeftRect()) || b.GetRect().Intersects(e.GetAvoidRightRect()))
            //        {
            //            e.AvoidSide(b.Pos);
            //        }
            //    }
            //}

            //foreach (Bullet b in bulletList)
            //{
            //    foreach (Enemy e in enemyList)
            //    {
            //        if (b.GetRect().Intersects(e.GetAvoidFrontRect()))
            //        {
            //            e.AvoidSide(b.Pos);
            //            e.AvoidFront();
            //        }
            //    }
            //}

            //grenades and enemies
            //coil and enemies
            //depleted and enemies
            //enemies and enemies (colour)

            //enemies and player
            foreach (Enemy e in enemyList)
            {
                if (e.GetRect().Intersects(player1.GetRect()))
                {
                    player1.Hit(e.Size, e.Colour);
                    e.Death();
                }
            }

        }


        private void Cleanup()
        {
            for (int b = 0; b < bulletList.Count; b++)
            {
                if (bulletList[b].Active == false)
                {
                    bulletList.RemoveAt(b);
                }
            }

            for (int e = 0; e < enemyList.Count; e++)
            {
                enemyList[e].CheckPos();
            }

            for (int e = 0; e < enemyList.Count; e++)
            {
                enemyList[e].CheckHealth();
            }


            for (int e = 0; e < enemyList.Count; e++)
            {
                if (!enemyList[e].Alive)
                {
                    if (sfx)
                    {
                        SoundEffectInstance death_instance = snd_death.CreateInstance();
                        death_instance.Volume = 0.1f;
                        death_instance.Play();
                    }
                    player1.ChangeScore(enemyList[e].Size, false);
                    player1.GainXP(enemyList[e].Size);
                }
            }

            for (int e = 0; e < enemyList.Count; e++)
            {
                if (enemyList[e].Active == false)
                {
                    enemyList.RemoveAt(e);
                    if (lasthit == e)
                    {
                        lasthit = -1;
                    }
                }
            }

            player1.CheckHealth();

            if (!player1.Active)
            {
                if (sfx)
                {
                    snd_explode_instance.Play();
                }
                SortScores();
                UpdateScores();
                drawScores = true;
                gameMode = GAMEOVER;
            }
        }

        private void MenuClick(Rectangle clickRect)
        {
            if (clickRect.Intersects(menu1))
            {
                ResetGame();
                gameMode = GAME;
            }

            if (clickRect.Intersects(menu2))
            {
                drawScores = !drawScores;
                drawHow = false;
            }

            if (clickRect.Intersects(menu3))
            {
                drawHow = !drawHow;
                drawScores = false;
            }

            if (clickRect.Intersects(menu4))
            {
                this.Exit();
            }

            if (clickRect.Intersects(noticeRect) && drawScores)
            {
                takename = true;
            }

            if (!clickRect.Intersects(noticeRect) && takename)
            {
                takename = false;
            }

        }

        private void TakeName()
        {
            if (takename)
            {

                // the keys that were pressed before – initially an empty array
                // the keys that are currently pressed
                Keys[] pressedKeys;
                pressedKeys = keyState.GetPressedKeys();

                // work through each key that is presently pressed
                for (int i = 0; i < pressedKeys.Length; i++)
                {
                    // set a flag to indicate we have not found the key
                    bool foundIt = false;

                    // work through each key in the old keys
                    for (int j = 0; j < oldKeys.Length; j++)
                    {
                        if (pressedKeys[i] == oldKeys[j])
                        {
                            // we found the key in the old keys
                            foundIt = true;
                        }
                    }
                    if (foundIt == false)
                    {
                        // if we get here we didn't find the key in the old key
                        // now decode the key value for use in the message
                        string keyString = ""; // initially this is an empty string
                        switch (pressedKeys[i])
                        {
                            case Keys.D0:
                                keyString = "0";
                                break;
                            case Keys.D1:
                                keyString = "1";
                                break;
                            case Keys.D2:
                                keyString = "2";
                                break;
                            case Keys.D3:
                                keyString = "3";
                                break;
                            case Keys.D4:
                                keyString = "4";
                                break;
                            case Keys.D5:
                                keyString = "5";
                                break;
                            case Keys.D6:
                                keyString = "6";
                                break;
                            case Keys.D7:
                                keyString = "7";
                                break;
                            case Keys.D8:
                                keyString = "8";
                                break;
                            case Keys.D9:
                                keyString = "9";
                                break;
                            case Keys.A:
                                keyString = "A";
                                break;
                            case Keys.B:
                                keyString = "B";
                                break;
                            case Keys.C:
                                keyString = "C";
                                break;
                            case Keys.D:
                                keyString = "D";
                                break;
                            case Keys.E:
                                keyString = "E";
                                break;
                            case Keys.F:
                                keyString = "F";
                                break;
                            case Keys.G:
                                keyString = "G";
                                break;
                            case Keys.H:
                                keyString = "H";
                                break;
                            case Keys.I:
                                keyString = "I";
                                break;
                            case Keys.J:
                                keyString = "J";
                                break;
                            case Keys.K:
                                keyString = "K";
                                break;
                            case Keys.L:
                                keyString = "L";
                                break;
                            case Keys.M:
                                keyString = "M";
                                break;
                            case Keys.N:
                                keyString = "N";
                                break;
                            case Keys.O:
                                keyString = "O";
                                break;
                            case Keys.P:
                                keyString = "P";
                                break;
                            case Keys.Q:
                                keyString = "Q";
                                break;
                            case Keys.R:
                                keyString = "R";
                                break;
                            case Keys.S:
                                keyString = "S";
                                break;
                            case Keys.T:
                                keyString = "T";
                                break;
                            case Keys.U:
                                keyString = "U";
                                break;
                            case Keys.W:
                                keyString = "W";
                                break;
                            case Keys.V:
                                keyString = "V";
                                break;
                            case Keys.X:
                                keyString = "X";
                                break;
                            case Keys.Y:
                                keyString = "Y";
                                break;
                            case Keys.Z:
                                keyString = "Z";
                                break;
                            case Keys.Space:
                                keyString = " ";
                                break;
                            case Keys.OemPeriod:
                                keyString = ".";
                                break;
                            case Keys.Enter:
                                takename = false;
                                break;
                        }


                        if (pressedKeys[i] == Keys.Back)
                        {
                            if (newname.Length > 0)
                            {
                                newname = newname.Remove(newname.Length - 1);
                            }
                        }

                        newname = newname + keyString;

                        if (!takename && newname.Length > 0 && newname.Length <= 8)
                        {
                            player1.Name = newname;
                        }

                        else if (!takename && player1.Name.Length + keyString.Length <= 0)
                        {
                            player1.Name = "Oshino";
                        }
                    }
                }
                // remember the keys for next time
                oldKeys = pressedKeys;
            }
        }

        private void SaveScores()
        {
            SortScores();
            try
            {
                StreamWriter outputStream = File.CreateText(currentFile); // creates the file
                for (int i = 0; i <= 4; i++)
                {
                    int j = i + 1;
                    outputStream.Write("" + j +" ");
                    outputStream.Write("" + hiscores[i].Name.ToUpper() + " ");
                    outputStream.Write("" + hiscores[i].Score + "\n");
                }

                outputStream.Close(); // close the file stream
                saved = 120;
            }
            catch (FileNotFoundException problem)
            {
                excepfnf = true;
                excepfnfproblem = problem;
            }
            catch (IOException anException)
            {
                excepio = true;
            }


        }

        private void LoadScores(string filename)
        {

            try
            {
                string line;
                string[] words;

                StreamReader inputStream = File.OpenText(filename);
                for (int i = 0; i <= 4; i++)
                {
                    line = inputStream.ReadLine();
                    words = line.Split(' ');
                    hiscores[i].Name = words[1];
                    hiscores[i].Score = Convert.ToInt32(words[2]);
                }

                inputStream.Close();
            }

            catch (FileNotFoundException problem)
            {
                excepfnf = true;
                excepfnfproblem = problem;
            }
            catch (IOException anException)
            {
                excepio = true;
            }
            loaded = 120;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(grm);

            spriteBatch.Begin();
            if (gameMode == SPLASH)
            {
                spriteBatch.Draw(tex_splash, new Rectangle(0, 0, screenwidth, screenheight), Color.White);

                ui1.DrawHairCentre(mousePos, player1.Pos);
            }

            if (gameMode == MENU)
            {
                spriteBatch.Draw(tex_splash, new Rectangle(0, 0, screenwidth, screenheight), Color.White);

                spriteBatch.Draw(tex_menubuttons, menu1, new Rectangle(0, 0, 128, 32), Color.White);
                spriteBatch.Draw(tex_menubuttons, menu2, new Rectangle(0, 32, 128, 32), Color.White);
                spriteBatch.Draw(tex_menubuttons, menu3, new Rectangle(0, 64, 128, 32), Color.White);
                spriteBatch.Draw(tex_menubuttons, menu4, new Rectangle(0, 96, 128, 32), Color.White);

                ui1.DrawString(105  , 800, "BGM: \"Speaker 1, Speaker 1\" by Ronald Jenkees");

                if (drawHow)
                {
                    spriteBatch.Draw(tex_howto, noticeRect, Color.White);
                }

                if (drawScores)
                {
                    SortScores();
                    spriteBatch.Draw(tex_scorebg, noticeRect, Color.White);
                    ui1.DrawString(280, 96, "SCORES");
                    ui1.DrawString(200, 140, "1.\n2.\n3.\n4.\n5.");
                    ui1.DrawString(220, 140, "" + hiscores[0].Name + "\n" + hiscores[1].Name + "\n" + hiscores[2].Name + "\n" + hiscores[3].Name + "\n" + hiscores[4].Name + "");
                    ui1.DrawString(400, 140, "" + hiscores[0].Score + "\n" + hiscores[1].Score + "\n" + hiscores[2].Score + "\n" + hiscores[3].Score + "\n" + hiscores[4].Score + "");

                    ui1.DrawString(250, 300, "Name:");

                    if (takename)
                    {
                        ui1.DrawString(250, 300, "Name:", Color.White);
                        ui1.DrawString(300, 300, newname, Color.White);
                    }
                    else
                    {
                        ui1.DrawString(250, 300, "Name:");
                        ui1.DrawString(300, 300, "" + player1.Name);
                    }
                }

                ui1.DrawHairCentre(mousePos, player1.Pos);
            }

            

            if (gameMode == GAME || gameMode == PAUSE)
            {
                foreach (Enemy e in enemyList)
                {
                    e.Draw(spriteBatch);
                }

                foreach (Bullet b in bulletList)
                {
                    b.Draw(spriteBatch);
                }

                try
                {
                    if (lasthit != -1)
                    {
                        ui1.DrawEnemyHealth(enemyList[lasthit]);
                    }
                }

                catch (ArgumentOutOfRangeException)
                {
                    lasthit = -1;
                }

                if (select)
                {
                    spriteBatch.Draw(tex_trans_red, rectRed, Color.White);
                    spriteBatch.Draw(tex_trans_grn, rectGreen, Color.White);
                    spriteBatch.Draw(tex_trans_blu, rectBlue, Color.White);
                    spriteBatch.Draw(tex_trans_cyn, rectCyan, Color.White);
                    spriteBatch.Draw(tex_trans_mgn, rectMagenta, Color.White);
                    spriteBatch.Draw(tex_trans_ylo, rectYellow, Color.White);
                    if (debug)
                    {
                        spriteBatch.Draw(tex_trans_wht, rectWhite, Color.White);
                        spriteBatch.Draw(tex_trans_blk, rectBlack, Color.White);
                    }
                }



                if (debug && debugmsg)
                {
                    ui1.DrawString(0, 0, "Debug Enabled, Backspace to exit.\n F - Cycle Colours\n E - Level Weapons\n R - Reset Weapons\n 0,1,2,3,4 - Weapon Select\n T - Spawn Enemy\n (Y) - Spawn Enemies\n (U) - Spawn Enemies anywhere\n Z - Die\n V - Automated Spawning Toggle\n H - Hide Keys");
                }

                if (debug && !debugmsg)
                {
                    ui1.DrawString(0, 0, "Debug Enabled, Backspace to exit.");
                }

                if (gameMode == PAUSE)
                {
                    if (pausetimer <= 0)
                    {
                        pausetimer = 120;
                    }

                    const int PAUSEWIDTH = 256;
                    const int PAUSEHEIGHT = 32;

                    spriteBatch.Draw(tex_pause_over, new Rectangle(0, 0, screenwidth, screenheight), Color.White);

                    if (pausetimer > 60)
                    {
                        spriteBatch.Draw(tex_pause, new Rectangle((int)midpoint.X - PAUSEWIDTH / 2, (int)midpoint.Y - PAUSEHEIGHT / 2, PAUSEWIDTH, PAUSEHEIGHT), new Rectangle(0, 0, PAUSEWIDTH, PAUSEHEIGHT), Color.White);
                    }

                    else if (pausetimer > 0)
                    {
                        spriteBatch.Draw(tex_pause, new Rectangle((int)midpoint.X - PAUSEWIDTH / 2, (int)midpoint.Y - PAUSEHEIGHT / 2, PAUSEWIDTH, PAUSEHEIGHT), new Rectangle(0, PAUSEHEIGHT, PAUSEWIDTH, PAUSEHEIGHT), Color.White);
                    }

                    ui1.DrawString((int)midpoint.X - 80, (int)midpoint.Y + 20, "Click to continue");
                    ui1.DrawString((int)midpoint.X - 40, (int)midpoint.Y + 40, "Q to quit");

                    pausetimer--;

                }

                ui1.DrawHairs(mousePos, player1.Pos);
                player1.Draw(spriteBatch, mousePos);
            }


            if (gameMode == GAMEOVER)
            {
                if (pausetimer <= 0)
                {
                    pausetimer = 120;
                }

                const int PAUSEWIDTH = 256;
                const int PAUSEHEIGHT = 32;

                spriteBatch.Draw(tex_pause_over, new Rectangle(0, 0, screenwidth, screenheight), Color.White);

                if (pausetimer > 60)
                {
                    spriteBatch.Draw(tex_gameover, new Rectangle((int)midpoint.X - PAUSEWIDTH / 2, (int)midpoint.Y - PAUSEHEIGHT / 2, PAUSEWIDTH, PAUSEHEIGHT), new Rectangle(0, 0, PAUSEWIDTH, PAUSEHEIGHT), Color.White);
                }

                else if (pausetimer > 0)
                {
                    spriteBatch.Draw(tex_gameover, new Rectangle((int)midpoint.X - PAUSEWIDTH / 2, (int)midpoint.Y - PAUSEHEIGHT / 2, PAUSEWIDTH, PAUSEHEIGHT), new Rectangle(0, PAUSEHEIGHT, PAUSEWIDTH, PAUSEHEIGHT), Color.White);
                }

                ui1.DrawString((int)midpoint.X - 60, (int)midpoint.Y + 20, "Final score: " + player1.Score);
                ui1.DrawString((int)midpoint.X - 40, (int)midpoint.Y + 40, "Q to quit");
                ui1.DrawHairs(mousePos, player1.Pos);

                if (drawScores)
                {
                    SortScores();
                    spriteBatch.Draw(tex_scorebg, noticeRect, Color.White);
                    ui1.DrawString(280, 96, "SCORES");
                    ui1.DrawString(200, 140, "1.\n2.\n3.\n4.\n5.");
                    ui1.DrawString(220, 140, "" + hiscores[0].Name + "\n" + hiscores[1].Name + "\n" + hiscores[2].Name + "\n" + hiscores[3].Name + "\n" + hiscores[4].Name + "");
                    ui1.DrawString(400, 140, "" + hiscores[0].Score + "\n" + hiscores[1].Score + "\n" + hiscores[2].Score + "\n" + hiscores[3].Score + "\n" + hiscores[4].Score + "");
                }
            }

            if (saved > 0)
            {
                ui1.DrawMsg(midpoint,"Hiscores\n  Saved", Color.Black);
                saved--;
            }

            if (loaded > 0)
            {
                ui1.DrawMsg(midpoint, "Hiscores\n Loaded", Color.Black);
                loaded--;
            }

            if (excepio)
            {
                ui1.DrawString(10, 100, "Error occured when trying to write to the file PengiSave.txt", Color.Red);
            }

            if (excepfnf)
            {
                ui1.DrawString(10, 120, "File not found: " + currentFile + " \n" + excepfnfproblem, Color.Red);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
