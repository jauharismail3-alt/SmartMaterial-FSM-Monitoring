// ========================================================================
// SMART MANUFACTURING MONITORING SYSTEM - PROGRAM ENTRY POINT
// ITS Surabaya - Instrumentation Department
// 
// File: Program.cs
// Application entry point
// ========================================================================

using System;
using System.Windows.Forms;

namespace SmartSCADA
{
    /// <summary>
    /// Main program entry point for Smart Manufacturing Monitoring System
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable visual styles for modern Windows appearance
            Application.EnableVisualStyles();

            // Set text rendering to compatible mode
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the main form
            Application.Run(new ScadaForm());
        }
    }
}