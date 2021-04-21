namespace GraphicsWpfLibrary
{
    public interface IFlashItem
    {
        bool IsFlashing { get; }

        void OnFlashTimer();
    }
}