using Overust.ViewModels.SettingsViewModels;
using System;
using System.Collections.Generic;
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

namespace Overust.Views.SettingsViews
{
    /// <summary>
    /// Interaction logic for AutoModerationSettingsView.xaml
    /// </summary>
    public partial class AutoModerationSettingsView : UserControl
    {
        public AutoModerationSettingsViewModel ViewModel {get; set;}

        public AutoModerationSettingsView()
        {
            ViewModel = new AutoModerationSettingsViewModel();
            InitializeComponent();
        }

        private void PingKickTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!char.IsDigit(e.Text.Single()))
            {
                e.Handled = true;
            }
        }
    }
}
