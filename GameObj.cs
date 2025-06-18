using Ethernetris;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Ethernetris
{
   

    
    public abstract class GameObj
    {

        public Point Position;

  

        public Point MiddlePosition 
        {
            get { return new Point(this.Position.X + Width/2,this.Position.Y + Height/2 ) ; }
        
        }

        public Texture2D[] ObjFrames;
        public int Width;
        public int Height;  

       

        public Rectangle Rectangle
        {
            get { return new Rectangle(Position.X, Position.Y ,Width,Height); }
            
        }


        public GameObj(Point pos , Texture2D[] Frames) 
        { 
            Position = pos;

            ObjFrames = Frames;
        
        }

        public abstract void Draw(SpriteBatch spriteBatch,int Frame); 


    }

 
    }


    public class Protocoll : GameObj 
    {
    
        static int Counter = 0; 
        static readonly int MaximalHohe = 140;

        Color ColorFont = Color.FromNonPremultiplied(56, 185, 71, 255);

        public int id { get; set; } 
        public static bool IveLost = false ; 


         public int DeadCounter = 50; 

       public  bool ImStreched = false ;   
        public string Name {  get; set; }   

        public static Protocoll selectedProtocoll = null;

        public bool SadlyIHaveToDie = false;    
        public int IndexI {  get; set; }

        public uint ProtocollPrimeNumber;

        public bool IMakeAPoint = false ;   

        public bool AmInStack {  get; set; }    

        public SpriteFont SpriteFont;

  
    public DynamicRectangleWrapper intersRect  
    {
        get { return new DynamicRectangleWrapper(this.Rectangle); }
        
    }






    public Texture2D[][,] protocollFrames {  get; set; }    
        public ushort[] OSIlvl
        {
            get {
                if (IndexI == 0) { return new ushort[] { 1, 2 }; }
                else if (IndexI == 1) { return new ushort[] { 3 }; }
                else if (IndexI == 2) { return new ushort[] { 4 }; }
                else if (IndexI == 3) { return new ushort[] { 5, 6, 7 }; }
                else
                {
                    return new ushort[] {0 };
                }
                }
                
        }


        public Color ProtocolColor = Color.White    ;

       

        const int Velocity = 1;



        public Protocoll(Point  p , Texture2D[] frames  , uint PrimeNumber ,int index, string Name ,SpriteFont f, Texture2D[][,] protocollFrame) : base (p,frames)
        {

            IndexI = index;
            ProtocollPrimeNumber = PrimeNumber;
            Width = 80;
            Height = 100;
            this.SpriteFont = f;
            AmInStack = false;
             this.protocollFrames = protocollFrame; 
            this.Name = Name;
        this.id = Counter;
        Counter++;
     




        }

        public bool IContainPolygon(Point []vertexs) 
        { 
            foreach (Point p in vertexs) 
            { 
            if ( this.Rectangle.Contains(p) )
                return true;
        
            }
        return false;
    
         } 
        public void Update(MouseState CurrentM, List<Protocoll>  protocolls, int GroundY)  
        {
            if ( !AmInStack)
            {
                
          
            //System.Diagnostics.Debug.WriteLine(this.Name + "----" +Position.Y);
         
                if (CurrentM.LeftButton == ButtonState.Pressed  /*&& AmIAusgewhaelt(CurrentM)*/) 
                {

                    if (AmIAusgewhaelt(CurrentM) && selectedProtocoll == null)
                    {
                        selectedProtocoll = this; 
                    }

                    if ( selectedProtocoll == this )
                    {
                   
                            this.Position = new Point(CurrentM.X, CurrentM.Y);
                    
                    }
                
                } 
                if (!Collide(protocolls) && this.Position.Y <= GroundY )
                  
                {
                    this.Position.Y += Velocity; 
                }
                else 
                {
                       if (this.Position.Y < MaximalHohe && selectedProtocoll == null)    
                 { 
                     IveLost = true; 
                 }
                
                }
            
            }
           

        }

        public bool Collide(List<Protocoll> Protocolls)
        {
            foreach (var p in Protocolls)
            {
                if (!p.Equals(this) && p.Position.Y > this.Rectangle.Y) 
                { 
                if (Intersect(this.Rectangle, p.Rectangle))
                {
                    return true;
                } 
                }
            }
            return false;

        }
        public bool Intersect(Rectangle rect1, Rectangle rect2)
        {

            return rect1.Left < rect2.Right &&
                   rect1.Right > rect2.Left &&
                   rect1.Top < rect2.Bottom &&
                   rect1.Bottom > rect2.Top -1/*+12*/;
        }



        public bool AmIAusgewhaelt(MouseState m) 
        {

            if ((m.X <= (this.Position.X + this.Rectangle.Width) && m.X >= this.Position.X) && 
                (m.Y >= this.Position.Y && m.Y <= this.Position.Y + this.Rectangle.Height)) 
            { 
            
                return true ;   
            
            }   

            return false ;  



        }
        public override void Draw(SpriteBatch spriteBatch, int Frame)
        {
            int strechint = OSIlvl.Length-1;  
            int state = 0;
        if (SadlyIHaveToDie)
        {
            state = 1; 
            DeadCounter--;
        }
        else if ( IMakeAPoint )
        {
            state = 2;
            DeadCounter--;
        }

        if (DeadCounter < 50) {
            if (state == 2
                ) { this.Position.Y -= 10;  }
      if (ImStreched)
        {
            spriteBatch.Draw(texture: protocollFrames[strechint][state, Frame], destinationRectangle: new Rectangle(Position.X, Position.Y, 80, Height), color: Color.White);
            spriteBatch.DrawString(SpriteFont, Name, new Vector2(this.Position.X + 5, this.Position.Y + 40), Color.Red);
        }
        else {    spriteBatch.Draw(texture: protocollFrames[0][state,Frame], destinationRectangle: new Rectangle(Position.X, Position.Y, 80, Height), color: Color.White);
             spriteBatch.DrawString(SpriteFont, Name, new Vector2(this.Position.X + 5, this.Position.Y + 40), Color.Red);
             }
     
         }
        else 
        {

            if (ImStreched)
            {
                spriteBatch.Draw(texture: protocollFrames[strechint][state, 0], destinationRectangle: new Rectangle(Position.X, Position.Y, 80, Height), color: Color.White);
                spriteBatch.DrawString(SpriteFont, Name, new Vector2(this.Position.X + 5, this.Position.Y + 40), ColorFont);
            }
            else
            {
                spriteBatch.Draw(texture: protocollFrames[0][state, 0], destinationRectangle: new Rectangle(Position.X, Position.Y, 80, Height), color: Color.White);
                spriteBatch.DrawString(SpriteFont, Name, new Vector2(this.Position.X + 5, this.Position.Y + 40), ColorFont);
            }

        }




    }

           
  
    }
    

        

   
    public class ProtocollFactory 
    {
        Random Random;

        Texture2D[][,] Texture;

        SpriteFont SpriteFont;  

        public ProtocollFactory(Texture2D[][,] frameTxt,SpriteFont f)
        {
                Random = new Random();
            Texture = frameTxt;
            this.SpriteFont = f;
        }


        public Protocoll Create(Point OriginPoint) 
        { 
        
         int Frame = Random.Next(0,15);
            int Protocoll = Random.Next(0, 4);

            return new Protocoll(OriginPoint, null , 
                        EthFramesSolutions.ProtokollUintCode[Frame][Protocoll],
                        Protocoll,
                        EthFramesSolutions.ProtokollNames[Frame][Protocoll], SpriteFont,Texture);
        
        
        
        }   



    }


    public static class EthFramesSolutions 
    {



    public static readonly string[][] ProtokollNames = new string[][]
    {
    new string[] { "Ethernet", "IPv4", "TCP", "HTTP" },
    new string[] { "Ethernet", "IPv4", "UDP", "DNS" },
    new string[] { "Ethernet", "IPv6", "TCP", "HTTPS" },
    new string[] { "Ethernet", "IPv4", "UDP", "DHCP" },
    new string[] { "Ethernet", "IPv4", "TCP", "FTP" },
    new string[] { "Ethernet", "IPv4", "TCP", "SMTP" },
    new string[] { "Ethernet", "IPv6", "UDP", "SNMP" },
    new string[] { "Ethernet", "IPv6", "TCP", "SSH" },
    new string[] { "Ethernet", "IPv4", "TCP", "POP3" },
    new string[] { "Ethernet", "IPv4", "TCP", "IMAP" },
    new string[] { "Ethernet", "IPv6", "TCP", "Telnet" },
    new string[] { "Ethernet", "IPv6", "UDP", "RTP" },
    new string[] { "Ethernet", "IPv6", "UDP", "NTP" },
    new string[] { "Ethernet", "IPv4", "TCP", "SNTP" },
    new string[] { "Ethernet", "IPv4", "UDP", "TFTP" },
    new string[] { "Ethernet", "IPv6", "TCP", "MIME" }
    };

    public static readonly uint[][] ProtokollUintCode = new uint[][]
    {
    new uint[] { 3, 5, 11, 17 },
    new uint[] { 3, 5, 13, 19 },
    new uint[] { 3, 5, 11, 23 },
    new uint[] { 3, 5, 13, 29 },
    new uint[] { 3, 5, 11, 31 },
    new uint[] { 3, 5, 11, 37 },
    new uint[] { 3, 5, 13, 41 },
    new uint[] { 3, 5, 11, 43 },
    new uint[] { 3, 5, 11, 47 },
    new uint[] { 3, 5, 11, 53 },
    new uint[] { 3, 5, 11, 59 },
    new uint[] { 3, 5, 13, 61 },
    new uint[] { 3, 5, 13, 67 },
    new uint[] { 3, 5, 11, 71 },
    new uint[] { 3, 5, 13, 73 },
    new uint[] { 3, 5, 11, 79 }
    };
    public static readonly uint[] FrameSolutions = new uint[ProtokollUintCode.Length];

    static EthFramesSolutions() 
    {
        for (int i = 0; i < ProtokollUintCode.Length; i++)
        {
            FrameSolutions[i] = CalculateSolution(ProtokollUintCode[i]);
        }
    }


        static uint CalculateSolution(uint[] protocolsCode) 
        {
            uint solution = 1;

            for (int i = 0; i < protocolsCode.Length; i++) 
            { 
            
                 solution = solution * protocolsCode[i];    
            
            }
        
            return solution;    
        }


    }

    public class Soket : GameObj 
    {
        ProtocollFactory protocollFactory;
        int id;
        static int Counter = 1 ;
        SpriteFont spriteFont;
        public Soket(Point pos , Texture2D[] frames, Texture2D[][,] protocollFrames ,SpriteFont f  ) : base(pos ,frames) 
        {
            this.Height = 89;
            this.Width = 80;
            spriteFont = f;
            protocollFactory = new ProtocollFactory(protocollFrames,spriteFont );
            id = Counter++ ;    
                
        }

        public override void Draw(SpriteBatch spriteBatch, int Frame)
        {

            spriteBatch.Draw(ObjFrames[0], Rectangle, Color.White
                );
        }

        public Protocoll CreateProtocoll(uint Counter) 
        {

            if (Counter % ((300-id)*id) == 0) 
            {

               return protocollFactory.Create(this.Position);
            
            
            }
            else
            {
                 return null;
            }



        }
    }


    public class OSIModell : GameObj 
    {
        public OSILayer[] Levels = new OSILayer[7];


    //public List<Protocoll>[] Packets = new List<Protocoll>[4];
    //
    public PointCounter PointCounter { get; set; }
    public Protocoll[][] Packets = new Protocoll[4][]; 

       

        public OSIModell(Point p, Texture2D[] Frames,PointCounter pointCounter,SpriteFont font) : base(p, Frames) 
        {
            for( int i = 0; i < Packets.Length; i++) { Packets[i] = new Protocoll[4]; /*new List<Protocoll>();*/ }
            PointCounter = pointCounter;   
       
        
           for (int i = 0; i < 7; i++) 
            {

                Texture2D[] frms = new Texture2D[1];
                frms[0] = Frames[i];    
                OSILayer layer = new OSILayer (new Point(p.X ,p.Y - (100*(i+1)) ),frms,(ushort)(i+1),this,pointCounter,font);
                Levels[i] = layer;   
            
            
            }
        
        }

        public override void Draw(SpriteBatch spriteBatch, int Frame)
        {
            foreach ( OSILayer layer in Levels) {  spriteBatch.Draw(ObjFrames[0], Rectangle, Color.White
                );}

           
        }


        public int CheckIPFrames() 
        {

        foreach (/*List<Protocoll>*/ Protocoll[] frame in Packets) 
        { 
        
           if ( !CheckFrame(frame)) 
            { 
            
            foreach (Protocoll protocoll in frame)
                { 
                    if (protocoll != null) 
                    { protocoll.SadlyIHaveToDie = true; } }   
            //frame.Clear();
            
            for(int i = 0;i < frame.Length; i++) { frame[i] = null; }

                return -1;
            
            }
            
        
        
        }
    
        return 0;    
    
    
        }
        

            public bool CheckFrame(/*List<Protocoll>*/ Protocoll[] frame) 
        {
            bool[] corrects = new bool[EthFramesSolutions.FrameSolutions.Length]; 
      
            for(int i =0; i < EthFramesSolutions.FrameSolutions.Length; i ++) 
             { 
          
            corrects[i] = CheckSolution(frame, EthFramesSolutions.FrameSolutions[i]);
            
             }

            foreach( bool correct in corrects) 
                { 
        
                    if (correct) { return true; }
                }

            return false;   
        }


        public bool CheckSolution(/*List<Protocoll>*/Protocoll[] protocolls , uint solution) 
        {
            foreach (Protocoll p in protocolls) 
            { 
            if (p != null) 
            {
                if ( solution %  p.ProtocollPrimeNumber != 0) { return  false;  }   
            }  
                
        
            }
            return true ;
    
    
        }
    }
