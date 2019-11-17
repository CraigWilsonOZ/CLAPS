// ***********************************************************************
// Assembly         : library.claps
// Author           : Craig Wilson
// Created          : 11-16-2019
//
// Last Modified By : Craig Wilson
// Last Modified On : 11-16-2019
// ***********************************************************************
// <copyright file="Configuration.cs" company="library.claps">
//     Copyright (c) craigwilson.blog. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace library.claps.Models
{
    /// <summary>
    /// Class Configuration.
    /// </summary>
    [JsonObject("application")]
    class Configuration
    {
        /// <summary>
        /// Gets or sets the name of the local account.
        /// </summary>
        /// <value>The name of the local account.</value>
        [JsonProperty("LocalAccountName")]
        public string LocalAccountName { get; set; }

        /// <summary>
        /// Gets or sets the name of the local administrator group.
        /// </summary>
        /// <value>The name of the local administrator group.</value>
        [JsonProperty("LocalAdministratorGroupName")]
        public string LocalAdministratorGroupName { get; set; }

        /// <summary>
        /// Gets or sets the azure key vault URL.
        /// </summary>
        /// <value>The azure key vault URL.</value>
        [JsonProperty("AzureKeyVaultURL")]
        public string AzureKeyVaultURL { get; set; }

        /// <summary>
        /// Gets or sets the azure key vault client identifier.
        /// </summary>
        /// <value>The azure key vault client identifier.</value>
        [JsonProperty("AzureKeyVaultClientID")]
        public string AzureKeyVaultClientID { get; set; }

        /// <summary>
        /// Gets or sets the azure key vault client key.
        /// </summary>
        /// <value>The azure key vault client key.</value>
        [JsonProperty("AzureKeyVaultClientKey")]
        public string AzureKeyVaultClientKey { get; set; }

        /// <summary>
        /// Gets or sets the hours before update.
        /// </summary>
        /// <value>The hours before update.</value>
        [JsonProperty("HoursBeforeUpdate")]
        public string HoursBeforeUpdate { get; set; }

        /// <summary>
        /// Gets or sets the use registry for configuration.
        /// </summary>
        /// <value>The use registry for configuration.</value>
        [JsonProperty("UseRegistryForConfiguration")]
        public string UseRegistryForConfiguration { get; set; }
    }
}
