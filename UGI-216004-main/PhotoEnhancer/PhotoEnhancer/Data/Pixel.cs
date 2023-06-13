using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEnhancer
{
    public struct Pixel
    {
        private double r;
        public double R 
        {
            get => r;
            set  => r = CheckValue(value); 
        }

        private double g;
        public double G
        {
            get => g;
            set => g = CheckValue(value);
        }

        private double b;
        public double B
        {
            get => b;
            set => b = CheckValue(value);
        }

        public double H
        {
            get
            {
                if ((RGBmax() == R) && (G >= B))
                    return 60 * ((G - B) / (RGBmax() - RGBmin())) + 0;
                if ((RGBmax() == R) && (G < B))
                    return 60 * ((G - B) / (RGBmax() - RGBmin())) + 360;
                if (RGBmax() == G)
                    return 60 * ((B - R) / (RGBmax() - RGBmin())) + 120;
                else
                    return 60 * ((R - G) / (RGBmax() - RGBmin())) + 240;
            }
        }
        public double S
        {
            get
            {
                if ((L == 0 || (RGBmax() == RGBmin())))
                    return 0;
                if ((L > 0 && L < 0.5))
                    return Trim(CheckValue(Math.Round((RGBmax() - RGBmin()) / (2 * L), 5)));
                else
                    return Trim(CheckValue(Math.Round((RGBmax() - RGBmin()) / (2 - 2 * L), 5)));
            }
        }


        public double L
        {
            get => CheckValue(Trim(0.5 * (RGBmax() + RGBmin())));
        }

        double RGBmax() => Math.Max(Math.Max(R, G), B);
        double RGBmin() => Math.Min(Math.Min(R, G), B);

        public Pixel(double red, double green, double blue) : this()
        {
            R = red;
            G = green;
            B = blue;
        }

        public static Pixel operator *(double k, Pixel p)
        {
            Pixel result = new Pixel();

            result.r = Trim(k * p.r);
            result.g = Trim(k * p.g);
            result.b = Trim(k * p.b);

            return result;
        }

        public static Pixel operator *(Pixel p, double k) => k * p;      

        private double CheckValue(double val)
        {
            if (val < 0 || val > 1)
                throw new ArgumentException("Неверное значение яркости канала");

            return val;
        }

        private static double Trim(double lightness)
        {
            if(lightness > 1)
                return 1;

            return lightness;
        }
    }
}
