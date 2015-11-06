//
// Copyright (c) Microsoft Corporation. All rights reserved.
//

namespace Microsoft.Llilum.Devices.Pwm
{
    using System;
    using System.Runtime.CompilerServices;

    public class PwmPin : IDisposable
    {
        private PwmChannel m_pwmPin;
        private bool m_disposed = false;
        private int m_pinNumber;

        public PwmPin(int pinNumber)
        {
            m_pwmPin = TryAcquirePwmPin(pinNumber);
            if (m_pwmPin == null)
            {
                throw new ArgumentException();
            }

            m_pwmPin.InitializePin();
            m_pinNumber = pinNumber;
        }

        ~PwmPin()
        {
            Dispose(false);
        }

        public void SetDutyCycle(float ratio)
        {
            m_pwmPin.SetDutyCycle(ratio);
        }

        public void SetPeriod(int microSeconds)
        {
            m_pwmPin.SetPeriod(microSeconds);
        }

        public void SetPulseWidth(int microSeconds)
        {
            m_pwmPin.SetPulseWidth(microSeconds);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    m_pwmPin.Dispose();
                }

                m_disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern PwmChannel TryAcquirePwmPin(int pinNumber);
    }
}
