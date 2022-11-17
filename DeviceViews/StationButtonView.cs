using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class StationButtonView : CommandDevice
    {
        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyShow]
        public StationButtonModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public RelayStatus Status { get; } = new();

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);

            (Shapes[0] as Rectangle).Render(dc, DeviceButtonView.BorderPen, Status.IsUp ? Brushes.Gray : Brushes.Red);
            RenderName(dc);
        }
    }
}
