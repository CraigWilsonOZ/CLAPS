// ***********************************************************************
// Assembly         : library.claps
// Author           : Craig Wilson
// Created          : 11-16-2019
//
// Last Modified By : Craig Wilson
// Last Modified On : 11-16-2019
// ***********************************************************************
// <copyright file="user.cs" company="library.claps">
//     Copyright (c) craigwilson.blog. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using library.claps.Models;
using Microsoft.Extensions.Logging;
using System.DirectoryServices.AccountManagement;
using Microsoft.Win32;
using System.Globalization;

namespace library.claps
{
    /// <summary>
    /// Class User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The log
        /// </summary>
        readonly Logger log = new Logger();
        /// <summary>
        /// Processes the user.
        /// </summary>
        public void ProcessUser()
        {
            // Appconfig file details
            Configuration appConfig = new Configuration();
                       
            // Registry Configuration details
            Registryconfiguration myRegistry = new Registryconfiguration();

            string RegistryBaseLocation = @"SOFTWARE\CraigWilson.Blog\CLAPS\";

            // Location of Appconfig.json
            // string baseConfigFileLocation = Directory.GetCurrentDirectory() ;
            string baseConfigFileLocation = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "BaseDirectory"); ;
            string configFile = baseConfigFileLocation + @"\appsettings.json";

            // Value for last update so we can check if updates are required.
            string LastUpdateTime;

            if (File.Exists(configFile))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(baseConfigFileLocation)
                    .AddJsonFile("appsettings.json");

                var config = builder.Build();
                appConfig = config.GetSection("application").Get<Configuration>();
            }
            else
            {
                Console.WriteLine("File appsettings.json does not exist in current directory.");
                log.Logevent($"File appsettings.json does not exist in current directory. {configFile} ", "LogCritical");
            }

            if (appConfig.UseRegistryForConfiguration == "Yes")
            {
                // Overwrite values from registry
                appConfig.AzureKeyVaultClientID = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "KeyVaultClientID");
                appConfig.AzureKeyVaultClientKey = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "KeyVaultSecret");
                appConfig.AzureKeyVaultURL = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "KeyVaultURL");
                appConfig.LocalAccountName = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "LocalAccountName");
                appConfig.LocalAdministratorGroupName = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "LocalAdministratorGroupName");
                appConfig.HoursBeforeUpdate = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "HoursBeforeUpdate");
            }

            // Get the last time the password was changed by service
            LastUpdateTime = myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "LastUpdateTime");
            log.Logevent($"Starting process to create and update Local Admin Account Name : {appConfig.LocalAccountName}", "LogInformation");
            log.Logevent($"Last recorded change to Local Admin Account Name : {LastUpdateTime}", "LogInformation");

            // Convert Last Update time to a data time value
            DateTime.TryParseExact(LastUpdateTime, "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);
            var currentDateTime = DateTime.Now;

            if ((currentDateTime - dateTime).TotalHours >= int.Parse(appConfig.HoursBeforeUpdate))
            {
                log.Logevent($"Local Admin Account Name : {appConfig.LocalAccountName} requires password change.", "LogInformation");
                // Create password
                string newPassword = Password.GenerateRandomPassword();

                // Crfeate or update existing user
                CreateUserOrUpdatePassword(appConfig.LocalAccountName, newPassword);

                // Record update change time
                myRegistry.UpdateDateAndTime(RegistryBaseLocation, "LastUpdateTime");

                // Add user to local group
                AddUsertoGroup(appConfig.LocalAccountName, appConfig.LocalAdministratorGroupName);

                // Store password in Azure Key Vault
                Azkeyvault kv = new Azkeyvault();
                kv.Keyvault(appConfig.AzureKeyVaultURL, appConfig.AzureKeyVaultClientID, appConfig.AzureKeyVaultClientKey, Environment.MachineName, newPassword);
            }
            else
            {
                log.Logevent($"No action required, within time period of last password change.", "LogInformation");
            }
            log.Logevent($"Completed process to create and update Local Admin Account Name : {appConfig.LocalAccountName}", "LogInformation");

        }


        /// <summary>
        /// Checks to see if the user exist.
        /// </summary>
        /// <param name="checkUserName">Name of the check user.</param>
        /// <returns><c>true</c> if true is exists, <c>false</c> otherwise.</returns>
        public bool DoesUserExist(string checkUserName)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine, Environment.MachineName);

            UserPrincipal usr = UserPrincipal.FindByIdentity(ctx,
                                                       IdentityType.SamAccountName,
                                                       checkUserName);
            if (usr != null)
            {
                usr.Dispose();
                ctx.Dispose();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Creates the user or update password.
        /// </summary>
        /// <param name="newUserAccountName">New name of the user account.</param>
        /// <param name="newPassword">The new password.</param>
        public void CreateUserOrUpdatePassword(string newUserAccountName, string newPassword)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine, Environment.MachineName);

            UserPrincipal usr = new UserPrincipal(ctx);

            if (!DoesUserExist(newUserAccountName))
            {
                try
                {
                    log.Logevent($"Creating user: {newUserAccountName}", "LogInformation");
                    usr.Name = newUserAccountName;
                    usr.Description = $"This is the user account for {newUserAccountName}";
                    usr.SetPassword(newPassword);
                    usr.Save();
                    log.Logevent($"Created user: {newUserAccountName}", "LogInformation");
                }
                catch (Exception e)
                {
                    log.Logevent($"{e.Message}", "LogError");
                    usr.Dispose();
                    ctx.Dispose();
                    Environment.Exit(31);
                }

                usr.Dispose();
                ctx.Dispose();
            }else
            {
                try
                {
                    log.Logevent($"Finding user: {newUserAccountName}", "LogInformation");
                    var user = UserPrincipal.FindByIdentity(ctx, newUserAccountName);
                    user.SetPassword(newPassword);
                    user.Save();
                    log.Logevent($"Updating user: {newUserAccountName}", "LogInformation");
                    user.Dispose();
                }
                catch (Exception e)
                {
                    log.Logevent($"{e.Message}", "LogError");
                    usr.Dispose();
                    ctx.Dispose();
                    Environment.Exit(31);
                }

                usr.Dispose();
                ctx.Dispose();
            }

        }

        /// <summary>
        /// Adds the user to local group.
        /// </summary>
        /// <param name="newUserAccountName">New name of the user account.</param>
        /// <param name="localGroupName">Name of the local group.</param>
        public void AddUsertoGroup(string newUserAccountName, string localGroupName)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine, Environment.MachineName);
            try
            {
                GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx,
                                                               IdentityType.Name,
                                                               localGroupName);
                if (grp != null)
                {
                    log.Logevent($"Adding user: {newUserAccountName} to {localGroupName}", "LogInformation");
                    
                    var user = UserPrincipal.FindByIdentity(ctx, newUserAccountName);
                    if (grp.Members.Contains(user))
                    {
                       log.Logevent($"User: {newUserAccountName} already a member of {localGroupName}", "LogInformation");
                    }
                    else
                    {
                        grp.Members.Add(ctx, IdentityType.Name, newUserAccountName);
                        grp.Save();
                        grp.Dispose();
                        log.Logevent($"Added user: {newUserAccountName} to {localGroupName}", "LogInformation");
                    }
                }
            }
            catch (Exception e)
            {
                log.Logevent($"{e.Message}", "LogError");
                ctx.Dispose();
                Environment.Exit(31);
            }
            ctx.Dispose();
        }

    }
}
