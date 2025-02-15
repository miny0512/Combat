﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadonlyAttribute))]
public class ReadonlyAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled= false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled= true;
    }
}
