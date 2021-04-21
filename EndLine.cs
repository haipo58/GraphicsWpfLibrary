using System.Windows.Media;

namespace GraphicsWpfLibrary
{
    public class EndLine : GraphicView
    {
        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            var pen = IsSelected ? SectionView.ProtectPen : SectionView.ClearPen;
            foreach (Line line in Shapes)
                line.Render(dc, pen);
        }
    }
}
