//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

namespace Microsoft.CortexM3OnMBED.HardwareModel
{
    using System;
    using System.Runtime.InteropServices;
    using Runtime = Microsoft.Zelig.Runtime;

    public class PwmChannel : Llilum.Devices.Pwm.PwmChannel
    {
        private unsafe PwmImpl* m_pwm;
        private int m_pinNumber;

        internal PwmChannel(int pinNumber)
        {
            m_pinNumber = pinNumber;
        }

        ~PwmChannel()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            unsafe
            {
                if (m_pwm != null)
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Runtime.HardwareProvider.Instance.ReleasePins(m_pinNumber);
            }
            unsafe
            {
                tmp_pwm_free(m_pwm);
                m_pwm = null;
            }
        }

        public override void InitializePin()
        {
            unsafe
            {
                fixed (PwmImpl** adc_ptr = &m_pwm)
                {
                    tmp_pwm_alloc_init(adc_ptr, m_pinNumber);
                }
            }
        }

        public unsafe override void SetDutyCycle(float ratio)
        {
            tmp_pwm_dutycycle(m_pwm, ratio);
        }

        public unsafe override void SetPulseWidth(int microSeconds)
        {
            tmp_pwm_pulsewidth_us(m_pwm, microSeconds);
        }

        public unsafe override void SetPeriod(int microSeconds)
        {
            tmp_pwm_period_us(m_pwm, microSeconds);
        }

        [DllImport("C")]
        private static unsafe extern void tmp_pwm_alloc_init(PwmImpl** obj, int pinNumber);
        [DllImport("C")]
        private static unsafe extern void tmp_pwm_free(PwmImpl* obj);
        [DllImport("C")]
        private static unsafe extern void tmp_pwm_dutycycle(PwmImpl* obj, float ratio);
        [DllImport("C")]
        private static unsafe extern void tmp_pwm_period_us(PwmImpl* obj, int uSeconds);
        [DllImport("C")]
        private static unsafe extern void tmp_pwm_pulsewidth_us(PwmImpl* obj, int uSeconds);
    }

    internal unsafe struct PwmImpl
    {
    };
}
