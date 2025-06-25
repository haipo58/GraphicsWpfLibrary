using RailwaySignalsModels;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GraphicsWpfLibrary.GraphicViews
{
    public interface IContainerView
    {
        void AddChildren2Canvas(Canvas canvas);
        void AddChildren2Models(CbiModel cbiModel);
        void RemoveFromGrahics(List<GraphicView> graphics);
    }
}
