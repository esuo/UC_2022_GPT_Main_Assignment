using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RC_Framework;

namespace MainAssignment
{
    class GameHelper
    {
        public static int score = 0;
        public static float gravity = 0.32f;
        public static int speed = 3;
        public static int max_coins = 15;
        public static int coin_drop_speed = 3;
        public static int bullet_speed = 3;
        public static int round_rows = 4;
        public static int round_colums = 8;

        public static string T_Level1 = "Catch As Many As You Can";
        public static string T_Level2 = "Beware of The Trap";
        public static string T_Level3 = "Find a Way Home";
        public static string T_Level4 = "Dodge The Bullet";
    }
    class Dir
    {
        public static string source_dir = @"E:\Projects\Game\MainAssignment\"; //Must Change
        public static string art_dir = source_dir + @"Art\"; // DO NOT CHANGE!!!!!!!!!!!
    }
    class GameLevel_Menu : RC_GameStateParent
    {
        Rectangle screenRect;

        Texture2D texBackground = null;
        Texture2D texTitle = null;
        Texture2D texStart = null;
        Texture2D texLeaderboard = null;
        Texture2D texCoin = null;

        ImageBackground title = null;
        ImageBackground start = null;
        ImageBackground leaderboard = null;
        ScrollBackGround background = null;

        ParticleSystem p;

        Rectangle rectangle = new Rectangle(0, 0, 800, 600);

        public override void EnterLevel(int fromLevelNum)
        {
            GameHelper.score = 0;
            base.EnterLevel(fromLevelNum);
        }
        public override void LoadContent()
        {
            LeaderBoard.getLeaderboard();
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            texTitle = Util.texFromFile(graphicsDevice, Dir.art_dir + "Header.png");
            texStart = Util.texFromFile(graphicsDevice, Dir.art_dir + "Start.png");
            texLeaderboard = Util.texFromFile(graphicsDevice, Dir.art_dir + "leaderboard.png");
            texCoin = Util.texFromFile(graphicsDevice, Dir.art_dir + "_dogecoin.png");

            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);
            title = new ImageBackground(texTitle, new Rectangle(0, 0, 1200, 150), new Rectangle(155, 150, 500, 60), Color.White);
            start = new ImageBackground(texStart, new Rectangle(0, 0, 1200, 150), new Rectangle(105, 300, 600, 70), Color.Green);
            leaderboard = new ImageBackground(texLeaderboard, new Rectangle(0, 0, 1200, 150), new Rectangle(105, 450, 600, 70), Color.White);

