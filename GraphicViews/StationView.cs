using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class StationView : GraphicView
    {
        [XmlAttribute]
        public string Name { get; set; }
        [PropertyIgnore]
        public PSDoorView[] Doors { get; set; }

        public override void Render(DrawingContext dc) => Shapes[0].Render(dc, rectPen);

        private static readonly Pen rectPen = new Pen(Brushes.Silver, 3);
    }
}
