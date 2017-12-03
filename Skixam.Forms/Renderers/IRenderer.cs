using SkiaSharp;
using Xamarin.Forms;

namespace Skixam.Forms.Renderers
{
    public interface IBaseRenderer { }

    public interface IRenderer<T>: IBaseRenderer
    {
        void Render(T element, SKCanvas canvas, Size size);
    }
}
