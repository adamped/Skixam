using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using SkiaSharp;
using Skixam.Forms;
using System;
using System.Collections.Generic;
using Xamarin.Forms.Platform.Android;
using ARelativeLayout = Android.Widget.RelativeLayout;

namespace Skixam.Droid
{
    public class SkixamAppCompatActivity : AppCompatActivity, IDeviceInfoProvider
    {        
        Xamarin.Forms.Application _application;
        
        ARelativeLayout _layout;
       
        bool _renderersAdded;
        
        protected virtual bool AllowFragmentRestore => false;

        public event EventHandler ConfigurationChanged;


        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            ConfigurationChanged?.Invoke(this, new EventArgs());
        }

        protected void LoadApplication(Xamarin.Forms.Application application)
        {
            if (!_renderersAdded)
            {

                //RegisterHandlerForDefaultRenderer(typeof(Xamarin.Forms.Button), typeof(FastRenderers.ButtonRenderer), typeof(ButtonRenderer));
                //RegisterHandlerForDefaultRenderer(typeof(Label), typeof(FastRenderers.LabelRenderer), typeof(LabelRenderer));

                _renderersAdded = true;
            }

            _application = application ?? throw new ArgumentNullException("application");
            
            Xamarin.Forms.Application.SetCurrentApplication(application);

            SetMainPage();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            ActivityResultCallbackRegistry.InvokeCallback(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
         
            _layout = new ARelativeLayout(BaseContext);
            SetContentView(_layout);

            Xamarin.Forms.Application.ClearCurrent();
            
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
        }


        SKCanvas _canvas;
        Xamarin.Forms.Size _size;

        void InternalSetPage(Xamarin.Forms.Page page)
        {
            IList<Action<SKCanvas, int, int>> renderChildren = new List<Action<SKCanvas, int, int>>();
            _layout.AddView(new NativeSkiaView(this, new SkiaView((canvas, w, h) =>
            {
                canvas.Clear(SKColors.White);
                _canvas = canvas;
                _size = new Xamarin.Forms.Size(w, h);
                foreach (var renderer in renderChildren)
                    renderer(canvas, w, h);
            })));

            _layout.BringToFront();

            AddChild(page, renderChildren);
        }

        void AddChild(Xamarin.Forms.Page page, IList<Action<SKCanvas, int, int>> renderChildren)
        {
            foreach (var child in page.InternalChildren)
            {
                if (child is Xamarin.Forms.Label)
                {
                    renderChildren.Add((canvas, width, height) => new Skixam.Forms.Renderers.LabelRenderer().Render((Xamarin.Forms.Label)child, _canvas, _size));
                }

            }
        }

        void SetMainPage()
        {
            InternalSetPage(_application.MainPage);
        }
        
        
    }
}