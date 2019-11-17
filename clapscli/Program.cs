// ***********************************************************************
// Assembly         : clapscli
// Author           : Craig Wilson
// Created          : 11-16-2019
//
// Last Modified By : Craig Wilson
// Last Modified On : 11-16-2019
// ***********************************************************************
// <copyright file="Program.cs" company="clapscli">
//     Copyright (c) craigwilson.blog. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using library.claps;


namespace clapscli
{
    /// <summary>
    /// Class Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Help Menu.
        /// </summary>
        static void HelpMenu()
        {
            Console.WriteLine("CLAPS - Cloud Local Administrator Password Server Command Line Tool.");
            Console.WriteLine();
            Console.WriteLine(" Created by : Craig Wilson");
            Console.WriteLine(" Email      : me@craigwilson.blog");
            Console.WriteLine();
            Console.WriteLine("Usage: clapscli -<option> ");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  To create a new local admin account or update an existing one.");
            Console.WriteLine("    clapscli -LocalAdmin ");
            Console.WriteLine("");
            Console.WriteLine("  To install the service.");
            Console.WriteLine("    clapscli -Installsvc ");
            Console.WriteLine("");
            Console.WriteLine("  To uninstall the service.");
            Console.WriteLine("    clapscli -uninstallsvc ");
            Console.WriteLine("");
        }
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Int32.</returns>
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                HelpMenu();
                return 1;
            }

            switch (args[0].ToLower())
            {
                case "-localadmin":
                    { 
                        User CreateUpdateAdmin = new User();
                        CreateUpdateAdmin.ProcessUser();
                    }
                    break;
                case "-installsvc":
                    { Installsvc(); }
                    break;
                case "-uninstallsvc":
                    { Uninstallsvc(); }
                    break;
                default:
                    { HelpMenu(); }
                    break;
            }


            return 0;
        }

        /// <summary>
        /// Install the clapssvc using system process 'sc' to the local machine.
        /// </summary>
        private static void Installsvc()
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = @"c:\Windows\System32\sc.exe";
                    myProcess.StartInfo.Arguments = "create CLAPS_Service BinPath=\"C:\\Program Files\\CraigWilson.Blog\\CLAPS\\clapssvc.exe\" start=auto";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = @"c:\Windows\System32\sc.exe";
                    myProcess.StartInfo.Arguments = "start CLAPS_Service";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Uninstall the clapssvc using system process 'sc' from the local machine.
        /// </summary>
        private static void Uninstallsvc()
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = @"c:\Windows\System32\sc.exe";
                    myProcess.StartInfo.Arguments = "stop CLAPS_Service";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = @"c:\Windows\System32\sc.exe";
                    myProcess.StartInfo.Arguments = "delete CLAPS_Service";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

