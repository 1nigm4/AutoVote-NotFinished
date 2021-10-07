using Remorse.AutoVote.Data;
using Remorse.AutoVote.Infrastructure.Commands;
using Remorse.AutoVote.Models;
using Remorse.AutoVote.ViewModels.Base;
using Remorse.AutoVote.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Remorse.AutoVote.ViewModels
{
    internal class ProjectWindowViewModel : ViewModel
    {
        #region Properties

        #region Project
        public Project Project { get; set; }

        #endregion

        #region Persons

        private ObservableCollection<Person> persons;
        public ObservableCollection<Person> Persons
        {
            get => persons;
            set => Set(ref persons, value);
        }

        #endregion

        #region SelectedPerson

        private Person selectedPerson;
        private string selectedPersonNick;
        private Uri selectedPersonSkin;
        public Person SelectedPerson
        {
            get => selectedPerson;
            set => Set(ref selectedPerson, value);
        }

        #endregion

        #region PersonsMargin

        private Thickness personsMargin;
        public Thickness PersonsMargin
        {
            get => personsMargin;
            set => Set(ref personsMargin, value);
        }

        #endregion

        #region Slider

        private Visibility sliderVisibility;
        public Visibility SliderVisibility
        {
            get => sliderVisibility;
            set
            {
                Set(ref sliderVisibility, value);
                if (value == Visibility.Collapsed) SliderValue = 0;
            }
        }

        private int _SliderMaximum;
        public int SliderMaximum
        {
            get => _SliderMaximum;
            set => Set(ref _SliderMaximum, value);
        }

        private int _SliderValue = 0;
        public int SliderValue
        {
            get => _SliderValue;
            set
            {
                Set(ref _SliderValue, value);
                PersonsMargin = new Thickness(-value, 0, 0, 0);
            }
        }

        #endregion

        #region VotePersonVisibility

        private Visibility votePersonVisibility = Visibility.Hidden;
        public Visibility VotePersonVisibility
        {
            get => votePersonVisibility;
            set 
            {
                Set(ref votePersonVisibility, value);
                ChoosePersonVisibility = value == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            }
        }

        #endregion

        #region ChoosePersonVisibility

        private Visibility choosePersonVisibility;
        public Visibility ChoosePersonVisibility
        {
            get => choosePersonVisibility;
            set => Set(ref choosePersonVisibility, value);
        }

        #endregion

        #region EditPersonVisibility

        private Visibility editPersonVisibility = Visibility.Hidden;
        public Visibility EditPersonVisibility
        {
            get => editPersonVisibility;
            set
            {
                Set(ref editPersonVisibility, value);
                ChoosePersonVisibility = value == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            }
        }

        #endregion

        #region Skin

        private string[] projectHasFullSkin = { "SimpleMinecraft", "Borealis", "ExcaliburCraft", "MinecraftOnly" };

        #region PersonSkinSize

        private int _PersonSkinHeight;
        private int _EditPersonSkinHeight;

        public int PreviewPersonSkinHeight
        {
            get => _PersonSkinHeight;
            set => Set(ref _PersonSkinHeight, value);
        }
        
        public int PersonSkinHeight
        {
            get => _EditPersonSkinHeight;
            set => Set(ref _EditPersonSkinHeight, value);
        }

        #endregion

        #endregion

        #endregion

        #region Commands

        #region HideProjectWindowCommand

        public ICommand CloseProjectWindowCommand { get; }
        private void OnCloseProjectWindowCommandExecuted(object window)
        {
            Application.Current.Windows.OfType<ProjectWindow>().First(x => x.DataContext == this).Close();
            Application.Current.Windows.OfType<MainWindow>().First().WindowState = WindowState.Normal;
            Persons.CollectionChanged -= Persons_onCollectionChanged;
            Database.UpdateData<Project>(Project.Id, Project);
        }

        private bool CanCloseProjectWindowCommandExecute(object p) => true;

        #endregion

        #region AddPersonCommand

        public ICommand AddPersonCommand { get; }
        private void OnAddPersonCommandExecuted(object p)
        {
            Persons.Add(new Person() { Nick = "Player" });
        }
        private bool CanAddPersonCommandExecute(object p) => Persons.Count < 10;

        #endregion

        #region EditPersonCommand

        public ICommand EditPersonCommand { get; }
        private void OnEditPersonCommandExecuted(object p)
        {
            selectedPersonNick = SelectedPerson.Nick;
            selectedPersonSkin = SelectedPerson.Skin;
            EditPersonVisibility = Visibility.Visible;
        }
        private bool CanEditPersonCommandExecute(object p) => SelectedPerson != null;

        #endregion

        #region RefreshPersonSkinCommand

        public ICommand RefreshPersonSkinCommand { get; }
        private void OnRefreshPersonSkinCommandExecuted(object p)
        {
            string skinUrl = Project.Skins.Replace("{Nick}", SelectedPerson.Nick);
            SelectedPerson.Skin = new Uri(skinUrl);
        }
        private bool CanRefreshPersonSkinCommandExecute(object p) => !string.IsNullOrWhiteSpace(SelectedPerson?.Nick);

        #endregion

        #region HideEditPersonCommand

        public ICommand HideEditPersonCommand { get; }
        private void OnHideEditPersonCommandExecuted(object Command)
        {
            if (Command as string == "Save")
                Database.UpdateData<Project>(Project.Id, Project);
            else
            {
                SelectedPerson.Nick = selectedPersonNick;
                SelectedPerson.Skin = selectedPersonSkin;
            }
                
            EditPersonVisibility = Visibility.Hidden;
        }
        private bool CanHideEditPersonCommandExecute(object p) => true;

        #endregion

        #region VotePersonCommand

        public ICommand VotePersonCommand { get; }
        private void OnVotePersonCommandExecuted(object p) => 
            VotePersonVisibility = VotePersonVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

        private bool CanVotePersonCommandExecute(object p) => SelectedPerson != null;

        #endregion

        #region DeletPersonCommand

        public ICommand DeletePersonCommand { get; }
        private void OnDeletePersonCommandExecuted(object p) => Persons.Remove(SelectedPerson);

        private bool CanDeletePersonCommandExecute(object p) => SelectedPerson != null;

        #endregion

        #endregion

        #region Events

        private void Persons_onCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SliderVisibility = Persons.Count > 3 ? Visibility.Visible : Visibility.Collapsed;
            SliderMaximum = (Persons.Count - 3) * 80;
            SliderValue = SliderVisibility == Visibility.Visible ? SliderMaximum : 0;
        }

        #endregion

        public ProjectWindowViewModel(Project SelectedProject)
        {
            #region InitProperties

            Project = SelectedProject;
            Persons = Project.Persons;
            PreviewPersonSkinHeight = projectHasFullSkin.Contains(Project.Title) ? 75 : 21;
            PersonSkinHeight = projectHasFullSkin.Contains(Project.Title) ? 145 : 40;

            #endregion

            #region InitCommands

            CloseProjectWindowCommand = new LambdaCommand(OnCloseProjectWindowCommandExecuted, CanCloseProjectWindowCommandExecute);
            AddPersonCommand = new LambdaCommand(OnAddPersonCommandExecuted, CanAddPersonCommandExecute);
            EditPersonCommand = new LambdaCommand(OnEditPersonCommandExecuted, CanEditPersonCommandExecute);
            DeletePersonCommand = new LambdaCommand(OnDeletePersonCommandExecuted, CanDeletePersonCommandExecute);
            HideEditPersonCommand = new LambdaCommand(OnHideEditPersonCommandExecuted, CanHideEditPersonCommandExecute);
            RefreshPersonSkinCommand = new LambdaCommand(OnRefreshPersonSkinCommandExecuted, CanRefreshPersonSkinCommandExecute);
            VotePersonCommand = new LambdaCommand(OnVotePersonCommandExecuted, CanVotePersonCommandExecute);

            #endregion

            #region InitEvents

            Persons.CollectionChanged += Persons_onCollectionChanged;
            Persons_onCollectionChanged(null, null);

            #endregion
        }
    }
}
