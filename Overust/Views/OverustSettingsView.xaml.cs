using Overust.ViewModels;
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

namespace Overust.Views
{
    /// <summary>
    /// Interaction logic for OverustSettingsView.xaml
    /// </summary>
    public partial class OverustSettingsView : UserControl
    {
        public OverustSettingsViewModel ViewModel {get; set;}

        public OverustSettingsView()
        {
            ViewModel = new OverustSettingsViewModel();
            InitializeComponent();
        }
    }
}
