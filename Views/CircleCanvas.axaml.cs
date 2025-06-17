using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using CircleApp.Models;
using CircleApp.ViewModels;


namespace CircleApp.Views;

public partial class CircleCanvas : UserControl
{
     private bool _isPointerPressed;

    public CircleCanvas()
    {
        InitializeComponent();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        
        if (DataContext is not MainWindowViewModel vm) return;
        
       
        foreach (var circle in vm.Circles)
        {
            DrawCircle(context, circle);
        }
        
        if (vm.SelectedCircle != null)
        {
            HighlightCircle(context, vm.SelectedCircle);
        }
    }

    private void DrawCircle(DrawingContext context, Circle circle)
    {
        var pen = new Pen(
            new SolidColorBrush(circle.MainColor),
            circle.Width);
        
        context.DrawEllipse(
            null,
            pen,
            circle.Center,
            circle.Radius,
            circle.Radius);
        
        var centerPen = new Pen(
            new SolidColorBrush(circle.VertexColor),
            circle.Width);
        
        context.DrawEllipse(
            null,
            centerPen,
            circle.Center,
            4,
            4);
    }
    
    private void HighlightCircle(DrawingContext context, Circle circle)
    {
        var highlightPen = new Pen(
            new SolidColorBrush(Colors.Yellow),
            1,
            dashStyle: DashStyle.Dash);
        
        context.DrawEllipse(
            null,
            highlightPen,
            circle.Center,
            circle.Radius + 5,
            circle.Radius + 5);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _isPointerPressed = true;
        
        if (DataContext is MainWindowViewModel vm)
        {
            var position = e.GetPosition(this);
            vm.HandlePointerPressed(position);
            InvalidateVisual();
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (!_isPointerPressed) return;
        
        if (DataContext is MainWindowViewModel vm)
        {
            var position = e.GetPosition(this);
            vm.HandlePointerMoved(position);
            InvalidateVisual();
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        _isPointerPressed = false;
        
        if (DataContext is MainWindowViewModel vm)
        {
            vm.HandlePointerReleased();
            InvalidateVisual();
        }
    }
}