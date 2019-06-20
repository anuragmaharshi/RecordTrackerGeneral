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
    public class TopicAndAreaListViewModel : ViewModelBase
    {
        ITopicAndAreaRepository repo;
        ObservableCollection<TopicsAndArea> _topicsAndAreas;
        public RelayCommand AddTopicOrAreaStation { get; private set; }
        public RelayCommand DeleteTopicOrAreaStation { get; private set; }
        public RelayCommand SaveTopicOrAreaStation { get; private set; }
        private string _TopicOrAreaName = "";
        private TopicsAndArea _selectedTopicOrArea;

        public TopicAndAreaListViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                //Code that throws the exception
                repo = new TopicAndAreaRepository();
                AddTopicOrAreaStation = new RelayCommand(OnAdd);
                DeleteTopicOrAreaStation = new RelayCommand(OnDelete, CanDelete);
                SaveTopicOrAreaStation = new RelayCommand(OnSave, CanSave);
            }
        }

        #region All Properties
        public ObservableCollection<TopicsAndArea> TopicsAndAreas
        {
            get { return _topicsAndAreas; }
            set { _topicsAndAreas = value; RaisePropertyChanged("TopicsAndAreas"); }

        }

        public TopicsAndArea SelectedTopicOrArea
        {
            get
            {
                return _selectedTopicOrArea;
            }
            set
            {
                _selectedTopicOrArea = value;
                DeleteTopicOrAreaStation.RaiseCanExecuteChanged();
                SaveTopicOrAreaStation.RaiseCanExecuteChanged();
            }
        }
        public string NewTOrAName
        {
            get { return _TopicOrAreaName; }
            set
            {
                _TopicOrAreaName = value;
                RaisePropertyChanged("NewTOrAName");
               
            }
        }
        #endregion

        #region All methods
        public void LoadData()
        {
            var data = repo.GetTopicAndAreasAsync().Result.ToList();
            ObservableCollection<TopicsAndArea> TaAData = new ObservableCollection<TopicsAndArea>();
            foreach (var item in data)
            {
                if (item.Id != 1)
                {
                    TaAData.Add(item);
                }
            }
               
            TopicsAndAreas = TaAData;
        }

        private void OnAdd()
        {
            TopicsAndArea ToA = new TopicsAndArea() { Name = NewTOrAName };
            NewTOrAName = "";
            repo.AddTopicOrAreaAsync(ToA);
            TopicsAndAreas.Add(ToA);
        }

        private void OnDelete()
        {

            repo.DeleteTopicOrAreaAsync(SelectedTopicOrArea.Id);
            TopicsAndAreas.Remove(SelectedTopicOrArea);
        }

        private bool CanDelete()
        {
            return SelectedTopicOrArea != null;
        }

        private void OnSave()
        {
            repo.UpdateTopicAndAreaAsync(SelectedTopicOrArea);
            SelectedTopicOrArea = null;
        }

        private bool CanSave()
        {
            return SelectedTopicOrArea != null; 
        }
        #endregion
    }
}
