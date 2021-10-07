using System.Windows;
using System.Windows.Threading;
using Remorse.AutoVote.Models;

namespace Remorse.AutoVote.Data
{
    public class ReceiverData
    {
        private object[] data;
        private string state;
        private DispatcherTimer reciever;

        public void GetData(string state)
        {
            this.state = state;
            reciever = new DispatcherTimer();
            reciever.Tick += Reciever_onTick;
            reciever.Interval = System.TimeSpan.FromSeconds(1);
            reciever.Start();
        }

        private int _ticks = 0;
        private const int _MAXTICKS = 3600;
        private void Reciever_onTick(object sender, System.EventArgs e)
        {
            _ticks++;
            data = Clipboard.GetData(state) as object[];
            if (data != null)
            {
                reciever.Stop();
                Clipboard.Clear();
                Reciever_onDone();
            }
            if (_ticks == _MAXTICKS)
                reciever.Stop();
        }

        private void Reciever_onDone()
        {
            if (state == "VkAuth")
            {
                Settings.VKExport(data);
                VKUser.Synchronization();
            }
        }
    }
}
