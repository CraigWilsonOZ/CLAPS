// ***********************************************************************
// Assembly         : library.claps
// Author           : Craig Wilson
// Created          : 11-16-2019
//
// Last Modified By : Craig Wilson
// Last Modified On : 11-16-2019
// ***********************************************************************
// <copyright file="registryconfiguration.cs" company="library.claps">
//     Copyright (c) craigwilson.blog. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace library.claps
{
    /// <summary>
    /// Class Registryconfiguration.
    /// </summary>
    public class Registryconfiguration
    {
        /// <summary>
        /// Determines whether [is registry availble].
        /// </summary>
        /// <returns><c>true</c> if [is registry availble]; otherwise, <c>false</c>.</returns>
        public bool IsRegistryAvailble()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the date and time.
        /// </summary>
        /// <param name="registryLocation">The registry location.</param>
        /// <param name="registryKey">The registry key.</param>
        public void UpdateDateAndTime(string registryLocation, string registryKey)
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(registryLocation, true);
            if (key != null)
            {
                key.SetValue(registryKey, $"{DateTime.Now:yyyyMMdd-HHmmss}");
            }
            else
            {
            }
        }

        /// <summary>
        /// Reads the HKLM string value.
        /// </summary>
        /// <param name="registryLocation">The registry location.</param>
        /// <param name="registrySubKey">The registry sub key.</param>
        /// <returns>System.String.</returns>
        public string ReadHKLMStringValue(string registryLocation, string registrySubKey)
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(registryLocation);
            if (key != null)
            {
                return key.GetValue(registrySubKey).ToString();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Reads the HKLM int value.
        /// </summary>
        /// <param name="registryLocation">The registry location.</param>
        /// <param name="registryKey">The registry key.</param>
        /// <returns>System.Int32.</returns>
        public int ReadHKLMIntValue(string registryLocation, string registryKey)
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@registryLocation);
            if (key != null)
            {
                return Int32.Parse(key.GetValue(registryKey).ToString());
            }
            else
            {
                return -1;
            }
        }

    }
}
