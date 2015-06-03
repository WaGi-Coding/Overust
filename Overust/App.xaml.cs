using ITK.ModelManager;
using Rust;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Overust
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ModelManager ModelManager = new ModelManager("UserData");
        public static Rcon RustRcon = new Rcon();
        public static LogManager LogManager = new LogManager();
        public static EventManager EventManager = new EventManager();
    }

}
