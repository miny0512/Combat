using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeperatorAttribute))]
public class SeperatorAttributeDrawer : DecoratorDrawer
{
    SeperatorAttribute attr => attribute as SeperatorAttribute;

    public override float GetHeight()
    {
        float total = attr.Spacing + attr.SeperatorThickness + attr.Spacing;
        return total;
    }
    public override void OnGUI(Rect position)
    {
        Rect seperator = new Rect(position.xMin, position.yMin + attr.Spacing, position.width, attr.SeperatorThickness);

        EditorGUI.DrawRect(seperator, attr.SeperatorColor);
    }
}
