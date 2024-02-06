using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class SeperatorAttribute : PropertyAttribute
{
    public readonly Color SeperatorColor;
    private Color GetColor(ColorType color)
    {
        Color col = color switch
        {
            ColorType.WHITE => Color.white,
            ColorType.GRAY => Color.grey,
            ColorType.CYAN => Color.cyan,
            ColorType.BLACK => Color.black,
            ColorType.RED => Color.red,
            ColorType.GREEN => Color.green,
            ColorType.BLUE => Color.blue,
            ColorType.YELLOW => Color.yellow,
            ColorType.MAGENTA => Color.magenta,
            _=>Color.white
        };
        return col;
    }
    public readonly float Spacing;
    public readonly float SeperatorThickness;

    public SeperatorAttribute(float spacing = 10f, float thickness = 2f, ColorType colorType = ColorType.GRAY)
    {
        SeperatorThickness = thickness;
        Spacing = spacing;
        SeperatorColor = GetColor(colorType);
    }


    public SeperatorAttribute(float thickness, float spacing, float r, float g, float b ,float a)
    {
        SeperatorThickness = thickness;
        Spacing = spacing;
        SeperatorColor = new Color(r, g, b, a);
    }
}
