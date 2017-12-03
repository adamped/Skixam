using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Skixam.Forms
{
    public class SkixamDeviceInfo : DeviceInfo
    {
        public override Size PixelScreenSize => throw new NotImplementedException();

        public override Size ScaledScreenSize => throw new NotImplementedException();

        public override double ScalingFactor => throw new NotImplementedException();
    }
}
