using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class TextView : GraphicView
    {
        private FormattedText fmtText;
        private Brush foreBrush = Brushes.White;
        private int fontSize = 20;

        [XmlAttribute]
        public string Text
        {
            get => fmtText == null ? "" : fmtText.Text;
            set => CreateFmtText(value);
        }

        [XmlAttribute]
        public int FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                CreateFmtText(fmtText.Text);
            }
        }

        [XmlAttribute]
        public string ForeColor
        {
            get
            {
                if (brushMap.ContainsValue(foreBrush))
                    foreach (var pair in brushMap)
                        if (pair.Value == foreBrush)
                            return pair.Key;
                return "";
            }
            set
            {
                if (brushMap.ContainsKey(value))
                {
                    foreBrush = brushMap[value];
                    CreateFmtText(fmtText.Text);
                }
            }
        }

        private void CreateFmtText(string value)
        {
            fmtText = new FormattedText(value, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("微软雅黑"), FontSize, foreBrush, 1);
            UI.InvalidateVisual();
        }

        public override void Render(DrawingContext dc) => dc.DrawText(fmtText, new Point());

        private static Dictionary<string, Brush> brushMap = new Dictionary<string, Brush>()
        {
            {"Red", Brushes.Red},
            {"Green", Brushes.Green},
            {"Yellow", Brushes.Yellow},
            {"White", Brushes.White},
            {"Black", Brushes.Black},
            {"Blue", Brushes.Blue},
        };
    }
}
