using LiteDB;
using Remorse.AutoVote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Remorse.AutoVote.Data
{
    class Database
    {
        private static LiteDatabase _sDataBase = new LiteDatabase(App.Folder + "\\Data.db");

        public static IEnumerable<T> GetData<T>(string Base)
        {
            var collection = _sDataBase.GetCollection<T>(Base);
            return new ObservableCollection<T>(collection.FindAll().ToList());
        }

        public static async void SetData<T>(IEnumerable<T> Data)
        {
            await Task.Run(() =>
            {
                var collection = _sDataBase.GetCollection<T>(typeof(T).Name + "s");
                collection.Upsert(Data);
            });
        }

        public static async void UpdateData<T>(int Id, object Data)
        {
            await Task.Run(() =>
            {
                var collection = _sDataBase.GetCollection<T>(typeof(T).Name + "s");
                dynamic element = collection.FindById(Id);
                element = Data;
                collection.Update(element);
            });
        }

        public static void CreateBase()
        {
            List<Project> projects = new List<Project>()
            {
                new Project() { Title = "Optimine", Skins = "https://optimine.su/", Logo = new Uri(Config.WebFavicon + "Optimine.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "VictoryCraft", Skins = "http://skins.victorycraft.ru/avatars/96/{Nick}.png", Logo = new Uri(Config.WebFavicon + "VictoryCraft.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "McSkill", Skins = "https://mcskill.net/MineCraft/?mode=5&fx=43&fy=43&name={Nick}", Logo = new Uri(Config.WebFavicon + "McSkill.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "MythicalWorld", Skins = "https://mythicalworld.su/skins/face.php?size=50&username={Nick}", Logo = new Uri(Config.WebFavicon + "MythicalWorld.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "SimpleMinecraft", Skins = "https://api.simpleminecraft.ru/images/skins/front/{Nick}.png", Logo = new Uri(Config.WebFavicon + "SimpleMinecraft.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "LavaCraft", Skins = "https://imglavacraft.ru/tmp/{Nick}_Mini.png", Logo = new Uri(Config.WebFavicon + "LavaCraft.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "Borealis", Skins = "https://borealis.su/viewskin.php?skin={Nick}.png", Logo = new Uri(Config.WebFavicon + "Borealis.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "ExcaliburCraft", Skins = "https://excalibur-craft.ru/engine/ajax/lk/skin3d.php?login={Nick}&mode=profile", Logo = new Uri(Config.WebFavicon + "ExcaliburCraft.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "MinecraftOnly", Skins = "https://download.minecraftonly.ru/renderskin.php?user={Nick}", Logo = new Uri(Config.WebFavicon + "MinecraftOnly.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "PentaCraft", Skins = "https://pentacraft.ru/uploads/{Nick}.png", Logo = new Uri(Config.WebFavicon + "PentaCraft.png"), Persons = new ObservableCollection<Person>() },
                new Project() { Title = "MythicalPlanet", Skins = "https://mcmp.su/head/{Nick}/40", Logo = new Uri(Config.WebFavicon + "MythicalPlanet.png"), Persons = new ObservableCollection<Person>() }
            };
            var collection = _sDataBase.GetCollection<Project>("Projects");
            collection.DeleteAll();
            collection.Insert(projects);
        }

        public static void Dispose()
        {
            _sDataBase?.Dispose();
        }
    }
}
