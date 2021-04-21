using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GraphicsWpfLibrary
{
    public class FlashTimer
    {
        private readonly Canvas canvas;

        public FlashTimer(Canvas canvas)
        {
            _ = new DispatcherTimer(
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                OnTimer, Dispatcher.CurrentDispatcher);
            this.canvas = canvas;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            foreach (var item in canvas.Children)
                if (item is UIRender ui)
                {
                    var graphic = ui.Graphic;
                    if (graphic is IFlashItem flashGraphic)
                    {
                        if (flashGraphic.IsFlashing)
                        {
                            flashGraphic.OnFlashTimer();
                            ui.InvalidateVisual();
                        }
                    }
                }
        }
    }
}