using Remorse.AutoVote.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Remorse.AutoVote
{
    public partial class App : Application
    {
        public static string Folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Remorse";
        public App() : base()
        {
            Dispatcher.UnhandledException += (s, e) =>
            {
                var exception = e.Exception;
                while (exception.InnerException != null)
                    exception = exception.InnerException;
                MessageBox.Show(exception.Message, "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
                if (!Current.MainWindow.IsLoaded)
                    Exit();
                e.Handled = true;
            };

            if (!Directory.Exists(Folder) || !File.Exists(Folder + "\\Protocol.exe"))
                GetStarted.Start();
        }

        public static new void Exit(bool RestoreData = false)
        {
            Database.Dispose();
            if (RestoreData)
            {
                File.Delete(Folder + "\\" + "Protocol.exe");
                Process.Start(Assembly.GetExecutingAssembly().Location, "runas");
            }
            Process.GetCurrentProcess().Kill();
        }
    }
}
