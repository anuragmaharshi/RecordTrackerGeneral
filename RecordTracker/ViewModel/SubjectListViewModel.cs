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
    public class SubjectListViewModel:ViewModelBase
    {

        ISubjectRepository repo;
        ObservableCollection<Subject> _subjects;

        public RelayCommand AddSubject { get; private set; }
        public RelayCommand DeleteSubject{ get; private set; }
        public RelayCommand SaveSubject { get; private set; }
        private string _subject = "";
        private Subject _selectedSubject;



        public SubjectListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new SubjectRepository();
                AddSubject = new RelayCommand(OnAdd);
                DeleteSubject = new RelayCommand(OnDelete, CanDelete);
                SaveSubject = new RelayCommand(OnSave, CanSave);
            }
        }
        #region All Properties
        public ObservableCollection<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; RaisePropertyChanged("Subjects"); }

        }

        public Subject SelectedSubject
        {
            get
            {
                return _selectedSubject;
            }
            set
            {
                _selectedSubject = value;
                DeleteSubject.RaiseCanExecuteChanged();
                SaveSubject.RaiseCanExecuteChanged();
            }
        }
        public string NewSubject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                RaisePropertyChanged("NewSubject");
                //AddPoliceStation.RaiseCanExecuteChanged(); // this code will work with CanAdd function.
            }
        }
        #endregion

        public void LoadData()
        {
            var data = repo.GetSubectsAsync().Result.ToList();
            ObservableCollection<Subject> SubjectData = new ObservableCollection<Subject>();
            foreach (var item in data)
            {
                if (item.Id != 1)
                {
                    SubjectData.Add(item);
                }
            }
           
            Subjects = SubjectData;
        }
        private bool CanSave()
        {
            return SelectedSubject != null;
        }

        private void OnSave()
        {
            repo.UpdateSubjectAsync(SelectedSubject);
            SelectedSubject = null;
        }

        private bool CanDelete()
        {
            return SelectedSubject != null;
        }

        private void OnDelete()
        {
            repo.DeleteSubectAsync(SelectedSubject.Id);
            Subjects.Remove(SelectedSubject);
        }

        private void OnAdd()
        {
            Subject subject = new Subject() { Name = NewSubject };
            NewSubject = "";
            repo.AddSubectAsync(subject);
            Subjects.Add(subject);
        }
    }
}
