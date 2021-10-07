using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Remorse.AutoVote.Data
{
    class GetStarted
    {
        public static void Start()
        {
            try
            {
                var protocolFullPath = App.Folder + "\\"+ "Protocol.exe";

                RegistryKey remorse = Registry.ClassesRoot.CreateSubKey("remorse");
                remorse.SetValue("", "URL:remorse Protocol");
                remorse.SetValue("URL Protocol", "");
                remorse.CreateSubKey(@"shell\open\command").SetValue("", protocolFullPath + " \"%1\"");

                Directory.CreateDirectory(App.Folder);
                File.WriteAllBytes(protocolFullPath, Properties.Resources.Protocol);

                Database.CreateBase();
            }
            catch
            {
                MessageBox.Show("Первый запуск требует права администратора!", "Контроль доступа", MessageBoxButton.OK, MessageBoxImage.Information);
                App.Exit();
            }
        }
    }
}
