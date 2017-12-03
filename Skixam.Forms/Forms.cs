using Xamarin.Forms;

namespace Skixam.Forms
{
    public class Forms
    {

        public static void Init()
        {
            Device.SetIdiom(TargetIdiom.Phone);
            Device.PlatformServices = new SkixamPlatformServices();
            Device.Info = new SkixamDeviceInfo();
        }
    }
}
