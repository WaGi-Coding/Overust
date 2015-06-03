using Overust.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace Overust.Views
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView : UserControl
    {
        public ConsoleViewModel ViewModel {get; set;}

        public ConsoleView()
        {
            ViewModel = new ConsoleViewModel();
            App.RustRcon.ConsoleLogUpdated += (s, args) =>
                {
                    if(ViewModel.ConsoleSettings.IsAutoScrollEnabled)
                        ConsoleScrollViewer.ScrollToBottom();
                };
            InitializeComponent();
        }
        
        private void CommandBoxKeyDown(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;

            if(textbox != null)
            {
                if(e.Key == Key.Enter)
                {
                    App.RustRcon.ExecCommand(textbox.Text, true);
                    textbox.Clear();
                }
            }
        }

        private void ClearConsoleButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Console = "";
        }
    }
}
