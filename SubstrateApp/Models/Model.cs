using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstrateApp.Models
{
    public class AppMenu
    {
        public string Name { get; set; }

        public string Page { get; set; }
        public string Tag { get; internal set; }
        public string Icon { get; internal set; }
    }
}
