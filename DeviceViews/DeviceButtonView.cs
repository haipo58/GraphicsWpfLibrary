using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary.DeviceViews;

public class DeviceButtonView : CommandDevice
{
    [PropertyIgnore, XmlIgnore]
    public override DeviceModel DeviceInfo => Model;

    [PropertyShow]
    public DeviceButtonModel Model { get; set; }

    [PropertyIgnore, XmlIgnore]
    public DeviceButtonStatus Status { get; } = new();

    [XmlIgnore]
    public Brush ToggleBrush { get; set; } = Brushes.Yellow;

    public override void Render()
    {
        using var dc = Drawing.Open();

        (Shapes[0] as Rectangle).Render(dc, BorderPen, Status.IsUp ? ToggleBrush : Brushes.Gray);
        RenderName(dc);
    }

    public static readonly Pen BorderPen = new(Brushes.White, 1);
}
