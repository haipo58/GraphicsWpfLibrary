using GraphicsWpfLibrary.DeviceViews;
using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary.GraphicViews;

public class RectView : GraphicView
{
    [XmlIgnore, PropertyIgnore]
    public override bool IsSelected
    {
        get => base.IsSelected;
        set
        {
            base.IsSelected = value;
            UI.InvalidateVisual();
        }
    }

    public override void Render()
    {
        using var dc = Drawing.Open();
        Shapes[0].Render(dc, IsSelected ? SelectedPen : DeviceButtonView.BorderPen);
    }

    private static readonly Pen SelectedPen = new(Brushes.Yellow, 1);
}
