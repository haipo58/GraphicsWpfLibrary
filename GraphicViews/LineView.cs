using System.Windows.Media;

namespace GraphicsWpfLibrary
{
    public class LineView : GraphicView
    {
        public static Pen LinePen { get; set; }

        public override bool IsSelected 
        { 
            get => base.IsSelected; 
            set
            {
                base.IsSelected = value;
                UI.InvalidateVisual();
            }
        }

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            RenderDefaultShpaes(dc);
        }
    }
}
