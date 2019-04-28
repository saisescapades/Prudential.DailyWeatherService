using System;
using System.Collections.Generic;
using System.Text;

namespace Prudential.DailyWeatherModule.Models
{
    //The case of  property members in this class will be dependent of the case of equivalent properties in the input file
    class City
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
