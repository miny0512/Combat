using System;
using UnityEngine;


public enum ColorType
{
    WHITE,
    GRAY,
    CYAN,
    BLACK,
    RED,
    GREEN,
    BLUE,
    YELLOW,
    MAGENTA
}

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class HeaderColorAttribute : PropertyAttribute
{
    public readonly string header;
    public readonly GUIStyle style;
    public readonly int fontSize;

    public readonly Color underlineColor;
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

    public HeaderColorAttribute(string header, float textColR, float textColG, float textColB, float textColA, float underLineColR, float underLineColG, float underLineColB, float underLineColA,
        FontStyle fontStyle = FontStyle.Bold | FontStyle.Italic, int fontSize = 14)
    {
        this.header = header;
        style = new GUIStyle();
        style.normal.textColor = new Color(textColR, textColG, textColB, textColA);
        this.underlineColor = new Color(underLineColR,underLineColG, underLineColB, underLineColA);
        style.fontSize = this.fontSize = fontSize;
        style.fontStyle = fontStyle;
    }

    public HeaderColorAttribute(string header, ColorType textColor, ColorType underlineColor = ColorType.WHITE, FontStyle fontStyle = FontStyle.Bold | FontStyle.Italic, int fontSize = 14)
    {
        this.header = header;
        style = new GUIStyle();
        style.normal.textColor = GetColor(textColor);
        this.underlineColor = GetColor(underlineColor);
        style.fontSize = this.fontSize = fontSize;
        style.fontStyle = fontStyle;
    }
}
