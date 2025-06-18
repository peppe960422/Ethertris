using Ethernetris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;


namespace Ethertris
{

    internal class GameWindow : GameObj
    {
  

        public Button[] buttons {  get; set; }

        SpriteFont spriteFont { get; set; }

        private int _layout;

        public Vector2 Layout 
        {
            get
            { 
                if (_layout == 0) { return new Vector2(Position.X +this.Width / 5, Position.Y +(Height / 2)-100); } 
                else if (_layout == 1) { return new Vector2(Position.X +Width / 2,Position.Y+ (Height / 2)-100); } 
                else { return new Vector2(Position.X +this.Width - this.Width / 5,Position.Y + (Height / 2)-100); }
            }
           
        }

        string Message { get; set; }
        public GameWindow(Point p , Texture2D[] frms , SpriteFont font , Button[] buttons , int Layout ,string message) : base ( p , frms )
        {
            this.Width = 600;
            this.Height = 400;  
            _layout = Layout;   
            this.Message = message;
            this.buttons = buttons;
            spriteFont = font;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Position = new Point (p.X + (Width - ((i+2)*150) ), p.Y + (Height -200)) ;
            
            
            
            }

        }

        public override void Draw(SpriteBatch spriteBatch, int Frame)
        {
            spriteBatch.Draw(texture: ObjFrames[Frame], destinationRectangle: new Rectangle(Position.X, Position.Y, Width, Height), color: Color.White);
            spriteBatch.DrawString(spriteFont, Message, Layout, Color.Red);
            foreach (Button button in buttons) { button.Draw(spriteBatch, 0); }
        }
    }


    internal class Button : GameObj
    {

        int Frame { get; set; } = 0;

        public  delegate void ButtonAction ();

        
      
        private int delay  =  5;

        public int MyProperty
        {
            get { if (delay < 0) { return 5; } else { return delay;  } ; }
            set { delay = value; }
        }




        bool iVeBeenClicked = false;
 

        public ButtonAction Action { get; set; }

        SpriteFont spriteFont {  get; set; } 
        
        string Message { get; set; }    
        public Button(Point pos, Texture2D[] Frames, ButtonAction action,string Message,SpriteFont font) : base(pos, Frames)
        {
            this.Action = action;   
            this.Message = Message;
            this.spriteFont = font;
            this.Width = 120 ; this.Height = 60;
        }

        public override void Draw(SpriteBatch spriteBatch, int Frame)
        {
            spriteBatch.Draw(texture: ObjFrames[this.Frame], destinationRectangle: new Rectangle(Position.X, Position.Y, Width, Height), color: Color.White);
            if (this.Frame != 2)
            {
                spriteBatch.DrawString(spriteFont, Message, new Vector2(this.Position.X + 8, this.Position.Y + 10), Color.White);
            }
            else { spriteBatch.DrawString(spriteFont, Message, new Vector2(this.Position.X +  8, this.Position.Y + 15), Color.White); }
               


        }

        public void Update(Point MousePosition, ButtonState input) 
        {

            if (this.Rectangle.Contains(MousePosition))
            {
                Frame = 1;
                if (input == ButtonState.Pressed)
                {
                    Frame = 2;

                   
                    iVeBeenClicked = true;


                }







            }
            else { Frame = 0; }

            if (iVeBeenClicked) { delay--; }

            if (delay == 0) { Action.Invoke(); }
        
        
        
        
        }
    }

}
