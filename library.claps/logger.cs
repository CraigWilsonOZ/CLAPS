// ***********************************************************************
// Assembly         : library.claps
// Author           : Craig Wilson
// Created          : 11-16-2019
//
// Last Modified By : Craig Wilson
// Last Modified On : 11-16-2019
// ***********************************************************************
// <copyright file="logger.cs" company="library.claps">
//     Copyright (c) craigwilson.blog. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace library.claps
{
    /// <summary>
    /// Class Logger.
    /// </summary>
    class Logger
    {
        /// <summary>
        /// Log events as specified message, to both the console and event logs.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logEventLevel">The log event level.</param>
        public void Logevent(string message, string logEventLevel)
        {
            message = DateTime.Now + " - " + message;

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("library.claps.helper", LogLevel.Debug)
                    .AddConsole()
                    .AddEventLog();
            });
            ILogger logger = loggerFactory.CreateLogger<User>();
            switch (logEventLevel)
            {
                case "LogInformation":
                    { logger.LogInformation(message); }
                    break;
                case "LogCritical":
                    { logger.LogCritical(message); }
                    break;
                case "LogDebug":
                    { logger.LogDebug(message); }
                    break;
                case "LogError":
                    { logger.LogError(message); }
                    break;
                case "LogTrace":
                    { logger.LogTrace(message); }
                    break;
                case "LogWarning":
                    { logger.LogWarning(message); }
                    break;
                default:
                    { logger.LogInformation(message); }
                    break;
            }
        }
    }
}
