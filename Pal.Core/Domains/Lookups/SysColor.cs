using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysColor
    {
        public SysColor(string colorName, string colorHex)
        {
            ColorName = colorName;
            ColorHex = colorHex;
        }

        public SysColor()
        {
        }

        public int Id { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }


        //-----------------------------------------------------------------------------------
        public static string GetRandomHexColor()
        {
            try
            {
                Random random = new ();
                return Colors.ToArray()[random.Next(Colors.Count - 1)].ColorHex;
            }
            catch (Exception)
            {
                return Colors.FirstOrDefault().ColorHex;
            }
        }
        //-----------------------------------------------------------------------------------
        public static List<SysColor> Colors
        {
            get
            {
                return new() { 
                    new SysColor("Aero", "#7CB9E8"),
                    new SysColor("Amaranth pink", "#F19CBB"),
                    new SysColor("Android green", "#3DDC84"),
                    new SysColor("Atomic tangerine", "#FF9966"),
                    new SysColor("Azure", "#007FFF"),
                    new SysColor("Barn red", "#7C0A02"),
                    new SysColor("Blush", "#DE5D83"),
                    new SysColor("Bronze", "#CD7F32"),
                    new SysColor("Byzantine", "#BD33A4"),
                    new SysColor("Cadmium green", "#006B3C"),
                    new SysColor("Cardinal", "#C41E3A"),
                    new SysColor("Caribbean green", "#00CC99"),
                    new SysColor("Cerulean", "#007BA7"),
                    new SysColor("Cosmic cobalt", "#2E2D88"),
                    new SysColor("Crimson", "#DC143C"),
                    new SysColor("Dark orchid", "#9932CC"),
                    new SysColor("Deep cerise", "#DA3287"),
                };

            }
        }
    }
}
