using Remorse.AutoVote.Data;
using Remorse.AutoVote.Infrastructure.Commands;
using Remorse.AutoVote.Models;
using Remorse.AutoVote.ViewModels.Base;
using Remorse.AutoVote.Views.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Remorse.AutoVote.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Properties

        #region SelectMenuIndex

        private int selectedMenuIndex = 0;
        public int SelectedMenuIndex
        {
            get => selectedMenuIndex;
            set => Set(ref selectedMenuIndex, value);
        }

        #endregion

        #region ProfileVisibility

        private Visibility vkDialogVisibility = Visibility.Visible;
        private Visibility profileVisibility = Visibility.Hidden;

        public Visibility VkDialogVisibility
        {
            get => vkDialogVisibility;
            set
            {
                Set(ref vkDialogVisibility, value);
            }
        }
        public Visibility ProfileVisibility
        {
            get => profileVisibility;
            set
            {
                VkDialogVisibility = value == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                Set(ref profileVisibility, value);
            }
        }

        #endregion

        #region Projects
        public ObservableCollection<Project> Projects { get; }

        private Project selectedProject;
        public Project SelectedProject
        {
            get => selectedProject;
            set => Set(ref selectedProject, value);
        }

        #endregion

        #endregion

        #region Commands

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }
        private void OnCloseApplicationCommandExecuted(object p) => App.Exit();
        private bool CanCloseApplicationCommandExecute(object p) => true;

        #endregion

        #region SlideMenuCommand

        private Thickness btnSlideMenuMargin = new Thickness(150, -44, -150, 44);
        public Thickness BtnSlideMenuMargin
        {
            get => btnSlideMenuMargin;
            set => Set(ref btnSlideMenuMargin, value);
        }

        private GridLength tabMenuWidth = new GridLength(250);
        public GridLength TabMenuWidth
        {
            get => tabMenuWidth;
            set => Set(ref tabMenuWidth, value);
        }

        public ICommand SlideMenuCommand { get; }
        private void OnSlideMenuCommandExecuted(object p)
        {
            if (tabMenuWidth.Value == 250)
            {
                TabMenuWidth = new GridLength(0);
                BtnSlideMenuMargin = new Thickness(-43, -44, -150, 44);
            }
            else
            {
                TabMenuWidth = new GridLength(250);
                BtnSlideMenuMargin = new Thickness(150, -44, -150, 44);
            }
        }
        private bool CanSlideMenuCommandExecute(object p) => true;

        #endregion

        #region ResetSettingsCommand

        public ICommand ResetSettingsCommand { get; }
        private void OnResetSettingsCommandExecuted(object p)
        {
            var answer = MessageBox.Show("Все сохраненные данные будут сброшены. Вы действительно хотите это сделать?", "Сброс настроек", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
                App.Exit(true);
        }
        private bool CanResetSettingsCommandExecute(object p) => true;

        #endregion

        #region VkAuthCommand

        public ICommand VkAuthCommand { get; }
        private void OnVkAuthCommandExecuted(object p)
        {
            System.Diagnostics.Process.Start(Config.OAuthLink);
            new ReceiverData().GetData("VkAuth");
        }
        private bool CanVkAuthCommandExecute(object p) => true;

        #endregion

        #region OpenVkProfileCommand

        public ICommand OpenVkProfileCommand { get; }
        private void OnOpenVkProfileCommandExecuted(object p) => System.Diagnostics.Process.Start("https://vk.com/id" + VKUser.UserId);
        private bool CanOpenVkProfileCommandExecute(object p) => true;

        #endregion

        #region SelectProjectCommand

        public ICommand SelectProjectCommand { get; }
        private void OnSelectProjectCommandExecuted(object Project)
        {
            new ProjectWindow(SelectedProject)
            {
                Owner = Application.Current.MainWindow,
                ShowInTaskbar = false
            }.Show();
        }
        private bool CanSelectProjectCommandExecute(dynamic Project) => (SelectedProject = Project) != null && !Application.Current.Windows.OfType<ProjectWindow>().Any(x => ((dynamic)x.DataContext).Project == SelectedProject);

        #endregion

        #region VkLeaveCommand

        public ICommand VkLeaveCommand { get; }
        private void OnVkLeaveCommandExecuted(object p)
        {
            ProfileVisibility = Visibility.Hidden;
            Settings.VKExport(null, null, 0);
        }
        private bool CanVkLeaveCommandExecute(object p) => true;

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region InitProperties

            var projectsData = Database.GetData<Project>("Projects");
            Projects = new ObservableCollection<Project>(projectsData);

            #endregion

            #region InitCommand

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            SlideMenuCommand = new LambdaCommand(OnSlideMenuCommandExecuted, CanSlideMenuCommandExecute);
            ResetSettingsCommand = new LambdaCommand(OnResetSettingsCommandExecuted, CanResetSettingsCommandExecute);
            VkAuthCommand = new LambdaCommand(OnVkAuthCommandExecuted, CanVkAuthCommandExecute);
            OpenVkProfileCommand = new LambdaCommand(OnOpenVkProfileCommandExecuted, CanOpenVkProfileCommandExecute);
            VkLeaveCommand = new LambdaCommand(OnVkLeaveCommandExecuted, CanVkLeaveCommandExecute);
            SelectProjectCommand = new LambdaCommand(OnSelectProjectCommandExecuted, CanSelectProjectCommandExecute);

            #endregion

            #region InitEvents

            Projects.CollectionChanged += (s, e) => Database.SetData(Projects);

            #endregion
        }
    }
}
