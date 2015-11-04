//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

namespace Microsoft.Llilum.STM32L152.HardwareModel.HardwareProviders
{
    using Chipset = CortexM3OnMBED;

    public sealed class GpioProvider : Chipset.HardwareModel.GpioProvider
    {
        public override int GetGpioPinIRQNumber(int pinNumber)
        {
            //
            // All GPIO interrupts use EINT3
            //
            return (int)IRQn.EINT3_IRQn;
        }
    }
}

