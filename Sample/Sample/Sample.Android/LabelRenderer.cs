using Android.Content;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Sample.Droid
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
                label.Layout(new Rectangle(0, 0, length, paint.TextSize));
            }
        }

    }
}