            Particle();
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            if (keyState.IsKeyDown(Keys.Enter) && !prevKeyState.IsKeyDown(Keys.Enter))
                gameStateManager.setLevel(8);
            if (keyState.IsKeyDown(Keys.Space) && !prevKeyState.IsKeyDown(Keys.Space))
                gameStateManager.setLevel(7);
            p.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            title.Draw(spriteBatch);
            start.Draw(spriteBatch);
            leaderboard.Draw(spriteBatch);
            p.Draw(spriteBatch);
        }
        void Particle()
        {
            p = new ParticleSystem(new Vector2(300, 50), 60, 0, 150);
            p.setMandatory1(texCoin, new Vector2(24, 24), new Vector2(64, 64), Color.White, new Color(255, 255, 255, 100));
            p.setMandatory2(-1, 10, 5, 3, 0);
            rectangle = new Rectangle(0, 0, 800, 700);
            p.setMandatory3(120, rectangle);
            p.setMandatory4(new Vector2(0, 0.1f), new Vector2(1, 0), new Vector2(0, 0.1f));
            p.randomDelta = new Vector2(0.1f, 0.1f);
            p.Origin = 1;
            p.originRectangle = new Rectangle(0, 0, 800, 5);
            p.activate();
        }
    }
    class GameLevel_Level_1 : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        TextRenderable score = null;
        TextRenderable levelTitle = null;
        TextRenderableFade tip = null;
        SpriteFont fonty;

        Texture2D texCharacter = null;
        Texture2D texDogeCoin = null;
        Texture2D texDoor0 = null;
        Texture2D texDoor1 = null;

        Sprite3 Door0;
        Sprite3 Door1;
        Sprite3 character;

        SpriteList coins = new SpriteList();
        Sprite3 coin;

        float yvel = 0;
        float ypos = 620;
        float xpos = 20;
        int direction = 1; // 0 = left, 1 = right, 2 = fornt, 3 = back
        int dCoins = 0;             
        bool gameWin = false;
        int _coins = 0;
        int loop = 0;
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            fonty = Content.Load<SpriteFont>("SpriteFont1");
            score = new TextRenderable("Score: " + GameHelper.score, new Vector2(50, 100), fonty, Color.Red);
            levelTitle = new TextRenderable("Level 1: " + GameHelper.T_Level1, new Vector2(230, 50), fonty, Color.Red);

            texDoor0 = Util.texFromFile(graphicsDevice, Dir.art_dir + "Door0.png");
            texDoor1 = Util.texFromFile(graphicsDevice, Dir.art_dir + "Door1.png");
            Door0 = new Sprite3(true, texDoor0, 770, 620);
            Door1 = new Sprite3(true, texDoor1, 770, 620);

            texCharacter = Util.texFromFile(graphicsDevice, Dir.art_dir + "character.png");
            character = new Sprite3(true, texCharacter, xpos, ypos);
            character.setWidthHeight(36, 48);
            character.setWidthHeightOfTex(72, 131);
            character.setXframes(3);
            character.setYframes(4);
            character.setHSoffset(new Vector2(12, 16));
            character.setBB(0, 0, 18, 33);
            Vector2[] anim = new Vector2[4];
            anim[0].X = 1; anim[0].Y = 1;
            anim[1].X = 0; anim[1].Y = 1;
            anim[2].X = 1; anim[2].Y = 1;
            anim[3].X = 2; anim[3].Y = 1;
            character.setAnimationSequence(anim, 0, 3, 5);
            character.animationStart();

            texDogeCoin = Util.texFromFile(graphicsDevice, Dir.art_dir + "dogecoin.png");

            do
            {
                Random rd = new Random();
                int count = rd.Next(3, 6);
                if (_coins + count > GameHelper.max_coins)
                {
                    count = GameHelper.max_coins - _coins;
                }
                _coins += count;
                for (int i = 0; i < count; i++)
                {
                    rd = new Random();
                    int rd_x = rd.Next(64, graphicsDevice.Viewport.Width - 64);
                    int rd_y = rd.Next(-192, -64) - (loop * graphicsDevice.Viewport.Height);
                    coin = new Sprite3(true, texDogeCoin, rd_x, rd_y);
                    coin.setXframes(8);
                    coin.setYframes(1);
                    coin.setWidthHeight(64, 64);
                    coin.setMoveAngleDegrees(0);
                    coin.setDeltaSpeed(new Vector2(0, (float)(GameHelper.coin_drop_speed + rd.NextDouble() + rd.NextDouble())));
                    Vector2[] seq = new Vector2[8];
                    seq[0].X = 0; seq[0].Y = 0;
                    seq[1].X = 1; seq[1].Y = 0;
                    seq[2].X = 2; seq[2].Y = 0;
                    seq[3].X = 3; seq[3].Y = 0;
                    seq[4].X = 4; seq[4].Y = 0;
                    seq[5].X = 5; seq[5].Y = 0;
                    seq[6].X = 6; seq[6].Y = 0;
                    seq[7].X = 7; seq[7].Y = 0;
                    coin.setAnimationSequence(seq, 0, 7, 10);
                    coin.setBB(0, 0, 64, 64);
                    coin.animationStart();
                    coins.addSpriteReuse(coin);
                }
                loop++;
            } while (_coins != GameHelper.max_coins);
            
        }
        public override void ExitLevel()
        {
            gameWin = false;
            xpos = 20;
            ypos = 620;
            loop = 0;
            _coins = 0;
            foreach (Sprite3 coin in coins)
            {
                Random rd = new Random();
                int rd_x = rd.Next(64, graphicsDevice.Viewport.Width - 64);
                int rd_y = rd.Next(-192, -64);
                coin.setPos(rd_x, rd_y);
            }
            base.ExitLevel();
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();
            coins.moveDeltaXY_Visible();
            coins.animationTick(gameTime);
            score.text = "Score: " + GameHelper.score;

            if ((keyState.IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up)) && yvel == 0)
                yvel = -10;
            yvel = yvel + GameHelper.gravity;
            ypos = ypos + yvel;
            if (ypos > 620)
            {
                ypos = 620;
                yvel = 0;
            }

            if (tip != null)
                tip.Update(gameTime);

            if (keyState.IsKeyDown(Keys.Left))
            {

                if (direction != 0)
                {
                    direction = 0;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 3;
                    anim[1].X = 0; anim[1].Y = 3;
                    anim[2].X = 1; anim[2].Y = 3;
                    anim[3].X = 2; anim[3].Y = 3;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                xpos -= GameHelper.speed;
                if (xpos < 20)
                    xpos = 20;
                character.animationTick(gameTime);
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                if (direction != 1)
                {
                    direction = 1;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 1;
                    anim[1].X = 0; anim[1].Y = 1;
                    anim[2].X = 1; anim[2].Y = 1;
                    anim[3].X = 2; anim[3].Y = 1;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                xpos += GameHelper.speed;
                if (xpos > 780)
                    xpos = 780;
                character.animationTick(gameTime);
            }

            int colision = coins.collisionAA(character);
            if (colision != -1)
            {
                Sprite3 _coin = coins.getSprite(colision);
                _coin.setDeltaSpeed(new Vector2(0, 0));
                _coin.setActive(false);
                tip = new TextRenderableFade("+100", new Vector2(character.getPosX() + 5, character.getPosY() - 35), fonty, Color.Red, Color.Transparent, 80);
                GameHelper.score += 100;
            }

            dCoins = 0;
            for (int i = 0; i < coins.count(); i++)
            {
                Sprite3 coin = coins.getSprite(i);
                if (coin == null || !coin.visible || !coin.active)
                {
                    dCoins++;
                    continue;
                }
                if (coin.getPosY() > graphicsDevice.Viewport.Height + 64)
                {
                    coin.setActive(false);
                }
            }

            if (dCoins == GameHelper.max_coins)
            {
                gameWin = true;
            }

            if (Door1.collision(character) && gameWin)
                gameStateManager.setLevel(2);
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            coins.drawActive(spriteBatch);
            character.Draw(spriteBatch);
            character.setPosX(xpos);
            character.setPosY(ypos);
            score.Draw(spriteBatch);
            if (gameWin)
                Door1.Draw(spriteBatch);
            else
                Door0.Draw(spriteBatch);
            if (tip != null)
                tip.Draw(spriteBatch);
            levelTitle.Draw(spriteBatch);
        }
    }
    class GameLevel_Level_2 : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        TextRenderable score = null;
        TextRenderable levelTitle = null;
        SpriteFont fonty;

        Texture2D texCharacter = null;
        Texture2D texFire = null;
        Texture2D texDoor1 = null;

        Sprite3 Door1;
        Sprite3 character;

        SpriteList fires = new SpriteList();

        float yvel = 0;
        public int direction = 1; // 0 = left, 1 = right, 2 = fornt, 3 = back

        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            fonty = Content.Load<SpriteFont>("SpriteFont1");
            score = new TextRenderable("Score: " + GameHelper.score, new Vector2(50, 100), fonty, Color.Red);
            levelTitle = new TextRenderable("Level 2: " + GameHelper.T_Level2, new Vector2(280, 50), fonty, Color.Red);

            texCharacter = Util.texFromFile(graphicsDevice, Dir.art_dir + "character.png");
            texFire = Util.texFromFile(graphicsDevice, Dir.art_dir + "fire.png");
            texDoor1 = Util.texFromFile(graphicsDevice, Dir.art_dir + "Door1.png");

            Sprite3 fire;

            for (int i = 0; i < 8; i++)
            {
                fire = new Sprite3(true, texFire, 90 * (i + 1), 588);
                fire.setWidthHeight(37, 56);
                fires.addSprite(fire);
            }

            Door1 = new Sprite3(true, texDoor1, 770, 620);

            character = new Sprite3(true, texCharacter, 20, 620);
            character.setWidthHeight(36, 48);
            character.setWidthHeightOfTex(72, 131);
            character.setXframes(3);
            character.setYframes(4);
            character.setHSoffset(new Vector2(12, 16));
            character.setBB(2, 0, 17, 33);
            Vector2[] anim = new Vector2[4];
            anim[0].X = 1; anim[0].Y = 1;
            anim[1].X = 0; anim[1].Y = 1;
            anim[2].X = 1; anim[2].Y = 1;
            anim[3].X = 2; anim[3].Y = 1;
            character.setAnimationSequence(anim, 0, 3, 5);
            character.animationStart();
        }
        public override void ExitLevel()
        {
            character.setPos(20, 620);
            base.ExitLevel();
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();
            score.text = "Score: " + GameHelper.score;
            if ((keyState.IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up)) && yvel == 0)
            {
                yvel -= 10;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {

                if (direction != 0)
                {
                    direction = 0;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 3;
                    anim[1].X = 0; anim[1].Y = 3;
                    anim[2].X = 1; anim[2].Y = 3;
                    anim[3].X = 2; anim[3].Y = 3;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                character.setDeltaSpeed(new Vector2(-GameHelper.speed, 0));
                if (character.getPosX() < 20)
                    character.setPosX(20);
                character.animationTick(gameTime);
            }
            if (prevKeyState.IsKeyDown(Keys.Left) && keyState.IsKeyUp(Keys.Left))
            {
                character.setDeltaSpeed(new Vector2(0, 0));
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                if (direction != 1)
                {
                    direction = 1;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 1;
                    anim[1].X = 0; anim[1].Y = 1;
                    anim[2].X = 1; anim[2].Y = 1;
                    anim[3].X = 2; anim[3].Y = 1;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                character.setDeltaSpeed(new Vector2(GameHelper.speed, 0));
                if (character.getPosX() > 780)
                    character.setPosX(780);
                character.animationTick(gameTime);
            }
            if (prevKeyState.IsKeyDown(Keys.Right) && keyState.IsKeyUp(Keys.Right))
            {
                character.setDeltaSpeed(new Vector2(0, 0));
            }
            character.savePosition();
            character.moveByDeltaXY();
            yvel = yvel + GameHelper.gravity;
            character.setPosY(character.getPosY() + yvel);
            if (character.getPosY() > 620)
            {
                character.setPosY(620);
                yvel = 0;
            }

            int colision = fires.collisionAA(character);
            if (colision != -1)
            {
                gameStateManager.setLevel(6);
            }
            if (Door1.collision(character))
            {
                GameHelper.score += 1000;
                gameStateManager.setLevel(3);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            score.Draw(spriteBatch);
            levelTitle.Draw(spriteBatch);
            character.Draw(spriteBatch);
            fires.Draw(spriteBatch);
            //fires.drawInfo(spriteBatch, Color.Red, Color.Blue);
            //character.drawInfo(spriteBatch, Color.Red, Color.Blue);
            Door1.Draw(spriteBatch);
        }
    }
    class GameLevel_Level_3 : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        TextRenderable score = null;
        TextRenderable levelTitle = null;
        TextRenderableFade tip = null;
        SpriteFont fonty;

        Texture2D texCharacter = null;
        Texture2D texDogeCoin = null;
        Texture2D texTree = null;
        Texture2D texBlock = null;
        Texture2D texDoor0 = null;
        Texture2D texDoor1 = null;

        Sprite3 character;
        Sprite3 tree;
        Sprite3 block;
        SpriteList dogeCoin = new SpriteList();
        Sprite3 Door0;
        Sprite3 Door1;

        float yvel = 0;
        float ypos = 620;
        float xpos = 20;
        public int direction = 1; // 0 = left, 1 = right, 2 = fornt, 3 = back
        int dCoins = 0;

        bool onTree = false;
        bool gameWin = false;
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            fonty = Content.Load<SpriteFont>("SpriteFont1");
            score = new TextRenderable("Score: " + GameHelper.score, new Vector2(50, 100), fonty, Color.Red);
            levelTitle = new TextRenderable("Level 3: " + GameHelper.T_Level3, new Vector2(230, 50), fonty, Color.Red);

            texCharacter = Util.texFromFile(graphicsDevice, Dir.art_dir + "character.png");
            texDogeCoin = Util.texFromFile(graphicsDevice, Dir.art_dir + "dogecoin.png");
            texTree = Util.texFromFile(graphicsDevice, Dir.art_dir + "Tree.png");
            texBlock = Util.texFromFile(graphicsDevice, Dir.art_dir + "pipe.png");
            texDoor0 = Util.texFromFile(graphicsDevice, Dir.art_dir + "Door0.png");
            texDoor1 = Util.texFromFile(graphicsDevice, Dir.art_dir + "Door1.png");

            Door0 = new Sprite3(true, texDoor0, 770, 620);
            Door1 = new Sprite3(true, texDoor1, 770, 620);

            character = new Sprite3(true, texCharacter, xpos, ypos);
            character.setWidthHeight(36, 48);
            character.setWidthHeightOfTex(72, 131);
            character.setXframes(3);
            character.setYframes(4);
            character.setHSoffset(new Vector2(12, 16));
            character.setBB(0, 0, 18, 33);
            Vector2[] anim = new Vector2[4];
            anim[0].X = 1; anim[0].Y = 1;
            anim[1].X = 0; anim[1].Y = 1;
            anim[2].X = 1; anim[2].Y = 1;
            anim[3].X = 2; anim[3].Y = 1;
            character.setAnimationSequence(anim, 0, 3, 5);
            character.animationStart();

            Sprite3 coin = new Sprite3(true, texDogeCoin, 180, 290);
            coin.setXframes(8);
            coin.setYframes(1);
            coin.setWidthHeight(64, 64);
            Vector2[] seq = new Vector2[8];
            seq[0].X = 0; seq[0].Y = 0;
            seq[1].X = 1; seq[1].Y = 0;
            seq[2].X = 2; seq[2].Y = 0;
            seq[3].X = 3; seq[3].Y = 0;
            seq[4].X = 4; seq[4].Y = 0;
            seq[5].X = 5; seq[5].Y = 0;
            seq[6].X = 6; seq[6].Y = 0;
            seq[7].X = 7; seq[7].Y = 0;
            coin.setAnimationSequence(seq, 0, 7, 10);
            coin.animationStart();
            coin.setBB(0, 0, 64, 64);
            dogeCoin.addSpriteReuse(coin);
            for (int i = 0; i < 2; i++)
            {
                coin = new Sprite3(true, texDogeCoin, 320 * (i + 1), 570);
                coin.setXframes(8);
                coin.setYframes(1);
                coin.setWidthHeight(64, 64);
                seq = new Vector2[8];
                seq[0].X = 0; seq[0].Y = 0;
                seq[1].X = 1; seq[1].Y = 0;
                seq[2].X = 2; seq[2].Y = 0;
                seq[3].X = 3; seq[3].Y = 0;
                seq[4].X = 4; seq[4].Y = 0;
                seq[5].X = 5; seq[5].Y = 0;
                seq[6].X = 6; seq[6].Y = 0;
                seq[7].X = 7; seq[7].Y = 0;
                coin.setAnimationSequence(seq, 0, 7, 10);
                coin.animationStart();
                coin.setBB(0, 0, 64, 64);
                dogeCoin.addSpriteReuse(coin);
            }
            tree = new Sprite3(true, texTree, 120, 485);
            tree.setWidthHeight(155, 160);
            tree.setBB(0, 0, 155, 160);

            block = new Sprite3(true, texBlock, 430, -30);
            block.setWidthHeight(125, 625);
            block.setBB(0, 0, 125, 625);
        }
        public override void ExitLevel()
        {
            gameWin = false;
            xpos = 20;
            ypos = 620;
            Sprite3 coin = dogeCoin.getSprite(0);
            coin.setPos(180, 290);
            coin.setActive(true);
            dCoins = 0;
            for (int i = 0; i < 2; i++)
            {
                coin = dogeCoin.getSprite(i + 1);
                coin.setPos(320 * (i + 1), 570);
                coin.active = true;
            }
            base.ExitLevel();
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            dogeCoin.animationTick(gameTime);
            tree.Update(gameTime);

            score.text = "Score: " + GameHelper.score;

            if (tip != null)
                tip.Update(gameTime);

            if ((keyState.IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up)) && yvel == 0)
                yvel = -10;

            if (keyState.IsKeyDown(Keys.Left))
            {

                if (direction != 0)
                {
                    direction = 0;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 3;
                    anim[1].X = 0; anim[1].Y = 3;
                    anim[2].X = 1; anim[2].Y = 3;
                    anim[3].X = 2; anim[3].Y = 3;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                xpos -= GameHelper.speed;
                if (xpos < 20)
                    xpos = 20;
                character.animationTick(gameTime);
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                if (direction != 1)
                {
                    direction = 1;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 1;
                    anim[1].X = 0; anim[1].Y = 1;
                    anim[2].X = 1; anim[2].Y = 1;
                    anim[3].X = 2; anim[3].Y = 1;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                xpos += GameHelper.speed;
                if (xpos > 780)
                    xpos = 780;
                character.animationTick(gameTime);
            }

            int colision = dogeCoin.collisionAA(character);
            if (colision != -1)
            {
                Sprite3 coin = dogeCoin.getSprite(colision);
                coin.setActive(false);
                tip = new TextRenderableFade("+100", new Vector2(character.getPosX() + 5, character.getPosY() - 35), fonty, Color.Red, Color.Transparent, 80);
                GameHelper.score += 100;
                dCoins++;
                if (dCoins == 3)
                    gameWin = true;
            }

            Rectangle _character = character.getBoundingBoxAA();
            Rectangle _tree = tree.getBoundingBoxAA();
            Rectangle _block = block.getBoundingBoxAA();

            if (_character.Intersects(_tree))
            {
                if (character.getPosY() > 500 && direction == 1)
                {
                    if (character.getPosX() >= 120 && character.getPosX() < 275)
                    {
                        character.setPosX(120);
                        xpos = 120;
                    }
                }
                else if (character.getPosY() > 500 && direction == 0)
                {
                    if (character.getPosX() <= 275 && character.getPosX() > 120)
                    {
                        character.setPosX(275);
                        xpos = 275;
                    }
                }
                else if (character.getPosY() <= 490)
                {
                    if (character.getPosX() >= 120 || character.getPosX() <= 275)
                    {
                        character.setPosY(490);
                        onTree = true;
                    }
                }
            }
            if (_character.Intersects(_block))
            {
                if (direction == 1 && character.getPosY() < 600)
                {
                    if (character.getPosX() >= 420 && character.getPosX() < 568)
                    {
                        character.setPosX(420);
                        xpos = 420;
                    }
                }
                else if (direction == 0 && character.getPosY() < 600)
                {
                    if (character.getPosX() <= 568 && character.getPosX() > 420)
                    {
                        character.setPosX(568);
                        xpos = 568;
                    }
                }
                else if (character.getPosY() <= 610)
                {
                    if (character.getPosX() >= 420 || character.getPosX() <= 568)
                    {
                        yvel = 0;
                        character.setPosY(620);
                        ypos = 635;
                    }
                }
            }
            if (character.getPosX() < 120 || character.getPosX() > 275)
            {
                onTree = false;
            }
            if (!onTree)
            {
                yvel = yvel + GameHelper.gravity;
                ypos = ypos + yvel;
                if (ypos > 620)
                {
                    ypos = 620;
                    yvel = 0;
                }
            }
            else
            {
                yvel = yvel + GameHelper.gravity;
                ypos = ypos + yvel;
                if (ypos > 490)
                {
                    ypos = 490;
                    yvel = 0;
                }
            }

            if (Door1.collision(character) && gameWin)
                gameStateManager.setLevel(4);
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            character.Draw(spriteBatch);
            character.setPosX(xpos);
            character.setPosY(ypos);
            dogeCoin.drawActive(spriteBatch);
            tree.Draw(spriteBatch);
            block.Draw(spriteBatch);
            levelTitle.Draw(spriteBatch);
            score.Draw(spriteBatch);

            if (gameWin)
                Door1.Draw(spriteBatch);
            else
                Door0.Draw(spriteBatch);
            if (tip != null)
                tip.Draw(spriteBatch);
        }
    }
    class GameLevel_Level_4 : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        TextRenderable score = null;
        TextRenderable levelTitle = null;
        SpriteFont fonty;

        Texture2D texCharacter = null;
        Texture2D texBullet = null;
        Texture2D texBlock = null;
        Texture2D texDoor1 = null;

        Sprite3 Door1;
        Sprite3 character;
        Sprite3 bullet;

        SpriteList blocks = new SpriteList();

        float ypos = 620;
        int direction = 1; // 0 = left, 1 = right, 2 = fornt, 3 = back
        float pushBackStrength = 8f;
        float pushBackDistance = 25f;

        bool moving = false;
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            fonty = Content.Load<SpriteFont>("SpriteFont1");
            score = new TextRenderable("Score: " + GameHelper.score, new Vector2(50, 100), fonty, Color.Red);
            levelTitle = new TextRenderable("Level 4: " + GameHelper.T_Level4, new Vector2(230, 50), fonty, Color.Red);

            texDoor1 = Util.texFromFile(graphicsDevice, Dir.art_dir + "Door1.png");
            Door1 = new Sprite3(true, texDoor1, 770, 620);

            texBlock = Util.texFromFile(graphicsDevice, Dir.art_dir + "round.png");
            for (int i = 0; i < GameHelper.round_rows; i++)
            {
                for (int n = 0; n < GameHelper.round_colums; n++)
                {
                    Sprite3 _block = new Sprite3(true, texBlock, n * 100 + 15, i * 90 + 150);
                    _block.setWidthHeight(64, 64);
                    _block.boundingSphereRadius = 32;
                    blocks.addSprite(_block);
                }
            }

            texCharacter = Util.texFromFile(graphicsDevice, Dir.art_dir + "character.png");
            character = new Sprite3(true, texCharacter, 20, ypos);
            character.setWidthHeight(36, 48);
            character.setWidthHeightOfTex(72, 131);
            character.setXframes(3);
            character.setYframes(4);
            character.setHSoffset(new Vector2(12, 16));
            character.setBB(0, 0, 18, 33);
            Vector2[] anim = new Vector2[4];
            anim[0].X = 1; anim[0].Y = 1;
            anim[1].X = 0; anim[1].Y = 1;
            anim[2].X = 1; anim[2].Y = 1;
            anim[3].X = 2; anim[3].Y = 1;
            character.setAnimationSequence(anim, 0, 3, 5);
            character.animationStart();

            texBullet = Util.texFromFile(graphicsDevice, Dir.art_dir + "bullet.png");
            bullet = new Sprite3(true, texBullet, 700, 30);
            bullet.setWidthHeight(60, 30);
            bullet.boundingSphereRadius = 30;
            moving = true;

        }
        public override void ExitLevel()
        {
            character.setPos(20, 620);
            ypos = 620;
            bullet.setPos(700, 30);
            base.ExitLevel();
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();
            score.text = "Score: " + GameHelper.score;

            if (moving) moveBullet();

            if (keyState.IsKeyDown(Keys.Left))
            {

                if (direction != 0)
                {
                    direction = 0;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 3;
                    anim[1].X = 0; anim[1].Y = 3;
                    anim[2].X = 1; anim[2].Y = 3;
                    anim[3].X = 2; anim[3].Y = 3;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                character.setDeltaSpeed(new Vector2(-GameHelper.speed, 0));
                if (character.getPosX() < 20)
                    character.setPosX(20);
                character.animationTick(gameTime);
            }
            if (prevKeyState.IsKeyDown(Keys.Left) && keyState.IsKeyUp(Keys.Left))
            {
                character.setDeltaSpeed(new Vector2(0, 0));
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                if (direction != 1)
                {
                    direction = 1;
                    Vector2[] anim = new Vector2[4];
                    anim[0].X = 1; anim[0].Y = 1;
                    anim[1].X = 0; anim[1].Y = 1;
                    anim[2].X = 1; anim[2].Y = 1;
                    anim[3].X = 2; anim[3].Y = 1;
                    character.setAnimationSequence(anim, 0, 3, 5);
                    character.animationStart();
                }
                character.setDeltaSpeed(new Vector2(GameHelper.speed, 0));
                if (character.getPosX() > 780)
                    character.setPosX(780);
                character.animationTick(gameTime);
            }
            if (prevKeyState.IsKeyDown(Keys.Right) && keyState.IsKeyUp(Keys.Right))
            {
                character.setDeltaSpeed(new Vector2(0, 0));
            }
            character.savePosition();
            character.moveByDeltaXY();
            if (bullet.collision(character))
            {
                gameStateManager.setLevel(6);
            }
            if (Door1.collision(character))
            {
                GameHelper.score += 1000;
                gameStateManager.setLevel(5);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            score.Draw(spriteBatch);
            levelTitle.Draw(spriteBatch);
            character.Draw(spriteBatch);
            bullet.Draw(spriteBatch);
            blocks.Draw(spriteBatch);
            Door1.Draw(spriteBatch);
        }
        void moveBullet()
        {
            Vector2 bulletPos = bullet.getPos();
            Vector2 baseDelta = character.getPos() - bulletPos;
            Vector2 moveDelta = baseDelta / baseDelta.Length();
            moveDelta *= GameHelper.bullet_speed;

            Vector2 pushBack = new Vector2(0, 0);

            for (int i = 0; i <= blocks.highUsed; i++)
            {
                Sprite3 ss = blocks.getSprite(i);
                if (ss == null) continue;
                if (!ss.getVisible()) continue;
                if (!ss.active) continue;

                float pushBackDist = pushBackDistance + ss.boundingSphereRadius + bullet.boundingSphereRadius;

                Vector2 pp = ss.getPos();
                if ((pp - bulletPos).Length() < pushBackDist)
                {
                    Vector2 pb = (pp - bulletPos);
                    float dist = pb.Length();
                    float amt = ((pushBackDist - dist) / pushBackDist) * pushBackStrength;
                    pb /= dist;
                    pushBack = pb * (-1);
                }
            }

            Vector2 newBulletPos = bulletPos + moveDelta + pushBack;
            bullet.setPos(newBulletPos);
            bullet.alignDisplayAngle();
            if ((newBulletPos - character.getPos()).Length() <= GameHelper.bullet_speed) moving = false;
        }
    }
    class GameLevel_Win : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        TextRenderable score = null;
        TextRenderable gotoLeaderboard = null;
        SpriteFont fonty;

        Texture2D texYouWin = null;

        ImageBackground youWin = null;
        public override void EnterLevel(int fromLevelNum)
        {
            System.Diagnostics.Debug.WriteLine("Enterd");
            LeaderBoard.updateLeaderboard(GameHelper.score);
        }
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            texYouWin = Util.texFromFile(graphicsDevice, Dir.art_dir + "Win.png");
            youWin = new ImageBackground(texYouWin, new Rectangle(0, 0, 731, 331), new Rectangle(250, 120, 365, 165), Color.White);

            fonty = Content.Load<SpriteFont>("SpriteFont1");
            score = new TextRenderable("Your Score: " + GameHelper.score, new Vector2(335, 355), fonty, Color.Red);
            gotoLeaderboard = new TextRenderable("Press Space to Leaderboard", new Vector2(285, 435), fonty, Color.Black);
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            score.text = "Score: " + GameHelper.score;

            if (keyState.IsKeyDown(Keys.Space) && !prevKeyState.IsKeyDown(Keys.Space))
                gameStateManager.setLevel(7);

            if (keyState.IsKeyDown(Keys.B) && !prevKeyState.IsKeyDown(Keys.B))
                gameStateManager.setLevel(0);
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            youWin.Draw(spriteBatch);
            score.Draw(spriteBatch);
            gotoLeaderboard.Draw(spriteBatch);
        }
    }
    class GameLevel_Fail : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        TextRenderable score = null;
        TextRenderable gotoLeaderboard = null;
        SpriteFont fonty;

        Texture2D texYouFail = null;

        ImageBackground youFail = null;
        public override void EnterLevel(int fromLevelNum)
        {
            LeaderBoard.updateLeaderboard(GameHelper.score);
        }
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            texYouFail = Util.texFromFile(graphicsDevice, Dir.art_dir + "lose.png");
            youFail = new ImageBackground(texYouFail, new Rectangle(0, 0, 565, 253), new Rectangle(110, 80, 565, 253), Color.White);

            fonty = Content.Load<SpriteFont>("SpriteFont1");
            score = new TextRenderable("Your Score: " + GameHelper.score, new Vector2(335, 405), fonty, Color.Red);
            gotoLeaderboard = new TextRenderable("Press Space to Leaderboard", new Vector2(285, 485), fonty, Color.Black);
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            score.text = "Score: " + GameHelper.score;

            if (keyState.IsKeyDown(Keys.Space) && !prevKeyState.IsKeyDown(Keys.Space))
                gameStateManager.setLevel(7);

            if (keyState.IsKeyDown(Keys.B) && !prevKeyState.IsKeyDown(Keys.B))
                gameStateManager.setLevel(0);
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            youFail.Draw(spriteBatch);
            score.Draw(spriteBatch);
            gotoLeaderboard.Draw(spriteBatch);
        }
    }
    class GameLevel_LeaderBoard : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        SpriteFont fonty;

        Texture2D texLBBg = null;
        RC_RenderableList scores = new RC_RenderableList();

        ImageBackground lbBg;
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            fonty = Content.Load<SpriteFont>("SpriteFont1");

            texLBBg = Util.texFromFile(graphicsDevice, Dir.art_dir + "leaderboard_bg.png");

            lbBg = new ImageBackground(texLBBg, new Rectangle(0, 0, 1000, 1000), new Rectangle(150, 100, 500, 500), Color.White);

            TextRenderable score;
            for (int i = 0; i < 6; i++)
            {
                score = new TextRenderable("0", new Vector2(380, 110 + (i + 1) * 75), fonty, Color.White);
                scores.addReuse(score);
            }


        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            TextRenderable score;
            string[] lines = LeaderBoard.getLeaderboard();

            int i = 0;
            foreach (string line in lines)
            {
                scores.getRenderable(i).text = line;
                i++;
            }

            if (keyState.IsKeyDown(Keys.B) && !prevKeyState.IsKeyDown(Keys.B))
                gameStateManager.setLevel(0);
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            lbBg.Draw(spriteBatch);
            scores.Draw(spriteBatch);
        }
    }
    class GameLevel_LevelChoose : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        SpriteFont fonty;

        Texture2D texLevelT = null;
        RC_RenderableList levels;

        ImageBackground levelT;
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            fonty = Content.Load<SpriteFont>("SpriteFont1");

            texLevelT = Util.texFromFile(graphicsDevice, Dir.art_dir + "Level.png");

            levelT = new ImageBackground(texLevelT, new Rectangle(0, 0, 1200, 150), new Rectangle(100, 100, 600, 75), Color.White);
            
            levels = new RC_RenderableList();
            TextRenderable level = new TextRenderable("1 - " + GameHelper.T_Level1, new Vector2(270, 235), fonty, Color.Black);
            levels.addReuse(level);
            level = new TextRenderable("2 - " + GameHelper.T_Level2, new Vector2(270, 295), fonty, Color.Black);
            levels.addReuse(level);
            level = new TextRenderable("3 - " + GameHelper.T_Level3, new Vector2(270, 355), fonty, Color.Black);
            levels.addReuse(level);
            level = new TextRenderable("4 - " + GameHelper.T_Level4, new Vector2(270, 415), fonty, Color.Black);
            levels.addReuse(level);
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            if (keyState.IsKeyDown(Keys.B) && !prevKeyState.IsKeyDown(Keys.B))
                gameStateManager.setLevel(0);
            if (keyState.IsKeyDown(Keys.D1) && !prevKeyState.IsKeyDown(Keys.D1))
            {
                gameStateManager.getLevel(1).InitializeLevel(graphicsDevice, spriteBatch, Content, gameStateManager);
                gameStateManager.getLevel(1).LoadContent();
                gameStateManager.setLevel(1);
            }
                
            if (keyState.IsKeyDown(Keys.D2) && !prevKeyState.IsKeyDown(Keys.D2))
                gameStateManager.setLevel(2);
            if (keyState.IsKeyDown(Keys.D3) && !prevKeyState.IsKeyDown(Keys.D3))
                gameStateManager.setLevel(3);
            if (keyState.IsKeyDown(Keys.D4) && !prevKeyState.IsKeyDown(Keys.D4))
                gameStateManager.setLevel(4);
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            levelT.Draw(spriteBatch);
            levels.Draw(spriteBatch);
        }
    }
    class GameLevel_Help : RC_GameStateParent
    {
        Rectangle screenRect;
        Texture2D texBackground = null;
        ScrollBackGround background = null;

        SpriteFont fonty;
        TextRenderable title = null;

        Texture2D texHelp = null;
        ImageBackground help;
        public override void LoadContent()
        {
            screenRect = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            texBackground = Util.texFromFile(graphicsDevice, Dir.art_dir + "bg001.png");
            background = new ScrollBackGround(texBackground, new Rectangle(0, 0, 800, 700), screenRect, 1.0f, 1);

            fonty = Content.Load<SpriteFont>("SpriteFont1");
            title = new TextRenderable("Game Help", new Vector2(350, 50), fonty, Color.Black);

            texHelp = Util.texFromFile(graphicsDevice, Dir.art_dir + "help.png");
            help = new ImageBackground(texHelp, new Rectangle(0, 0, 500, 500), new Rectangle(150, 100, 500, 500), Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            if (keyState.IsKeyDown(Keys.B) && !prevKeyState.IsKeyDown(Keys.B))
                gameStateManager.popLevel();
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            help.Draw(spriteBatch);
            title.Draw(spriteBatch);
        }
    }
    class LeaderBoard
    {
        static List<int> scores = new List<int> { };
        public static string[] getLeaderboard()
        {
            string path = Dir.source_dir + "hiscore.txt";

            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            setScores(lines);
            return lines;
        }

        public static void updateLeaderboard(int newScore)
        {
            scores.Add(newScore);
            scores.Sort();
            scores.Reverse();

            saveScores();
        }

        private static void setScores(string[] lines)
        {
            scores.Clear();
            foreach (string line in lines)
            {
                scores.Add(Int32.Parse(line));
            }
        }

        private static void saveScores()
        {
            string path = Dir.source_dir + "hiscore.txt";

            FileStream file = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(file);

            string strings = "";
            for (int i = 0; i < 6; i++)
            {
                strings += scores[i].ToString() + "\r\n";
            }
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            file.Close();
        }
    }
}
