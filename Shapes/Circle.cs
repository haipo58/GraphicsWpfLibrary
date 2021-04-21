using RailwaySignalsModels;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class Circle : Shape
    {
        [XmlIgnore]
        public Point Center { get; set; }

        [PropertyIgnore]
        [XmlAttribute]
        public string SCenter
        {
            get => Center.ToString();
            set => Center = Point.Parse(value);
        }

        [XmlAttribute]
        public int Radio { get; set; }

        public override void Offset(double x, double y) =>
            Center = new Point(Center.X + x, Center.Y + y);

        public override void Render(DrawingContext dc, Pen pen = null, Brush brush = null)
            => dc.DrawEllipse(brush, pen, Center, Radio, Radio);

        public override void FlipX(double flipBase = 0) => Center = new Point(flipBase * 2 - Center.X, Center.Y);
        public override void FlipY(double flipBase = 0) => Center = new Point(Center.X, flipBase * 2 - Center.Y);
    }
}