public class OSILayer : GameObj
{

    public ushort OsiLvl;

    public Color layerColor;

    static readonly int[] ProtocollPosition = { 200, 400, 600, 800 };
     OSIModell Modell = null;
    public PointCounter Pointcounter {  get; set; }

    public SpriteFont SpriteFont { get; set; }

    Vector2 namePosition { get { return  new Vector2(this.Position.X + (this.Width -50), this.Position.Y +30 ); } } 


    public OSILayer(Point pos, Texture2D[] frames, ushort OsiLvl, OSIModell modell, PointCounter pointCounter,SpriteFont font) : base(pos, frames)
    {
        this.OsiLvl = OsiLvl;
        this.Width = 1000;
        this.Height = 100;
        this.Pointcounter = pointCounter;
        this.SpriteFont = font; 
        if (Modell == null)
        {
            Modell = modell;

        }


        this.Modell.Levels[OsiLvl - 1] = this;

    }

    public Rectangle UpdateGohst(Protocoll protocoll)
    {
        foreach (int i in ProtocollPosition)
        {
            int X = i + this.Position.X;

            if (X - protocoll.Position.X < 100 && X - protocoll.Position.X > -100 && this.Position.Y <= protocoll.Position.Y + this.Height / 2 && protocoll.Position.Y + this.Height / 2 <= this.Position.Y + this.Height)
            {
                
                return new Rectangle(X, Position.Y , 100,100);    

            }
           
        }
        return new Rectangle(0,0,0,0);
    }

