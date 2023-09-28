namespace SubstrateApp.ViewModel
{
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using SubstrateUtils.ProduceAssembly;

    public class ProduceAssemblyViewModel : ObservableRecipient
    {
        public ProduceAssemblyViewModel()
        {
        }

        public string ProjectPath { get; set; }

        private string result;

        public string Result
        {
            get { return result; }
            set { SetProperty(ref result, value); }
        }

        public async void Produce()
        {
            string result = await Task.Run(() =>
               {
                   string rootPath = AppConfiguration.SubstratePath;
                   ProjectReference projectReference = new ProjectReference(rootPath, ProjectPath);
                   return projectReference.Produce();
               }).ConfigureAwait(false);

            Result = result;
        }
    }
}
