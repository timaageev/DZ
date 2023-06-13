using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEnhancer
{
    public class SepiaParameters : IParameters
    {
        [ParameterInfo(Name = "Оттенок",
                    MinValue = 0,
                    MaxValue = 359.95,
                    DefaultValue = 40,
                    Increment = 0.05)]
        public double Hue { get; set; }

        [ParameterInfo(Name = "Насыщенность",
                    MinValue = 0,
                    MaxValue = 1,
                    DefaultValue = 0.2,
                    Increment = 0.01)]
        public double Sat { get; set; }
    }
}
