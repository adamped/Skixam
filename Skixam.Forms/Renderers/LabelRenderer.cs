using SkiaSharp;
using Xamarin.Forms;

namespace Skixam.Forms.Renderers
{
    public class LabelRenderer
    {
        public void Render(Label label, SKCanvas canvas, Size size)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = (float)label.FontSize * 4;
                paint.Color = SKColors.Black;

                var length = paint.MeasureText(label.Text);

                canvas.DrawText(label.Text, 0, paint.TextSize, paint);
            }
        }
    }
}