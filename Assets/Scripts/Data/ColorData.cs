using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ColorData
{
    public float r;
    public float g; 
    public float b; 
    public float a;
    public void SetColor(Color color)
    {
        if (color == null) return; 
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }
}
