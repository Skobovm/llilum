﻿//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

namespace Microsoft.Llilum.STM32L152
{
    using System;
    
    using Chipset           = Microsoft.CortexM3OnMBED;
    using ChipsetAbstration = Microsoft.DeviceModels.Chipset.CortexM3;

    public sealed class Board : Chipset.Board
    {
        //
        // Serial Ports
        //
        private static readonly string[] m_serialPorts = { "UART2", "UART3", "UART4", "UART5" };

        public static readonly ChipsetAbstration.Board.SerialPortInfo UART2 = new ChipsetAbstration.Board.SerialPortInfo() 
        {
            TxPin = (int)PinName.USBTX,
            RxPin = (int)PinName.USBRX,
            RtsPin = unchecked((int)PinName.NC),
            CtsPin = unchecked((int)PinName.NC)
        };

        public static readonly ChipsetAbstration.Board.SerialPortInfo UART3 = new ChipsetAbstration.Board.SerialPortInfo()
        {
            TxPin = (int)PinName.PB_10,
            RxPin = (int)PinName.PB_11,
            RtsPin = unchecked((int)PinName.NC),
            CtsPin = unchecked((int)PinName.NC)
        };

        public static readonly ChipsetAbstration.Board.SerialPortInfo UART4 = new ChipsetAbstration.Board.SerialPortInfo() 
        {
            TxPin = (int)PinName.PC_10,
            RxPin = (int)PinName.PC_11,
            RtsPin = unchecked((int)PinName.NC),
            CtsPin = unchecked ((int)PinName.NC)
        };

        public static readonly ChipsetAbstration.Board.SerialPortInfo UART5 = new ChipsetAbstration.Board.SerialPortInfo()
        {
            TxPin = (int)PinName.PC_12,
            RxPin = (int)PinName.PD_2,
            RtsPin = unchecked((int)PinName.NC),
            CtsPin = unchecked((int)PinName.NC)
        };


        //
        // Gpio discovery
        //

        public override int PinCount
        {
            get
            {
                return 64;
            }
        }

        public override int NCPin
        {
            get
            {
                return -1;
            }
        }
        
        public override int PinToIndex( int pin )
        {
            return pin;
        }

        //
        // Serial Ports
        //

        public override string[] GetSerialPorts()
        {
            return m_serialPorts;
        }

        public override ChipsetAbstration.Board.SerialPortInfo GetSerialPortInfo(string portName)
        {
            switch (portName)
            {
                case "UART2":
                    return UART2;
                case "UART3":
                    return UART3;
                case "UART4":
                    return UART4;
                case "UART5":
                    return UART5;
                default:
                    return null;
            }
        }

        public override int GetSerialPortIRQNumber(string portName)
        {
            switch (portName)
            {
                case "UART2":
                    return (int)IRQn.UART0_IRQn;
                case "UART3":
                    return (int)IRQn.UART1_IRQn;
                case "UART4":
                    return (int)IRQn.UART2_IRQn;
                case "UART5":
                    return (int)IRQn.UART3_IRQn;
                default:
                    throw new NotSupportedException();
            }
        }

        //
        // System timer
        //
        public override int GetSystemTimerIRQNumber( )
        {
            return (int)IRQn.TIMER3_IRQn;
        }
    }
}

