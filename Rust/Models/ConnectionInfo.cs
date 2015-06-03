using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rust.Models
{
    public interface ConnectionInfo
    {
        string Hostname {get; set;}
        int Port {get; set;}
        string Password {get; set;}
    }
}
