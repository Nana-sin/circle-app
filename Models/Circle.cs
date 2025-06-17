using System;
using Avalonia;
using Avalonia.Media;
using SkiaSharp;

namespace CircleApp.Models;

public class Circle
{
    private static int _counter = 1;
    private readonly Random _rnd = new Random();
    
    public int Width { get; set; } = 2;
    public Color VertexColor { get; set; } = Colors.Blue;
    public Color MainColor { get; private set; }
    public int SelectedVertex { get; set; } = -1;
    
    public Point Center { get; set; }
    public int Radius { get; set; }
    public int Index { get; }

    public string DisplayText => $"Круг {Index}"; 
    public Circle()
    {
        Center = new Point(_rnd.Next(300, 500), _rnd.Next(150, 400));
        Radius = _rnd.Next(50, 100);
        MainColor = InvertColor();
        Index = _counter++;
    }

    private Color InvertColor()
    {
        return new Color(
            VertexColor.A,
            (byte)(255 - VertexColor.R),
            (byte)(255 - VertexColor.G),
            (byte)(255 - VertexColor.B));
    }
    public bool IsSelectedVertex(Point point)
    {
        SelectedVertex = -1;
        
        if ((Math.Pow(Center.X - point.X, 2) + Math.Pow(Center.Y - point.Y, 2) <= 25))
        {
            SelectedVertex = 0;
            return true;
        }
        
        double distance = Math.Sqrt(Math.Pow(Center.X - point.X, 2) + Math.Pow(Center.Y - point.Y, 2));
        if (Math.Abs(distance - Radius) <= 5)
        {
            SelectedVertex = 1;
            return true;
        }

        return false;
    }

    public void ChangeXY(Point point)
    {
        if (SelectedVertex == 0)
        {
            Center = point;
        }
        else if (SelectedVertex == 1)
        {
            Radius = (int)Math.Sqrt(Math.Pow(point.X - Center.X, 2) + Math.Pow(point.Y - Center.Y, 2));
        }
    }

    public void ChangeAll(Vector offset)
    {
        Center = new Point(Center.X + offset.X, Center.Y + offset.Y);
    }
}