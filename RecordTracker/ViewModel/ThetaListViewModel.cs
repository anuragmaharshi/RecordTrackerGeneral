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
    public class ThetaListViewModel:ViewModelBase
    {

        IThetaRepository repo;
        ObservableCollection<Theta> _Thetas;

        public RelayCommand AddTheta { get; private set; }
        public RelayCommand DeleteTheta{ get; private set; }
        public RelayCommand SaveTheta { get; private set; }
        private string _ThetaName = "";
        private Theta _selectedTheta;



        public ThetaListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new ThetaRepository();
                AddTheta = new RelayCommand(OnAdd);
                DeleteTheta = new RelayCommand(OnDelete, CanDelete);
                SaveTheta = new RelayCommand(OnSave, CanSave);
            }
        }
        #region All Properties
        public ObservableCollection<Theta> Thetas
        {
            get { return _Thetas; }
            set { _Thetas = value; RaisePropertyChanged("Thetas"); }

        }

        public Theta SelectedTheta
        {
            get
            {
                return _selectedTheta;
            }
            set
            {
                _selectedTheta = value;
                DeleteTheta.RaiseCanExecuteChanged();
                SaveTheta.RaiseCanExecuteChanged();
            }
        }
        public string NewThetaName
        {
            get { return _ThetaName; }
            set
            {
                _ThetaName = value;
                RaisePropertyChanged("NewThetaName");
                
            }
        }
        #endregion

        public void LoadData()
        {
            var data = repo.GetThetasAsync().Result.ToList();
            ObservableCollection<Theta> ThetaData = new ObservableCollection<Theta>();
            foreach (var item in data)
            {
                if (item.Id != 1)
                {
                    ThetaData.Add(item);
                }
            }
           
            Thetas = ThetaData;
        }
        private bool CanSave()
        {
            return SelectedTheta != null;
        }

        private void OnSave()
        {
            repo.UpdateThetaAsync(SelectedTheta);
            SelectedTheta = null;
        }

        private bool CanDelete()
        {
            return SelectedTheta != null;
        }

        private void OnDelete()
        {
            repo.DeleteThetaAsync(SelectedTheta.Id);
            Thetas.Remove(SelectedTheta);
        }

        private void OnAdd()
        {
            Theta _theta = new Theta() { Name = NewThetaName };
            NewThetaName = "";
            repo.AddThetaAsync(_theta);
            Thetas.Add(_theta);
        }
    }
}
