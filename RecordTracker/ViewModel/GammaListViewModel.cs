using GalaSoft.MvvmLight;
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
    public class GammaListViewModel : ViewModelBase
    {
        IGammaRepository repo;
        ObservableCollection<Gamma> _gammas;
        public RelayCommand AddGamma { get; private set; }
        public RelayCommand DeleteGamma { get; private set; }
        public RelayCommand SaveGamma{ get; private set; }
        private string _GammaName = "";
        private Gamma _selectedGamma;

        public GammaListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new GammaRepository();
                AddGamma = new RelayCommand(OnAdd);
                DeleteGamma = new RelayCommand(OnDelete, CanDelete);
                SaveGamma = new RelayCommand(OnSave, CanSave);
            }
        }

        #region All Properties
        public ObservableCollection<Gamma> Gammas
        {
            get { return _gammas; }
            set { _gammas = value; RaisePropertyChanged("Gammas"); }

        }

        public Gamma SelectedGamma
        {
            get
            {
                return _selectedGamma;
            }
            set
            {
                _selectedGamma = value;
                DeleteGamma.RaiseCanExecuteChanged();
                SaveGamma.RaiseCanExecuteChanged();
            }
        }
        public string NewGammaName
        {
            get { return _GammaName; }
            set
            {
                _GammaName = value;
                RaisePropertyChanged("NewGammaName");
               
            }
        }
        #endregion

        #region All methods
        public void LoadData()
        {
            var data = repo.GetGammasAsync().Result.ToList();
            ObservableCollection<Gamma> GammaData = new ObservableCollection<Gamma>();
            foreach (var item in data)
            {
                if (item.Id != 1)
                {
                    GammaData.Add(item);
                }
            }
               
            Gammas = GammaData;
        }

        private void OnAdd()
        {
            Gamma _gamma = new Gamma() { Name = NewGammaName };
            NewGammaName = "";
            repo.AddGammaAsync(_gamma);
            Gammas.Add(_gamma);
        }

        private void OnDelete()
        {

            repo.DeleteGammaAsync(SelectedGamma.Id);
            Gammas.Remove(SelectedGamma);
        }

        private bool CanDelete()
        {
            return SelectedGamma != null;
        }

        private void OnSave()
        {
            repo.UpdateGammaAsync(SelectedGamma);
            SelectedGamma = null;
        }

        private bool CanSave()
        {
            return SelectedGamma != null; 
        }
        #endregion
    }
}
