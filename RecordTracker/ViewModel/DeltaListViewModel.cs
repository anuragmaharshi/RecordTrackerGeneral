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
    public class DeltaListViewModel: ViewModelBase
    {

        IDeltaRepository repo;
        ObservableCollection<Delta> _deltas;
        public RelayCommand AddDelta { get; private set; }
        public RelayCommand DeleteDelta { get; private set; }
        public RelayCommand SaveDelta { get; private set; }
        private string _DeltaName = "";
        private Delta _selectedDelta;

        #region Constructor
        public DeltaListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new DeltaRepository();
                AddDelta = new RelayCommand(OnAdd);
                DeleteDelta = new RelayCommand(OnDelete, CanDelete);
                SaveDelta = new RelayCommand(OnSave, CanSave);
            }
        }
        #endregion

        #region Observable Collection
        public ObservableCollection<Delta> Deltas
        {
            get { return _deltas; }
            set { _deltas = value; RaisePropertyChanged("Deltas"); }

        }

        public Delta SelectedDelta
        {
            get
            {
                return _selectedDelta;
            }
            set
            {
                _selectedDelta = value;
                DeleteDelta.RaiseCanExecuteChanged();
                SaveDelta.RaiseCanExecuteChanged();
            }
        }
        public string NewDeltaName
        {
            get { return _DeltaName; }
            set
            {
                _DeltaName = value;
                RaisePropertyChanged("NewDeltaName");
            }
        }
        #endregion
        #region Events handler
        private bool CanSave()
        {
            return SelectedDelta != null;
        }

        private void OnSave()
        {
            repo.UpdateDeltaAsync(SelectedDelta);
            SelectedDelta = null;
        }

        private bool CanDelete()
        {
            return SelectedDelta != null;
        }

        private void OnDelete()
        {
           
            repo.DeleteDeltaAsync(SelectedDelta.Id);
            Deltas.Remove(SelectedDelta);
        }

        private void OnAdd()
        {
            Delta _delta = new Delta() { Name = NewDeltaName };
            NewDeltaName = "";
            repo.AddDeltaAsync(_delta);
            Deltas.Add(_delta);
        }

        public void LoadData()
        {
            var data = repo.GetDeltasAsync().Result.ToList();
            ObservableCollection<Delta> DeltaData = new ObservableCollection<Delta>();
            foreach (var item in data)
            {
                if (item.Id != 1)
                {
                    DeltaData.Add(item);
                }
            }
       
            Deltas = DeltaData;
        }
        #endregion

       
    }
}
