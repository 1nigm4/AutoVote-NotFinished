using Remorse.AutoVote.Models;
using Remorse.AutoVote.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Remorse.AutoVote.Views.Windows
{
    public partial class ProjectWindow : Window
    {
        public ProjectWindow(Project SelectedProject)
        {
            InitializeComponent();

            DataContext = new ProjectWindowViewModel(SelectedProject);
        }

        #region WindowMoveEvent

        private Point _mouseOffset;
        private bool _parentIsGrid;
        private bool _mousePressed;

        private void Window_onMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!_mousePressed)
                {
                    _mousePressed = true;
                    _mouseOffset = e.GetPosition(this);
                    dynamic control = InputHitTest(_mouseOffset);
                    _parentIsGrid = control?.Parent is Grid || control?.TemplatedParent?.Parent is Grid;
                }
                else if (_parentIsGrid)
                {
                    var mouseScreen = PointToScreen(Mouse.GetPosition(this));
                    this.Left = mouseScreen.X - _mouseOffset.X;
                    this.Top = mouseScreen.Y - _mouseOffset.Y;
                }
            }
            else _mousePressed = false;
        }

        #endregion

    }
}
