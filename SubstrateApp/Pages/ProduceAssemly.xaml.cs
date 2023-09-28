namespace SubstrateApp.Pages
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using SubstrateApp.ViewModel;
    using SubstrateUtils.ProduceAssembly;

    /// <summary>
    /// Interaction logic for ProduceAssemly.xaml
    /// </summary>
    public partial class ProduceAssemly : Page
    {
        private ProduceAssemblyViewModel produceAssemblyViewModel;
        public ProduceAssemly()
        {
            InitializeComponent();
            produceAssemblyViewModel = AppServiceProvider.GetService<ProduceAssemblyViewModel>();
            DataContext = produceAssemblyViewModel;
        }

        private void Btn_Produce(object sender, RoutedEventArgs e)
        {
            produceAssemblyViewModel.Produce();
        }
    }
}
