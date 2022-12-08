using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Settings
    {
        // Fields
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static string Directions;

        // Constructor
        public Settings()
        {
            Width = 16;
            Height = 16;
            Directions = "left";
        }
    }
}
