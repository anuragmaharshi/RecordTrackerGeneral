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
    public class BetaListViewModel: ViewModelBase
    {
        IBetaRepository repo;
        ObservableCollection<Beta> _Betas;
        public RelayCommand AddBeta { get; private set; }
        public RelayCommand DeleteBeta { get; private set; }
        public RelayCommand SaveBeta { get; private set; }
        private string _BetaName = "";
        private Beta _selectedBeta;

        //constructor
        public BetaListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new BetaRepository();
                AddBeta = new RelayCommand(OnAdd);
                DeleteBeta = new RelayCommand(OnDelete, CanDelete);
                SaveBeta = new RelayCommand(OnSave, CanSave);
            }
        }

       

        #region All Properties
        public ObservableCollection<Beta> Betas
        {
            get { return _Betas; }
            set { _Betas = value;RaisePropertyChanged("Betas"); }
            
        }

        public Beta SelectedBeta
        {
            get
            {
                return _selectedBeta;
            }
            set
            {
                _selectedBeta = value;
                DeleteBeta.RaiseCanExecuteChanged();
                SaveBeta.RaiseCanExecuteChanged();
            }
        }
        public string NewBetaName
        {
            get { return _BetaName; }
            set
            {
                _BetaName = value;
                RaisePropertyChanged("NewBetaName");
                //AddBeta.RaiseCanExecuteChanged(); // this code will work with CanAdd function.
            }
        }
        #endregion

        #region All methods
        public void LoadData()
        {
            var data= repo.GetBetasAsync().Result.ToList();
            ObservableCollection<Beta> BetaData = new ObservableCollection<Beta>();
            foreach (var item in data)
                if (item.Id != 1)
                {
                    BetaData.Add(item);
                }
           
            Betas = BetaData;
        }

        private void OnAdd()
        {
            Beta _beta = new Beta() { Name = NewBetaName };
            NewBetaName = "";
            repo.AddBetaAsync(_beta);
            Betas.Add(_beta);
        }

        private void OnDelete()
        {
            
            repo.DeleteBetaAsync(SelectedBeta.Id);
            Betas.Remove(SelectedBeta);
        }

        private bool CanDelete()
        {
            return SelectedBeta!=null;
        }

        private void OnSave()
        {
            repo.UpdateBetaAsync(SelectedBeta);
            SelectedBeta = null;
        }

        private bool CanSave()
        {
            return SelectedBeta != null; 
        }
        #endregion

    }
}
