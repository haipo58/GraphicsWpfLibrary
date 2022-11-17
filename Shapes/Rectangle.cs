using RailwaySignalsModels;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class Rectangle : Shape
    {
        [XmlIgnore]
        public Rect Rect { get; set; }

        [XmlAttribute, PropertyIgnore]
        public string SRect
        {
            get => Rect.ToString();
            set => Rect = Rect.Parse(value);
        }

        [XmlIgnore, PropertyIgnore]
        public Brush ForeBrush { get; set; } = Brushes.Black;

        [XmlAttribute]
        public string SBrush
        {
            get => ForeBrush.ToString();
            set {
                Color color = (Color)ColorConverter.ConvertFromString(value);
                ForeBrush = new SolidColorBrush(color);
            }
        }

        public override void FlipX(double flipBase) =>
            Rect = new Rect(flipBase * 2 - Rect.Left - Rect.Width, Rect.Top, Rect.Width, Rect.Height);

        public override void FlipY(double flipBase) =>
            Rect = new Rect(Rect.Left, flipBase * 2 - Rect.Top - Rect.Height, Rect.Width, Rect.Height);

        public override void Offset(double x, double y) =>
            Rect = new Rect(Rect.X + x, Rect.Y + y, Rect.Width, Rect.Height);

        public override void Render(DrawingContext dc, Pen pen = null, Brush brush = null)
            => dc.DrawRectangle(brush??ForeBrush, pen, Rect);
    }
}