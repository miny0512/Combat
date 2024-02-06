using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HeaderColorAttribute))]
public class CustomHeaderDecorator : DecoratorDrawer
{
    HeaderColorAttribute attr => attribute as HeaderColorAttribute;
    public override float GetHeight()
    {
        return base.GetHeight() + 6f;
    }
    public override void OnGUI(Rect position)
    {
        GUI.contentColor= Color.white;
        // GUI.backgroundColor = Color.red;
        GUI.Label(position, attr.header, attr.style);
        var underlineRect = position;
        underlineRect.y += attr.fontSize + 4f;
        underlineRect.height = 1.5f;
       
        GUI.DrawTexture(underlineRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, false, 0, attr.underlineColor,0,0);
    }
}
