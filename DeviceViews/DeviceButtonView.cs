using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
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

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);

            (Shapes[0] as Rectangle).Render(dc, BorderPen, Status.IsUp ? ToggleBrush : Brushes.Gray);
            RenderName(dc);
        }

        public static readonly Pen BorderPen = new(Brushes.White, 1);
    }
}
