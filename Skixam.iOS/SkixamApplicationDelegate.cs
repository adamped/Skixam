using Foundation;
using SkiaSharp;
using Skixam.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UIKit;

namespace Skixam.iOS
{
    public class SkixamApplicationDelegate : UIApplicationDelegate
    {
        Xamarin.Forms.Application _application;
        bool _isSuspended;
        UIWindow _window;
        public override UIWindow Window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
            }
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            return true;
        }

      
        // finish initialization before display to user
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            // check contents of launch options and evaluate why the app was launched and respond
            // initialize the important data structures
            // prepare you apps window and views for display
            // keep lightweight, anything long winded should be executed asynchronously on a secondary thread.
            // application:didFinishLaunchingWithOptions
            if (Window == null)
                Window = new UIWindow(UIScreen.MainScreen.Bounds);

            if (_application == null)
                throw new InvalidOperationException("You MUST invoke LoadApplication () before calling base.FinishedLaunching ()");

            SetMainPage();
            _application.SendStart();
            return true;
        }

        // about to become foreground, last minute preparatuin
        public override void OnActivated(UIApplication uiApplication)
        {
            // applicationDidBecomeActive
            // execute any OpenGL ES drawing calls
            if (_application != null && _isSuspended)
            {
                _isSuspended = false;
                CultureInfo.CurrentCulture.ClearCachedData();
                TimeZoneInfo.ClearCachedData();
                _application.SendResume();
            }
        }

        // transitioning to background
        public override async void OnResignActivation(UIApplication uiApplication)
        {
            // applicationWillResignActive
            if (_application != null)
            {
                _isSuspended = true;
                await _application.SendSleepAsync();
            }
        }

        // first chance to execute code at launch time
        public override bool WillFinishLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            // check contents of launch options and evaluate why the app was launched and respond
            // initialize the important data structures
            // prepare you apps window and views for display
            // keep lightweight, anything long winded should be executed asynchronously on a secondary thread.
            // application:willFinishLaunchingWithOptions
            // Restore ui state here
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _application != null)
                _application.PropertyChanged -= ApplicationOnPropertyChanged;

            base.Dispose(disposing);
        }

        protected void LoadApplication(Xamarin.Forms.Application application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            Xamarin.Forms.Application.SetCurrentApplication(application);
            _application = application;

            application.PropertyChanged += ApplicationOnPropertyChanged;
        }

        void ApplicationOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "MainPage")
                UpdateMainPage();
        }
        
        void SetMainPage()
        {
            UpdateMainPage();
            Window.MakeKeyAndVisible();
        }

        void UpdateMainPage()
        {
            if (_application.MainPage == null)
                return;

            IList<Action<SKCanvas, int, int>> renderChildren = new List<Action<SKCanvas, int, int>>();

            UIView view = new NativeSkiaView(new SkiaView((canvas, w, h) =>
            {
                canvas.Clear(SKColors.White);
                _canvas = canvas;
                _size = new Xamarin.Forms.Size(w, h);
                foreach (var renderer in renderChildren)
                    renderer(canvas, w, h);
            }));

            UIViewController vc = new UIViewController
            {
                View = view
            };

            AddRenderers(_application.MainPage, renderChildren);

            Window.RootViewController = vc;
            
        }

        SKCanvas _canvas;
        Xamarin.Forms.Size _size;

        void AddRenderers(Xamarin.Forms.Page page, IList<Action<SKCanvas, int, int>> renderChildren)
        {
            foreach (var child in page.InternalChildren)
            {
                if (child is Xamarin.Forms.Label)
                {
                    renderChildren.Add((canvas, width, height) => new Skixam.Forms.Renderers.LabelRenderer().Render((Xamarin.Forms.Label)child, _canvas, _size));
                }
            }
        }
    }
}
