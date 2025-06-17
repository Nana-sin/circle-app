using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;

namespace CircleApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        RequestedThemeVariant = ThemeVariant.Dark;
    }
    
    private void DragWindow_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}