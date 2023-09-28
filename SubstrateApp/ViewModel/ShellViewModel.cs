namespace SubstrateApp.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using MahApps.Metro.IconPacks;
    using SubstrateApp.Mvvm;
    using SubstrateApp.Pages;

    public class ShellViewModel : BindableBase
    {
        private static readonly ObservableCollection<MenuItem> AppMenu = new ObservableCollection<MenuItem>();
        private static readonly ObservableCollection<MenuItem> AppOptionsMenu = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> Menu => AppMenu;

        public ObservableCollection<MenuItem> OptionsMenu => AppOptionsMenu;

        public ShellViewModel()
        {
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconEntypo() { Kind = PackIconEntypoKind.Popup },
                Label = "ProduceAssemly",
                NavigationType = typeof(ProduceAssemly),
                NavigationDestination = new Uri("Pages/ProduceAssemly.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconEntypo() { Kind = PackIconEntypoKind.OpenBook },
                Label = "ProduceAssemly2",
                NavigationType = typeof(ProduceAssemly2),
                NavigationDestination = new Uri("Pages/ProduceAssemly2.xaml", UriKind.RelativeOrAbsolute)
            });
        }
    }
}
