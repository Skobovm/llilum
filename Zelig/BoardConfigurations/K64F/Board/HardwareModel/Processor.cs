﻿//
// Copyright ((c) Microsoft Corporation.    All rights reserved.
//

namespace Microsoft.Llilum.K64F
{
    using System.Runtime.InteropServices;

    using RT            = Microsoft.Zelig.Runtime;
    using ChipsetModel  = Microsoft.CortexM4OnMBED;


    [RT.ProductFilter("Microsoft.Llilum.BoardConfigurations.K64FMBEDHosted")]
    public sealed class Processor : Microsoft.CortexM4OnMBED.Processor
    {
        public new class Context : ChipsetModel.Processor.Context
        {
            public override unsafe void SwitchTo( )
            {
                //
                // BUGBUG: return to thread usign VFP state as well 
                //
                base.SwitchTo( ); 
            }
        }

        //
        // Helper methods
        //

        [RT.Inline]
        public override void InitializeProcessor()
        {
            base.InitializeProcessor( );

            DisableMPU( ); 
        }
        
        [RT.Inline]
        public override Microsoft.Zelig.Runtime.Processor.Context AllocateProcessorContext()
        {
            return new Context();
        }

        //--//

        private unsafe void DisableMPU()
        {
            CUSTOM_STUB_K64F_DisableMPU( ); 
        }

        [DllImport( "C" )]
        private static extern void CUSTOM_STUB_K64F_DisableMPU( ); 
    }
}
