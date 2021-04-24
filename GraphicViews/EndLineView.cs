using System.Windows.Media;

namespace GraphicsWpfLibrary
{
    public class EndLineView : GraphicView
    {
        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            RenderDefaultShpaes(dc);
        }
    }
}
