using System.Windows;
using System.Windows.Media.Imaging;
using Remorse.AutoVote.Models;
using Remorse.AutoVote.Views.Windows;

namespace Remorse.AutoVote.Data
{
    class Settings
    {
        public static Properties.Settings App = Properties.Settings.Default;
        public static MainWindow MainWindow { get; set; }
        private static void SaveAll() => Properties.Settings.Default.Save();
        public static void VKImport()
        {
            string Token = App.Token;
            string Email = App.Email;
            long UserId = App.UserId;
            VKUser.NewUser(Token, Email, UserId);
        }

        public static void VKExport(params dynamic[] data)
        {
            VKUser.Token = App.Token = data[0];
            VKUser.Email = App.Email = data[1];
            VKUser.UserId = App.UserId = data[2];
            SaveAll();
        }

        public static void UpdateProfile(string name, BitmapImage avatar)
        {
            MainWindow.Profile.Visibility = Visibility.Visible;
            MainWindow.ProfileName.Text = name;
            MainWindow.ProfileImage.ImageSource = avatar;
        }
    }
}
