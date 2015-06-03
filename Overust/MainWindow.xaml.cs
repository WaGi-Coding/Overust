using Overust.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ITK.Extensions;
using System.IO;

namespace Overust
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GeneralSettings GeneralSettings {get; set;}

        public MainWindow()
        {
            GeneralSettings = App.ModelManager.Get<UserSettings>().GeneralSettings;
            InitializeComponent();
        }

        private void ApplicationClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.ModelManager.SaveAll();
            App.LogManager.SaveLog();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            App.RustRcon.Run();
            MainLoop();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.StopFlashing();
        }



        private async Task MainLoop()
        {
            App.EventManager.Events.Add(new TimedEvent(20000, () =>
                {
                    if(App.RustRcon.IsConnected)
                        App.RustRcon.GetStatus();
                }));

            App.RustRcon.ServerConnectionSucceeded += (s, args) =>
                {
                    foreach(var timedEvent in App.EventManager.Events)
                    {
                        timedEvent.Reset();
                    }
                };

            while(true)
            {
                App.EventManager.Update();
                await Task.Delay(1000);
            }
        }
    }
}
