using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEnhancer
{
    public static class Convertors
    {
        public static Photo BitmapToPhoto(Bitmap bmp)
        {
            var photo = new Photo(bmp.Width, bmp.Height);

            for(var x = 0; x < bmp.Width; x++)
                for(var y = 0; y < bmp.Height; y++)
                {
                    var p = bmp.GetPixel(x, y);
                    photo[x, y] = new Pixel(p.R / 255.0, p.G / 255.0, p.B / 255.0);
                }

            return photo;
        }

        public static Bitmap PhotoToBitmap(Photo photo)
        {
            var bmp = new Bitmap(photo.Width, photo.Height);

            for (var x = 0; x < photo.Width; x++)
                for (var y = 0; y < photo.Height; y++)
                    bmp.SetPixel(x, y, 
                        Color.FromArgb(
                        (int)Math.Round(photo[x, y].R * 255),
                        (int)Math.Round(photo[x, y].G * 255),
                        (int)Math.Round(photo[x, y].B * 255)
                        ));
            return bmp;
        }

        public static Pixel HSLToPixel(double hue, double saturation, double lightness)
        {
            double c = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
            double m = lightness - c / 2;
            hue = hue / 360;
            double r, g, b;
            if (hue < 1 / 6)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (hue < 1 / 3)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (hue < 1 / 2)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (hue < 2 / 3)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (hue < 5 / 6)
            {
                r = x;
                g = 0;
                b = c;
            }
            else
            {
                r = c;
                g = 0;
                b = x;
            }
            return new Pixel(r + m, g + m, b + m);
        }
    }
}
