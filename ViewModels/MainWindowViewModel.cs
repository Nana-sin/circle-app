using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using CircleApp.Models;
using ReactiveUI;

namespace CircleApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Circle? _selectedCircle;
    private Circle? _selectedCircleInListBox;
    private Point _lastPointerPosition;
    
    public ReactiveCommand<Unit, Unit> AddCircleCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveCircleCommand { get; }
    public ReactiveCommand<Circle, Unit> RemoveSpecificCircleCommand { get; }

    public ObservableCollection<Circle> Circles { get; } = new();

    public Circle? SelectedCircle
    {
        get => _selectedCircle;
        set => this.RaiseAndSetIfChanged(ref _selectedCircle, value);
    }
    
    public Circle? SelectedCircleInListBox
    {
        get => _selectedCircleInListBox;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedCircleInListBox, value);
            SelectedCircle = value;
        }
    }

    public MainWindowViewModel()
    {
        Circles.Add(new Circle());
        AddCircleCommand = ReactiveCommand.Create(AddCircle);
        RemoveCircleCommand = ReactiveCommand.Create(RemoveSelectedCircle, 
            this.WhenAnyValue(vm => vm.SelectedCircleInListBox).Select(c => c != null));
        
        RemoveSpecificCircleCommand = ReactiveCommand.Create<Circle>(RemoveCircle);
    }
    private void AddCircle()
    {
        var newCircle = new Circle();
        Circles.Add(newCircle);
        SelectedCircleInListBox = newCircle;
    }

    
    private void RemoveSelectedCircle()
    {
        if (SelectedCircleInListBox != null)
        {
            RemoveCircle(SelectedCircleInListBox);
        }
    }

    private void RemoveCircle(Circle circle)
    {
        if (circle == null) return;
        
        Circles.Remove(circle);
        
        if (SelectedCircleInListBox == circle)
        {
            SelectedCircleInListBox = Circles.LastOrDefault();
            SelectedCircle = SelectedCircleInListBox;
        }
    }

    public void HandlePointerPressed(Point position)
    {
        _lastPointerPosition = position;
        
        foreach (var circle in Circles)
        {
            if (circle.IsSelectedVertex(position))
            {
                SelectedCircle = circle;
                return;
            }
        }
        SelectedCircle = null;
    }

    public void HandlePointerMoved(Point position)
    {
        if (SelectedCircle == null) return;
        
        if (SelectedCircle.SelectedVertex >= 0)
        {
            SelectedCircle.ChangeXY(position);
        }
        else
        {
            var offset = position - _lastPointerPosition;
            SelectedCircle.ChangeAll(new Vector(offset.X, offset.Y));
        }
        
        _lastPointerPosition = position;
    }

    public void HandlePointerReleased()
    {
        if (SelectedCircle != null)
        {
            SelectedCircle.SelectedVertex = -1;
        }
    }
}