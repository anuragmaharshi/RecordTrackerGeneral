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
    public class SourceListViewModel: ViewModelBase
    {

        ISourceRepository repo;
        ObservableCollection<Source> _sources;
        public RelayCommand AddSource { get; private set; }
        public RelayCommand DeleteSource { get; private set; }
        public RelayCommand SaveSource { get; private set; }
        private string _source = "";
        private Source _selectedSource;

        #region Constructor
        public SourceListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new SourceRepository();
                AddSource = new RelayCommand(OnAdd);
                DeleteSource = new RelayCommand(OnDelete, CanDelete);
                SaveSource = new RelayCommand(OnSave, CanSave);
            }
        }
        #endregion

        #region Observable Collection
        public ObservableCollection<Source> Sources
        {
            get { return _sources; }
            set { _sources = value; RaisePropertyChanged("Sources"); }

        }

        public Source SelectedSource
        {
            get
            {
                return _selectedSource;
            }
            set
            {
                _selectedSource = value;
                DeleteSource.RaiseCanExecuteChanged();
                SaveSource.RaiseCanExecuteChanged();
            }
        }
        public string NewSource
        {
            get { return _source; }
            set
            {
                _source = value;
                RaisePropertyChanged("NewSource");
            }
        }
        #endregion
        #region Events handler
        private bool CanSave()
        {
            return SelectedSource != null;
        }

        private void OnSave()
        {
            repo.UpdateSourceAsync(SelectedSource);
            SelectedSource = null;
        }

        private bool CanDelete()
        {
            return SelectedSource != null;
        }

        private void OnDelete()
        {
           
            repo.DeleteSourceAsync(SelectedSource.Id);
            Sources.Remove(SelectedSource);
        }

        private void OnAdd()
        {
            Source source = new Source() { Name = NewSource };
            NewSource = "";
            repo.AddSourceAsync(source);
            Sources.Add(source);
        }

        public void LoadData()
        {
            var data = repo.GetSourcesAsync().Result.ToList();
            ObservableCollection<Source> SourceData = new ObservableCollection<Source>();
            foreach (var item in data)
            {
                if (item.Id != 1)
                {
                    SourceData.Add(item);
                }
            }
       
            Sources = SourceData;
        }
        #endregion

       
    }
}
