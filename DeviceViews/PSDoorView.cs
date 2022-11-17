using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class PSDoorView : DeviceView
    {
        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyShow]
        public PSDoorModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public PSDoorStatus Status { get; } = new();

        public override void Render(DrawingContext dc)
        {
            Pen pen = Status.IsPassby ? PassbyPen
                : Status.IsOpen ? OpenPen : ClosePen;

            Shapes[0].Render(dc, pen);
            Shapes[2].Render(dc, pen);

            if (!Status.IsOpen)
                Shapes[1].Render(dc, pen);
        }

        private static readonly Pen PassbyPen = new(Brushes.Yellow, 3);
        private static readonly Pen OpenPen = new(Brushes.Red, 3);
        private static readonly Pen ClosePen = new(Brushes.Blue, 3);
    }
}
