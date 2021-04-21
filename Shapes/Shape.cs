using System.Windows.Media;

namespace GraphicsWpfLibrary
{
    public abstract class Shape
    {
        public abstract void Render(DrawingContext dc, Pen pen = null, Brush brush = null);

        public abstract void Offset(double x, double y);

        public abstract void FlipX(double flipBase);
        public abstract void FlipY(double flipBase);
    }
}