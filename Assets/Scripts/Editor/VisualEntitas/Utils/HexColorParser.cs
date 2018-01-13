using System;
using System.Globalization;
using UnityEngine;

namespace Entitas.Visual.Utils
{
    public static class HexColor
    {
        public static Color Parse(string hexstring)
        {
            if (hexstring.StartsWith("#"))
            {
                hexstring = hexstring.Substring(1);
            }

            if (hexstring.StartsWith("0x"))
            {
                hexstring = hexstring.Substring(2);
            }

            if (hexstring.Length != 6)
            {
                throw new Exception(string.Format("{0} is not a valid color string.", hexstring));
            }

            byte r = byte.Parse(hexstring.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexstring.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexstring.Substring(4, 2), NumberStyles.HexNumber);

            return new Color32(r, g, b, 1);
        }
    }
}