// ***********************************************************************
// Assembly         : library.claps
// Author           : Craig Wilson
// Created          : 11-16-2019
//
// Last Modified By : Craig Wilson
// Last Modified On : 11-16-2019
// ***********************************************************************
// <copyright file="azkeyvault.cs" company="library.claps">
//     Copyright (c) craigwilson.blog. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using library.claps;

namespace library.claps
{
    /// <summary>
    /// Class Azkeyvault.
    /// </summary>
    class Azkeyvault
    {
        /// <summary>
        /// The log
        /// </summary>
        readonly Logger log = new Logger();
        /// <summary>
        /// Initializes a new instance of the <see cref="Azkeyvault"/> class.
        /// </summary>
        public Azkeyvault()
        {

        }

        /// <summary>
        /// Keyvaults the specified keyvault URL.
        /// </summary>
        /// <param name="keyvaultURL">The keyvault URL.</param>
        /// <param name="keyvaultClientId">The keyvault client identifier.</param>
        /// <param name="keyvaultSecret">The keyvault secret.</param>
        /// <param name="kvSecretName">Name of the kv secret.</param>
        /// <param name="kvSecretValue">The kv secret value.</param>
        public void Keyvault(string keyvaultURL, string keyvaultClientId, string keyvaultSecret, string kvSecretName, string kvSecretValue)
        {
            Azkeyvault P = new Azkeyvault();

            string kvURL = keyvaultURL;
            string clientId = keyvaultClientId;
            string clientSecret = keyvaultSecret;
            string secretName = kvSecretName;
            string secretValue = kvSecretValue;

            // <authentication>
            log.Logevent($"Connecting to KeyVault: {kvURL}", "LogInformation");
            KeyVaultClient kvClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(clientId, clientSecret);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });
            // </authentication>

            // <setsecret>
            log.Logevent($"Setting secret for: {kvSecretName}", "LogInformation");
            var result = P.SetSecret(kvClient, kvURL, secretName, secretValue);
            // </setsecret>
            System.Threading.Thread.Sleep(5000);
            log.Logevent($"Updated secret for: {kvSecretName}", "LogInformation");
        }



        /// <summary>
        /// Sets the secret.
        /// </summary>
        /// <param name="kvClient">The kv client.</param>
        /// <param name="kvURL">The kv URL.</param>
        /// <param name="secretName">Name of the secret.</param>
        /// <param name="secretValue">The secret value.</param>
        /// <returns>The created or the updated secret</returns>
        public async Task<bool> SetSecret(KeyVaultClient kvClient, string kvURL, string secretName, string secretValue)
        {
            // <setsecret>
            await kvClient.SetSecretAsync($"{kvURL}", secretName, secretValue);
            // </setsecret>

            return true;
        }

        /// <summary>
        /// Gets the secret.
        /// </summary>
        /// <param name="kvClient">The kv client.</param>
        /// <param name="kvURL">The kv URL.</param>
        /// <param name="secretName">Name of the secret.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetSecret(KeyVaultClient kvClient, string kvURL, string secretName)
        {
            // <getsecret>                
            var keyvaultSecret = await kvClient.GetSecretAsync($"{kvURL}", secretName).ConfigureAwait(false);
            // </getsecret>
            return keyvaultSecret.Value;
        }
    }
}

