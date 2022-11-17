using RailwaySignalsModels;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class Line : Shape
    {
        [XmlIgnore]
        public Point Pt0 { get; set; }

        [PropertyIgnore]
        [XmlAttribute]
        public string SPt0
        {
            get => Pt0.ToString();
            set => Pt0 = Point.Parse(value);
        }

        [XmlIgnore]
        public Point Pt1 
        { 
            get; 
            set; 
        }

        [PropertyIgnore]
        [XmlAttribute]
        public string SPt1
        {
            get => Pt1.ToString();
            set => Pt1 = Point.Parse(value);
        }

        public override void Render(DrawingContext dc, Pen pen = null, Brush brush = null) => dc.DrawLine(pen, Pt0, Pt1);

        public override void Offset(double x, double y)
        {
            Pt0 = new Point(Pt0.X + x, Pt0.Y + y);
            Pt1 = new Point(Pt1.X + x, Pt1.Y + y);
        }

        public override void FlipX(double flipBase = 0)
        {
            Pt0 = new Point(flipBase * 2 - Pt0.X, Pt0.Y);
            Pt1 = new Point(flipBase * 2 - Pt1.X, Pt1.Y);
        }

        public override void FlipY(double flipBase = 0)
        {
            Pt0 = new Point(Pt0.X, flipBase * 2 - Pt0.Y);
            Pt1 = new Point(Pt1.X, flipBase * 2 - Pt1.Y);
        }

        public bool InRange(Point checkPoint, ref Point connectPoint, double range)
        {
            if (IsPointInRange(Pt0, checkPoint, range))
            {
                connectPoint = Pt0;
                return true;
            }
            
            if (IsPointInRange(Pt1, checkPoint, range))
            {
                connectPoint = Pt1;
                return true;
            }

            return false;
        }

        private static bool IsPointInRange(Point point1, Point point2, double range) => (point1 - point2).Length < range;
    }
}