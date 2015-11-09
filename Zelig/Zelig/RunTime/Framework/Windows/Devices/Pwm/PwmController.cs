//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

namespace Windows.Devices.Pwm
{
    using System;
    using Windows.Devices.Pwm.Provider;
    using Llilum = Microsoft.Llilum.Devices.Pwm;
    using System.Runtime.CompilerServices;

    public sealed class PwmController
    {
        internal IPwmControllerProvider m_pwmControllerProvider;

        private PwmController(IPwmControllerProvider provider)
        {
            m_pwmControllerProvider = provider;
        }

        public double ActualFrequency
        {
            get
            {
                return m_pwmControllerProvider.ActualFrequency;
            }
        }

        public double MaxFrequency
        {
            get
            {
                return m_pwmControllerProvider.MaxFrequency;
            }
        }

        public double MinFrequency
        {
            get
            {
                return m_pwmControllerProvider.MinFrequency;
            }
        }

        public int PinCount
        {
            get
            {
                return m_pwmControllerProvider.PinCount;
            }
        }

        public double SetDesiredFrequency(double desiredFrequency)
        {
            return m_pwmControllerProvider.SetDesiredFrequency(desiredFrequency);
        }

        public PwmPin OpenPin(int pinNumber)
        {
            // If the channel cannot be acquired, this will throw
            m_pwmControllerProvider.AcquirePin(pinNumber);

            return new PwmPin(this, pinNumber);
        }
        
        public static /*IAsyncOperation<PwmController>*/PwmController GetDefaultAsync()
        {
            return new PwmController(new DefaultPwmControllerProvider(GetPwmProviderInfo()));
        }

        //public static extern IAsyncOperation<IVectorView<PwmController>> GetControllersAsync([In] IPwmProvider provider);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Llilum.IPwmChannelInfoUwp GetPwmProviderInfo();
    }
}
