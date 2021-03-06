﻿//-----------------------------------------------------------------------
// <copyright file="GattService.uap.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WBluetooth = Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace InTheHand.Bluetooth
{
    partial class GattService
    {
        readonly WBluetooth.GattDeviceService _service;

        internal GattService(BluetoothDevice device, WBluetooth.GattDeviceService service) : this(device)
        {
            _service = service;
        }

        public static implicit operator WBluetooth.GattDeviceService(GattService service)
        {
            return service._service;
        }

        async Task<GattCharacteristic> DoGetCharacteristic(BluetoothUuid characteristic)
        {
            var result = await _service.GetCharacteristicsForUuidAsync(characteristic);

            if (result.Status == WBluetooth.GattCommunicationStatus.Success && result.Characteristics.Count > 0)
                return new GattCharacteristic(this, result.Characteristics[0]);

            return null;
        }

        async Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            var result = await _service.GetCharacteristicsAsync();
            if(result.Status == WBluetooth.GattCommunicationStatus.Success)
            {
                foreach(var c in result.Characteristics)
                {
                    characteristics.Add(new GattCharacteristic(this, c));
                }
            }

            return characteristics.AsReadOnly();
        }

        private async Task<GattService> DoGetIncludedServiceAsync(BluetoothUuid service)
        {
            var servicesResult = await _service.GetIncludedServicesForUuidAsync(service);

            if(servicesResult.Status == WBluetooth.GattCommunicationStatus.Success)
            {
                return new GattService(Device, servicesResult.Services[0]);
            }

            return null;
        }

        private async Task<IReadOnlyList<GattService>> DoGetIncludedServicesAsync()
        {
            List<GattService> services = new List<GattService>();

            var servicesResult = await _service.GetIncludedServicesAsync();

            if (servicesResult.Status == WBluetooth.GattCommunicationStatus.Success)
            {
                foreach(var includedService in servicesResult.Services)
                {
                    services.Add(new GattService(Device, includedService));
                }

                return services;
            }

            return null;
        }

        BluetoothUuid GetUuid()
        {
            return _service.Uuid;
        }

        bool GetIsPrimary()
        {
            return _service.ParentServices == null || _service.ParentServices.Count == 0;
        }
    }
}
