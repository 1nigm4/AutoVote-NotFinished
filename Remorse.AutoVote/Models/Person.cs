using Remorse.AutoVote.ViewModels.Base;
using System;

namespace Remorse.AutoVote.Models
{
    public class Person : ViewModel
    {
        private string nick;
        public string Nick
        {
            get => nick;
            set => Set(ref nick, value);
        }
        private Uri skin;
        public Uri Skin
        {
            get => skin;
            set => Set(ref skin, value);
        }
    }
}
