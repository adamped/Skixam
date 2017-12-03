using SkiaSharp;
using Xamarin.Forms;

namespace Skixam.Forms.Renderers
{
    public class LabelRenderer
    {
        public void Render(Label label, SKCanvas canvas, Size size)
        {
            // TODO: Need to work on layout, and adjust x/y points as determined by X.F. layout system
            using (var paint = new SKPaint())
            {
                paint.TextSize = (float)label.FontSize * 4; // Note: Need to work on scaling factor for Android. x4 here is a just a workaround for my particular simulator device.
                paint.Color = SKColors.Black;

                var length = paint.MeasureText(label.Text);

                canvas.DrawText(label.Text, 0, paint.TextSize, paint);
            }
        }
    }
}