    public bool ImInDeathBereich(Rectangle[] rects , Point p ) 
    {

        foreach (Rectangle r in rects) 
        { 
        
        if (r.Contains(p)) {  return true; }    
        
        }
        return false;   
    
    }

    public int Update(Protocoll protocoll, Rectangle[] DeadFields)
    {
        int j = 0;  
       int point = int.MaxValue;
        if (ImInDeathBereich(DeadFields, protocoll.Position)) { protocoll.SadlyIHaveToDie = true; return -1; }

        foreach (int i in ProtocollPosition)
        {
            int X = i + this.Position.X; // devo passare il MouseState?? e`possibile che aggiunge il protocollo anche quando lo sto spostando  

            if (X - protocoll.Position.X < 100 && X - protocoll.Position.X > -100 && this.Position.Y <= protocoll.Position.Y + this.Height / 2 && protocoll.Position.Y + this.Height / 2 <= this.Position.Y + this.Height)
            {
                if (this.Modell.Packets[j][protocoll.IndexI] == null && ImOnThisLvl(protocoll.OSIlvl)  ) 
                {
                    point = 0;
                    this.Modell.Packets[j][protocoll.IndexI] = protocoll;
                     protocoll.Position.X = X;
               
                    if (protocoll.OSIlvl.Length == 2) { protocoll.Position.Y = Modell.Levels[1].Position.Y; }
                    else if (protocoll.OSIlvl.Length == 3) { protocoll.Position.Y = Modell.Levels[6].Position.Y; }
                    else { protocoll.Position.Y = this.Position.Y;  }
                    protocoll.AmInStack = true;
                    if (!protocoll.ImStreched) {  protocoll.Height *=  protocoll.OSIlvl.Length; protocoll.ImStreched = true; }


                    //this.Modell.Packets[j].Add (protocoll);
                    
                     point = Modell.CheckIPFrames();
                    if (DoImakeAPoint(this.Modell.Packets[j])) 
                    { 
                       
                        for (int k = 0; k < this.Modell.Packets[j].Length;k++) 
                        { 
                            Modell.Packets[j][k].IMakeAPoint = true; 
                           // Modell.Packets[j][k] = null; 
                        }

                        point++;
                        
                    }
                 }
                else 
                {
                   
                        
                        protocoll.SadlyIHaveToDie = true;
                    point = -1;
                        
                     
                }
            
            }
            
            j++;

        }
        for (int k = 0; k < this.Modell.Packets.Length; k++)
        {
           
            for (int f = 0; f < this.Modell.Packets[k].Length; f++)
            {
                if (Modell.Packets[k][f] != null  && (Modell.Packets[k][f].SadlyIHaveToDie || Modell.Packets[k][f].IMakeAPoint))
                {
                    Modell.Packets[k][f] = null;
                    
                }
              

            }


        }
       
        return point;

       //for (int k = 0; k < this.Modell.Packets.Length; k++) 
       //{
       //    System.Diagnostics.Debug.WriteLine("");
       //    for (int f = 0; f < this.Modell.Packets[k].Length; f++) 
       //    {
       //        if (Modell.Packets[k][f]!= null) { 
       //       System.Diagnostics.Debug.Write(Modell.Packets[k][f].Name+ " ");}
       //        else { System.Diagnostics.Debug.Write("X "); }

       //   }
        
        
       // }
       // System.Diagnostics.Debug.WriteLine("________________________________");
    



}

