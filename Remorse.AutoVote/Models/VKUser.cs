using System.Linq;
using System.Windows.Media.Imaging;
using VkNet;
using VkNet.Model;
using Remorse.AutoVote.Data;
using System.Windows;

namespace Remorse.AutoVote.Models
{
    class VKUser
    {
        private static string token;
        public static string Token
        {
            get => token;
            set => token = value;
        }

        private static string email;
        public static string Email 
        {
            get => email;
            set => email = value;
        }

        private static long userId;
        public static long UserId
        {
            get => userId;
            set => userId = value;
        }

        public static void NewUser(params dynamic[] data)
        {
            token = data[0];
            email = data[1];
            userId = data[2];
        }

        public static void Synchronization()
        {
            try
            {
                VkApi vk = new VkApi();
                vk.Authorize(new ApiAuthParams { AccessToken = Token });

                User user = vk.Users.Get(new long[] { UserId }, VkNet.Enums.Filters.ProfileFields.Photo50).FirstOrDefault();
                string name = $"{user.FirstName} {user.LastName}";
                var avatar = new BitmapImage(user.Photo50);
                Settings.UpdateProfile(name, avatar);
            }
            catch
            {
                if (Token != string.Empty)
                {
                    MessageBox.Show("Ошибка синхронизации пользователя, авторизируйтесь повторно", "Синхронизация");
                    Settings.VKExport(null, null, 0);
                }
            }
        }
    }
}
