using Ethertris;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ethernetris
{
    public class Game1 : Game
    {

        SoundEffect PointSound;
        SoundEffect PutSound;
        SoundEffect ElectricSound;

        Protocoll lastSelectedProtocoll = null;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D GohstTexture {  get; set; }   
        PointCounter _pointCounter {  get; set; }  
        Texture2D Background {  get; set; } 
        //List<OSILayer> OsiModell = new List<OSILayer>();
        List<Soket> sokets = new List<Soket>(); 
        List<Protocoll> protocols = new List<Protocoll>();
        FrameMngr frameManager = new FrameMngr();   
        Rectangle Gohst { get; set; }

        Rectangle[] DeadFields = { new Rectangle(0, 0, 1000, 100), new Rectangle(0, 800, 1000, 300), new Rectangle(1900 - 345, 0, 400, 1000) , new Rectangle (0,0,50,1000)}; 
        OSIModell OsiModell { get; set; }
        int Frame = 0;
        Ethertris.GameWindow _gameWindow { get; set; }

        private MouseState PreviousM;
        private MouseState CurrentM;

        int YGround = 900; 

        uint Counter = 0;    

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public Texture2D[][,] InitzializeProtocolAsset() 
        {
            Texture2D[][,] assets = new Texture2D[3][,]; 
            for (int i = 0; i < 3; i++) 
            {
                assets[i] = new Texture2D[3,4];
                for (int j = 0; j < 3; j++) 
                {

                    for (int k = 0; k < 4; k++)
                    {
                        if (j !=0 ) { assets[i][j, k] = Content.Load<Texture2D>($"protocoll-{i}-{j}-{k}");
                            

                     }


                      else
                        { 
                            assets[i][j, k] = Content.Load<Texture2D>($"protocoll-{i}-{j}-0"); }
                      
                    
                    }
                
                
                }
                



            }
            return assets;
        
        
        }

        private void LoadSound() 
        {

            PointSound = Content.Load<SoundEffect>("SoundEffects/PointSound");
            PutSound = Content.Load<SoundEffect>("SoundEffects/PutSound");
            ElectricSound = Content.Load<SoundEffect>("SoundEffects/ElectricSound");
        
        
        
        
        }
        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferWidth = 1900;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
        }

        private void CloseButtonAction() { this.Exit();/*System.Diagnostics.Debug.WriteLine("ivh wurde gecklict");*/ }
        private void NewGameButtonAction()
        {
           protocols.Clear();
            Counter = 0;
            _pointCounter.Points = 0;
            _pointCounter.Errors = 0;
            
            for (int i = 0; i < OsiModell.Packets.Length; i++) 
            { 
            
                for(int j = 0;j < OsiModell.Packets[i].Length; j++) 
                {
                    OsiModell.Packets[i][j] = null; 
                
                }
            
            }
            Protocoll.IveLost = false;

        }

        private void InitGameWindowTest(Texture2D [] buttonframes, Texture2D[] frmsWindow ,SpriteFont font,SpriteFont windowFnt ) 
        { 
            Button[] buttons = { new Button(new Point(0, 0), buttonframes, CloseButtonAction, "Exit", font) , new Button(new Point(0, 0), buttonframes, NewGameButtonAction, "NewGame", font) };


            _gameWindow = new Ethertris.GameWindow(new Point(300, 300), frmsWindow, windowFnt, buttons, 0, "Game Over!!!");
        
        
        
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadSound();
            GameObjTextureInitializer textureInitializer = new GameObjTextureInitializer();

            GohstTexture = textureInitializer.GohstTexture(GraphicsDevice, 100, 100, Color.Red);
            SpriteFont font = Content.Load<SpriteFont>("ProtocollFont");
            SpriteFont bigFont = Content.Load<SpriteFont>("DialogFont");
            //for (int i = 0; i < 7; i++) 
            //{
            //    Texture2D[] frms = { textureInitializer.CreateRectangleWithBorder(GraphicsDevice, 1000, 100, 5, Color.Red, Color.Transparent) };
            //    OSILayer layer = new OSILayer (new Point(50 , 100*(i+1) ),frms,(ushort)(i+1));
            //    OsiModell.Add(layer);   


            //}

          
            Texture2D[] frmsOSI = new Texture2D[7];

            Background = Content.Load<Texture2D>("Background");
            Texture2D[] pointZehelerTexture = new Texture2D[1];
                pointZehelerTexture[0] =  Content.Load<Texture2D>("Punktzehler0") ;
            _pointCounter = new PointCounter(new Point (  1900 - 345, 1000 - 345),pointZehelerTexture, font);
            
            for (int i = 0; i < frmsOSI.Length; i++ ) { frmsOSI[i] = Content.Load<Texture2D>("Layer0");/*textureInitializer.CreateRectangleWithBorder(GraphicsDevice, 1000, 100, 5, Color.Red, Color.Transparent); */};
            OSIModell modell = new OSIModell(new Point(50, 800), frmsOSI,_pointCounter,bigFont);
            OsiModell = modell; 


            Texture2D[][,] assets = InitzializeProtocolAsset();
            for (int i = 0; i < 4; i++) {
                Texture2D[] frms = { Content.Load<Texture2D>("Socket") };
                Soket s = new Soket(new Point((80 * (i + 1)) + 1100, 50), frms, assets,font); 
                sokets.Add(s);
                  
            
            
            }

            Texture2D[] buttonTxtr = new Texture2D[3];
            for ( int i = 0;i < buttonTxtr.Length;i++ ) { buttonTxtr[i] = Content.Load<Texture2D>("Button" + i ); }
            Texture2D[] WindowTxture = { Content.Load<Texture2D>("MenuWindow") };
            InitGameWindowTest(buttonTxtr, WindowTxture,font,bigFont);


        }




        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Counter++;
            Frame = frameManager.GimmieDaFrame(Counter);  
            CurrentM = Mouse.GetState();
            if (_pointCounter.Errors >= 5 || Protocoll.IveLost ) 
            { 
                foreach (Button b in _gameWindow.buttons) { b.Update(new Point(CurrentM.X, CurrentM.Y), CurrentM.LeftButton); } 
            }
            else 
            { 
                foreach (Soket s in sokets) 
                {
                    Protocoll c =  s.CreateProtocoll(Counter) ;

                    if (c != null) {protocols.Add(c);}  
            
                }
                Parallel.ForEach(protocols, p =>
                {
                    if (p.DeadCounter == 0) { protocols = protocols.Where(x => x.DeadCounter > 0).ToList(); }
              
                
                    p.Update(CurrentM, protocols, YGround);
                });

                Parallel.ForEach(protocols, p =>
                {
                 
                    if (lastSelectedProtocoll != null)
                    {

                        if (lastSelectedProtocoll.id != p.id && !p.AmInStack)
                        {
                            if (p.IContainPolygon(lastSelectedProtocoll.intersRect.Vertexes)) { { lastSelectedProtocoll.Position.Y = p.Position.Y - lastSelectedProtocoll.Height - 1; } }
                        }

                    }

                
                });




                if (Protocoll.selectedProtocoll != null/* && CurrentM.LeftButton == ButtonState.Released*/) 
            {
                int point = int.MaxValue;
               
                    foreach ( OSILayer o in OsiModell.Levels) 

                {
                         
                        if (CurrentM.LeftButton == ButtonState.Pressed) 
                    { 
                        Rectangle t_Gohst =  o.UpdateGohst(Protocoll.selectedProtocoll);
                        if (t_Gohst.Width > 0) { Gohst = t_Gohst; }
                    
                    }
                   
                        else if (CurrentM.LeftButton == ButtonState.Released) 
                    { 
                        point = o.Update(Protocoll.selectedProtocoll,DeadFields); Gohst = new Rectangle(0, 0, 0, 0);
                     
                        

                    }

                    if (point > 0 && point < int.MaxValue) { _pointCounter.AddPoint();PointSound.Play(); break; }
                    else if (point < 0 ) { _pointCounter.AddError(); ElectricSound.Play();  break; }
                    else if (point == 0) { PutSound.Play();break; }

                            
                    }
              

            
            }  
              if (CurrentM.LeftButton == ButtonState.Released) { lastSelectedProtocoll = Protocoll.selectedProtocoll; Protocoll.selectedProtocoll = null; }

           }

            // TODO: Add your update logic here
            PreviousM = CurrentM;   

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(texture: Background, destinationRectangle: new Rectangle(0,0,1900,1000), Color.White);
        

            foreach ( OSILayer o in OsiModell.Levels) { o.Draw(_spriteBatch, 0); }    
            if (Gohst.Y > 0) 
            { 
                _spriteBatch.Draw(GohstTexture,Gohst,Color.White);
            
            
            }

            foreach ( Soket s in sokets) { s.Draw(_spriteBatch, 0); }    

            foreach (Protocoll x  in protocols) {  x.Draw(_spriteBatch, Frame); }

            _pointCounter.Draw(_spriteBatch,0);
             

            if ((_pointCounter.Errors >= 5 || Protocoll.IveLost) ) { _gameWindow.Draw(_spriteBatch, 0); }


            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }


    public class FrameMngr 
    { 
    
     public int GimmieDaFrame (uint Counter) 
        {

            if (Counter % 4 == 0) { return 0; }
            else { return (int)Counter % 4;  }
        
        
        
        }
    
    
    
    
    
    }
}
