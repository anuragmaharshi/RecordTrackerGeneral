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
    public class AlphaListViewModel : ViewModelBase
    {
        IAlphaRepository repo;
        ObservableCollection<Alpha> _Alphas;
        public RelayCommand AddAlpha { get; private set; }
        public RelayCommand DeleteAlpha { get; private set; }
        public RelayCommand SaveAlpha { get; private set; }
        private string _alphaName = "";
        private Alpha _selectedAlpha;

        //constructor
        public AlphaListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new AlphaRepository();
                AddAlpha = new RelayCommand(OnAdd);
                DeleteAlpha = new RelayCommand(OnDelete, CanDelete);
                SaveAlpha = new RelayCommand(OnSave, CanSave);
            }
        }

        #region All Properties
        public ObservableCollection<Alpha> Alphas
        {
            get { return _Alphas; }
            set { _Alphas = value; RaisePropertyChanged("Alphas"); }

        }

        public Alpha SelectedAlpha
        {
            get
            {
                return _selectedAlpha;
            }
            set
            {
                _selectedAlpha = value;
                DeleteAlpha.RaiseCanExecuteChanged();
                SaveAlpha.RaiseCanExecuteChanged();
            }
        }
        public string NewAlphaName
        {
            get { return _alphaName; }
            set
            {
                _alphaName = value;
                RaisePropertyChanged("NewAlphaName");
                //AddPoliceStation.RaiseCanExecuteChanged(); // this code will work with CanAdd function.
            }
        }
        #endregion

        #region All methods
        public void LoadData()
        {
            var data = repo.GetAlphasAsync().Result.ToList();
            ObservableCollection<Alpha> PSData = new ObservableCollection<Alpha>();
            foreach (var item in data)
            {
                if (item.Id != 1)
                {
                    PSData.Add(item);
                }
            }  
            Alphas = PSData;
        }

        private void OnAdd()
        {
            Alpha PO = new Alpha() { Name = NewAlphaName };
            NewAlphaName = "";
            repo.AddAlphaAsync(PO);
            Alphas.Add(PO);
        }

        private void OnDelete()
        {

            repo.DeleteAlphaAsync(SelectedAlpha.Id);
            Alphas.Remove(SelectedAlpha);
        }

        private bool CanDelete()
        {
            return SelectedAlpha != null;
        }

        private void OnSave()
        {
            repo.UpdateAlphaAsync(SelectedAlpha);
            SelectedAlpha = null;
        }

        private bool CanSave()
        {
            return SelectedAlpha != null;
        }
        #endregion
    }
}
