using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MINECRAFTWIKI.MODELS;
using MINECRAFTWIKI.SERVICES;

namespace MINECRAFTWIKI.VIEWMODEL
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private MinecraftEntity _selectedEntity;
        private ObservableCollection<MinecraftEntity> _entities;

        public ObservableCollection<MinecraftEntity> Entities
        {
            get => _entities;
            set { _entities = value; OnPropertyChanged(); }
        }

        public MinecraftEntity SelectedEntity
        {
            get => _selectedEntity;
            set { _selectedEntity = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _apiService = new ApiService();
            Entities = new ObservableCollection<MinecraftEntity>();
            _ = LoadDataAsync(); // Carga los datos automáticamente al abrir
        }

        private async Task LoadDataAsync()
        {
            var data = await _apiService.GetEntitiesAsync();
            Entities = new ObservableCollection<MinecraftEntity>(data);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