    public bool DoImakeAPoint(Protocoll[] Frame) 
    {

        for (int i = 0; i < Frame.Length; i++) 
        {

            if (Frame[i] == null) { return false; }

           
        
        
        }

        return true; 
    
    
    } 

 

    public bool ImOnThisLvl  (ushort [] Osilvl) 
    { 
       for( int i = 0; i < Osilvl.Length; i++) 
        { 
            if (  Osilvl[i] == this.OsiLvl) 
            { 
                return true;    
            
            }
        
        
        }
    
    
        return false;
    }

    public override void Draw(SpriteBatch spriteBatch, int Frame)
    {
        spriteBatch.Draw(texture: ObjFrames[Frame], destinationRectangle: Rectangle, Color.White);
        spriteBatch.DrawString(this.SpriteFont, this.OsiLvl.ToString(), namePosition, Color.Red);


    }






}
 public class PointCounter : GameObj 
{
    public short Points { get; set; } = 0;

    public short Errors { get; set; } = 0;
    SpriteFont SpriteFont { get; set; } 

    public PointCounter(Point pos , Texture2D[] frame  , SpriteFont f ) : base (pos, frame) { SpriteFont = f;  }
    
        public override void Draw(SpriteBatch spriteBatch, int Frame)
    {
        {
            spriteBatch.Draw(texture: ObjFrames[Frame], destinationRectangle: new Rectangle(Position.X, Position.Y, 345, 345), color: Color.White);
            spriteBatch.DrawString(SpriteFont, Points.ToString(), new Vector2(this.Position.X + 70, this.Position.Y + 120), Color.White);
            spriteBatch.DrawString(SpriteFont, Errors.ToString(), new Vector2(this.Position.X + 200, this.Position.Y + 120), Color.White);
        }
        }

