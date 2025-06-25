using System.Windows;
using System.Windows.Media;

namespace GraphicsWpfLibrary
{
    public class UIRender : UIElement
    {
        public GraphicView Graphic { get; set; }

        protected override void OnRender(DrawingContext dc) => dc.DrawDrawing(Graphic.Drawing);
    }
}