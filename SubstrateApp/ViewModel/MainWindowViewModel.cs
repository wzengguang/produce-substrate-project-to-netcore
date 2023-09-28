
namespace SubstrateApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using SubstrateApp.Models;

    public class MainWindowViewModel : ObservableRecipient
    {
        public List<AppMenu> Menues { get; } = new List<AppMenu>
        {
            new AppMenu{ Name = "ProduceAssemly", Tag = "Home", Icon = "Home"},
            new AppMenu{ Name = "ProduceAssemly2", Tag = "Produce", Icon = "Home"},
        };
    }
}
