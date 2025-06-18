using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ethertris
{
    internal class GameObjTextureInitializer
    {

        public Texture2D CreateRectangleWithBorder(GraphicsDevice graphicsDevice, int width, int height, int borderWidth, Color borderColor, Color backgroundColor)
        {

            Texture2D texture = new Texture2D(graphicsDevice, width, height);


            Color[] colorData = new Color[width * height];


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    if (x < borderWidth || x >= width - borderWidth || y < borderWidth || y >= height - borderWidth)
                    {
                        colorData[y * width + x] = borderColor;
                    }
                    else
                    {
                        colorData[y * width + x] = backgroundColor;
                    }
                }
            }


            texture.SetData(colorData);

            return texture;
        }


        public Texture2D GohstTexture(GraphicsDevice graphicsDevice, int width, int height, Color backgroundColor)
        {

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] colorData = new Color[width * height];



            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    if (x % 3 == 0)
                    {
                        colorData[y * width + x] = backgroundColor;
                    }
                    else
                    {
                        colorData[y * width + x] = Color.Transparent;


                    }
                }
            }

            texture.SetData(colorData);

            return texture;



        }
    }
}
