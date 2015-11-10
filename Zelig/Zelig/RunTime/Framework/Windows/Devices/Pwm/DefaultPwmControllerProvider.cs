﻿//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

namespace Windows.Devices.Pwm
{
    using System;
    using Llilum = Microsoft.Llilum.Devices.Pwm;
    using Windows.Devices.Pwm.Provider;

    internal class DefaultPwmControllerProvider : IPwmControllerProvider
    {
        /// <summary>
        /// Used to keep track of the pin past and present state for UWP
        /// </summary>
        private class ControllerPin
        {
            public Llilum.PwmPin    Pin;
            public double           PreviousDutyCycle;
            public bool             Enabled;
        }

        private readonly    Llilum.IPwmChannelInfoUwp   m_providerInfo;
        private             ControllerPin[]             m_pwmPins;
        private             object                      m_channelLock;
        private             double                      m_frequency;

        public DefaultPwmControllerProvider(Llilum.IPwmChannelInfoUwp pwmInfoUwp)
        {
            m_providerInfo = pwmInfoUwp;
            m_pwmPins = new ControllerPin[m_providerInfo.PwmPinNumbers.Length];
            m_channelLock = new object();
        }

        public double ActualFrequency
        {
            get
            {
                return m_frequency;
            }
        }

        public double MaxFrequency
        {
            get
            {
                return m_providerInfo.MaxFrequency;
            }
        }

        public double MinFrequency
        {
            get
            {
                return m_providerInfo.MinFrequency;
            }
        }

        public int PinCount
        {
            get
            {
                return m_providerInfo.PwmPinNumbers.Length;
            }
        }

        public void AcquirePin(int pin)
        {
            if (pin >= m_pwmPins.Length || pin < 0)
            {
                throw new InvalidOperationException();
            }

            lock (m_channelLock)
            {
                if (m_pwmPins[pin] == null)
                {
                    // Try allocating first, to avoid releasing the pin if allocation fails
                    ControllerPin controlPin = new ControllerPin();
                    Llilum.PwmPin newPin = new Llilum.PwmPin(m_providerInfo.PwmPinNumbers[pin]);

                    // Set frequency to current, and duty cycle to 0 (disabled)
                    int usPeriod = (int)(1000000.0 / ActualFrequency);
                    newPin.SetPeriod(usPeriod);

                    // Initialize the pin to disabled by default
                    newPin.SetDutyCycle(0);

                    controlPin.Pin = newPin;
                    m_pwmPins[pin] = controlPin;
                }
            }
        }

        public void DisablePin(int pin)
        {
            if (pin >= m_pwmPins.Length || pin < 0)
            {
                throw new InvalidOperationException();
            }

            lock (m_channelLock)
            {
                if (m_pwmPins[pin] == null)
                {
                    throw new InvalidOperationException();
                }

                // mBed does not have a notion of disabling a pin, so we just
                // set the duty cycle to 0
                m_pwmPins[pin].Pin.SetDutyCycle(0);
                m_pwmPins[pin].Enabled = false;
            }
        }

        public void EnablePin(int pin)
        {
            if (pin >= m_pwmPins.Length || pin < 0)
            {
                throw new InvalidOperationException();
            }

            lock (m_channelLock)
            {
                if (m_pwmPins[pin] == null)
                {
                    throw new InvalidOperationException();
                }

                // mBed does not have a notion of enabling a pin, so we just
                // set the duty cycle to its previous value
                m_pwmPins[pin].Pin.SetDutyCycle((float)m_pwmPins[pin].PreviousDutyCycle);
                m_pwmPins[pin].Enabled = true;
            }
        }

        public void ReleasePin(int pin)
        {
            if (pin >= m_pwmPins.Length || pin < 0)
            {
                throw new InvalidOperationException();
            }

            lock (m_channelLock)
            {
                if (m_pwmPins[pin] == null)
                {
                    throw new InvalidOperationException();
                }

                m_pwmPins[pin].Pin.Dispose();
                m_pwmPins[pin] = null;
            }
        }

        public double SetDesiredFrequency(double frequency)
        {
            // UWP does not have a notion of having a separate frequency per pin
            // so we need to conform to the API and use the same frequency for all pins
            m_frequency = frequency;

            return m_frequency;
        }

        public void SetPulseParameters(int pin, double dutyCycle, bool invertPolarity)
        {
            if (pin >= m_pwmPins.Length || pin < 0)
            {
                throw new ArgumentException(string.Empty, "pin");
            }

            if(dutyCycle < 0 || dutyCycle > 1)
            {
                throw new ArgumentOutOfRangeException("dutyCycle", string.Empty);
            }

            lock (m_channelLock)
            {
                if (m_pwmPins[pin] == null)
                {
                    throw new InvalidOperationException();
                }

                // If polarity is inverted, inverted duty cycle is 100% - dutyCycle
                if(invertPolarity)
                {
                    dutyCycle = 1.0 - dutyCycle;
                }

                if(m_pwmPins[pin].Enabled)
                {
                    m_pwmPins[pin].Pin.SetDutyCycle((float)dutyCycle);
                }
                
                // We always want to set this, so it gets picked up when the pin gets enabled
                m_pwmPins[pin].PreviousDutyCycle = dutyCycle;
            }
        }
    }
}
