using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class LineView : GraphicView
    {
        private Pen pen = new Pen(Brushes.AliceBlue, 3);
        [XmlIgnore, PropertyIgnore]
        public Brush ForeBrush
        {
            get => pen.Brush;
            set => pen.Brush = value;
        }

        [XmlAttribute]
        public string SBrush
        {
            get => ForeBrush.ToString();
            set
            {
                Color color = (Color)ColorConverter.ConvertFromString(value);
                ForeBrush = new SolidColorBrush(color);
            }
        }

        public override void Render()
        {
            using var dc = Drawing.Open();

            AdditionalRenderAction?.Invoke(dc);
            foreach (var shape in Shapes)
                shape.Render(dc, pen);
        }
    }
}
