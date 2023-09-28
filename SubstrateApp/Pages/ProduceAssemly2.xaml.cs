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
using SubstrateApp.ViewModel;

namespace SubstrateApp.Pages
{
    /// <summary>
    /// Interaction logic for ProduceAssemly2.xaml
    /// </summary>
    public partial class ProduceAssemly2 : Page
    {
        private ProduceAssemblyViewModel produceAssemblyViewModel;
        public ProduceAssemly2()
        {
            InitializeComponent();
            produceAssemblyViewModel = AppServiceProvider.GetService<ProduceAssemblyViewModel>();
            DataContext = produceAssemblyViewModel;
        }

        private void Btn_Produce(object sender, RoutedEventArgs e)
        {

            Trace.WriteLine(produceAssemblyViewModel.ProjectPath);
        }
    }
}