    public void AddPoint() { Points += 100; }   
    public void AddError() { Errors ++;Points-= 50; }

}

public class PointWrapper

{ 


    public Point Point { get; set; }

    public PointWrapper(Point p)
    {
            this.Point = p; 
    }





}

public class DynamicRectangleWrapper
{
    public PointWrapper Vertex0; // top links
    public PointWrapper Vertex1; // top recht 
    public PointWrapper Vertex2; // bottom recht 
    public PointWrapper Vertex3; // bottom links



    public Point[] Vertexes { get; set; } = new Point[4];
    public Rectangle Rectangle { get; set; }
    public DynamicRectangleWrapper(Rectangle r )
    {
        Rectangle = r;  

        Vertex0 = new PointWrapper( Rectangle.Location);

        Vertex1 = new PointWrapper(Rectangle.Location + new Point (Rectangle.Width, 0 ) );
        Vertex2 = new PointWrapper(Rectangle.Location + new Point(Rectangle.Width, Rectangle.Height));
        Vertex3 = new PointWrapper(Rectangle.Location + new Point(0,Rectangle.Height ));

        for (int i = 0; i < Vertexes.Length; i++) { if (i == 0) { Vertexes[i] = Vertex0.Point; } else if (i== 1) { Vertexes[i] = Vertex1.Point; } else if (i==2 ) { Vertexes[i] = Vertex2.Point; } else { Vertexes[i] = Vertex3.Point; }  }
    }





} 
