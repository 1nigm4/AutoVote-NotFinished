using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Remorse.AutoVote.Models;
using Remorse.AutoVote.Data;
using System.Windows.Threading;

namespace Remorse.AutoVote.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Settings.MainWindow = this;
            Settings.VKImport();
            VKUser.Synchronization();
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