using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RecordTracker.Services;
using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.ViewModel
{
    public class PoliceStationListViewModel: ViewModelBase
    {
        IPoliceStationRepository repo;
        ObservableCollection<PoliceStation> _PoliceStations;
        public RelayCommand AddPoliceStation { get; private set; }
        public RelayCommand DeletePoliceStation { get; private set; }
        public RelayCommand SavePoliceStation { get; private set; }
        private string _psname = "";
        private PoliceStation _selectedPoliceStation;

        //constructor
        public PoliceStationListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new PoliceStationRepository();
                AddPoliceStation = new RelayCommand(OnAdd);
                DeletePoliceStation = new RelayCommand(OnDelete, CanDelete);
                SavePoliceStation = new RelayCommand(OnSave, CanSave);
            }
        }

       

        #region All Properties
        public ObservableCollection<PoliceStation> PoliceStations
        {
            get { return _PoliceStations; }
            set { _PoliceStations = value;RaisePropertyChanged("PoliceStations"); }
            
        }

        public PoliceStation SelectedPoliceStation
        {
            get
            {
                return _selectedPoliceStation;
            }
            set
            {
                _selectedPoliceStation = value;
                DeletePoliceStation.RaiseCanExecuteChanged();
                SavePoliceStation.RaiseCanExecuteChanged();
            }
        }
        public string NewPsname
        {
            get { return _psname; }
            set
            {
                _psname = value;
                RaisePropertyChanged("NewPsname");
                //AddPoliceStation.RaiseCanExecuteChanged(); // this code will work with CanAdd function.
            }
        }
        #endregion

        #region All methods
        public void LoadData()
        {
            var data= repo.GetPoliceStationsAsync().Result.ToList();
            ObservableCollection<PoliceStation> PSData = new ObservableCollection<PoliceStation>();
            foreach (var item in data)
                if (item.Id != 1)
                {
                    PSData.Add(item);
                }
           
            PoliceStations = PSData;
        }

        private void OnAdd()
        {
            PoliceStation PS = new PoliceStation() { Name = NewPsname };
            NewPsname = "";
            repo.AddPoliceStationAsync(PS);
            PoliceStations.Add(PS);
        }

        private void OnDelete()
        {
            
            repo.DeletePoliceStationAsync(SelectedPoliceStation.Id);
            PoliceStations.Remove(SelectedPoliceStation);
        }

        private bool CanDelete()
        {
            return SelectedPoliceStation!=null;
        }

        private void OnSave()
        {
            repo.UpdatePoliceStationAsync(SelectedPoliceStation);
            SelectedPoliceStation = null;
        }

        private bool CanSave()
        {
            return SelectedPoliceStation != null; 
        }
        #endregion

    }
}
