﻿// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;

namespace InTheHand.Nfc
{
    public sealed partial class NdefReader
    {

        public Task ScanAsync(NdefScanOptions options = null)
        {
            return PlatformScanAsync(options);
        }

        public event EventHandler<NdefMessage> Reading;

        public event EventHandler Error;
    }
}
