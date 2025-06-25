using GraphicsWpfLibrary.DeviceViews;

namespace GraphicsWpfLibrary
{
    public class EndLineView : GraphicView
    {
        public override void Render()
        {
            using var dc = Drawing.Open();

            AdditionalRenderAction?.Invoke(dc);
            foreach (var item in Shapes)
                item.Render(dc, SectionView.LockPen);
        }
    }
}
