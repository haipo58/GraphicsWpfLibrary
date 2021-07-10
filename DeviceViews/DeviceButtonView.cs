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
        public RelayStatus Status { get; set; }

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);

            var rect = Shapes[0] as Rectangle;
            dc.DrawRectangle(Brushes.Gray, BorderPen, rect.Rect);

            RenderName(dc);
        }

        private static readonly Pen BorderPen = new(Brushes.White, 1);
    }
}
