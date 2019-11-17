// ***********************************************************************
// Assembly         : clapssvc
// Author           : Craig Wilson
// Created          : 11-16-2019
//
// Last Modified By : Craig Wilson
// Last Modified On : 11-16-2019
// ***********************************************************************
// <copyright file="service.cs" company="clapssvc">
//     Copyright (c) craigwilson.blog. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.ServiceProcess;

using library.claps;

namespace clapssvc
{
    /// <summary>
    /// Class Program.
    /// </summary>
    class Program
    {

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        static void Main()
        {
            ServiceBase.Run(new CoreService());
        }
    }

    /// <summary>
    /// Class CoreService.
    /// Implements the <see cref="System.ServiceProcess.ServiceBase" />
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    class CoreService : ServiceBase
    {
        /// <summary>
        /// The pathtolog
        /// </summary>
        private readonly static string pathtolog = Path.GetTempPath() + "servicelog.txt";
        /// <summary>
        /// The log file location
        /// </summary>
        private readonly string _logFileLocation = pathtolog;

        /// <summary>
        /// The timer
        /// </summary>
        System.Timers.Timer _timer;
        /// <summary>
        /// The schedule time
        /// </summary>
        DateTime _scheduleTime;
        /// <summary>
        /// The create admin user
        /// </summary>
        readonly User CreateAdminUser = new User();

        /// <summary>
        /// Logs the specified log message.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.UtcNow.ToString() + " : " + logMessage + Environment.NewLine);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            Log("Starting");

            // Registry Configuration details
            Registryconfiguration myRegistry = new Registryconfiguration();

            string RegistryBaseLocation = @"SOFTWARE\CraigWilson.Blog\CLAPS\";

            int HourOfDayForReset = int.Parse(myRegistry.ReadHKLMStringValue(RegistryBaseLocation, "HourOfDayForReset"));

            _timer = new System.Timers.Timer();
            _scheduleTime = DateTime.Today.AddDays(0).AddHours(HourOfDayForReset); // Schedule to run once a day at 9:00 a.m.

            base.OnStart(args);
            CreateAdminUser.ProcessUser();
            // For first time, set amount of seconds between current time and schedule time
            _timer.Enabled = true;
            _timer.Interval = _scheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);



        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CreateAdminUser.ProcessUser();

            if (_timer.Interval != 24 * 60 * 60 * 1000)
            {
                _timer.Interval = 24 * 60 * 60 * 1000;
            }
        }
        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            Log("Stopping");
            base.OnStop();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Pause command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service pauses.
        /// </summary>
        protected override void OnPause()
        {
            Log("Pausing");
            base.OnPause();
        }
    }
}
