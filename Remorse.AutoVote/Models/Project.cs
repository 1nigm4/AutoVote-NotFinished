using System;
using System.Collections.ObjectModel;

namespace Remorse.AutoVote.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Skins { get; set; }
        public Uri Logo { get; set; }
        public ObservableCollection<Person> Persons { get; set; }
    }
}
