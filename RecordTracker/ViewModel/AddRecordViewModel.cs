using GalaSoft.MvvmLight;
using NLog;
using RecordTracker.Services;
using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.ViewModel
{
    public class AddRecordViewModel : ViewModelBase
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        IAlphaRepository Alpharepo;
        ObservableCollection<Alpha> _Alphas;

        IPoliceStationRepository PSrepo;
        ObservableCollection<PoliceStation> _PoliceStations;

        ITopicAndAreaRepository TArepo;
        ObservableCollection<TopicsAndArea> _topicsAndAreas;

        ISubjectRepository SubRepo;
        ObservableCollection<Subject> _subjects;

        ISourceRepository SourceRepo;
        ObservableCollection<Source> _source;

        IRecordRepository AddRepo;
        public RelayCommand AddNewRecord{ get; private set; }
        public RelayCommand CancelRecord { get; private set; }

        #region Constructor
        public AddRecordViewModel()
        {
            try
            {
                if (!ViewModelBase.IsInDesignModeStatic)
                {
                    _logger.Info("Inside Add Record ViewModel construtor");
                    Alpharepo = new AlphaRepository();
                    PSrepo = new PoliceStationRepository();
                    TArepo = new TopicAndAreaRepository();
                    SourceRepo = new SourceRepository();
                    SubRepo = new SubjectRepository();
                    AddRepo = new RecordRepository();
                    AddNewRecord = new RelayCommand(OnAdd, CanAdd);
                    CancelRecord = new RelayCommand(OnCancel, CanCancel);
                   
                }
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in AddRecordViewModel" + e.StackTrace);
                _logger.Error("AddRecordViewModel error message is " + e.Message + " inner error is " + e.InnerException.Message);
            }


        }
        #endregion

        #region Observable collections
        public ObservableCollection<Alpha> Alphas
        {
            get { return _Alphas; }
            set { _Alphas = value; RaisePropertyChanged("Alphas"); }

        }
        public ObservableCollection<PoliceStation> PoliceStations
        {
            get { return _PoliceStations; }
            set { _PoliceStations = value; RaisePropertyChanged("PoliceStations"); }

        }

        public ObservableCollection<TopicsAndArea> TopicsAndAreas
        {
            get { return _topicsAndAreas; }
            set { _topicsAndAreas = value; RaisePropertyChanged("TopicsAndAreas"); }

        }

        public ObservableCollection<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; RaisePropertyChanged("Subjects"); }

        }

        public ObservableCollection<Source> Sources
        {
            get { return _source; }
            set { _source = value; RaisePropertyChanged("Sources"); }

        }
        #endregion

        #region Properties
      

        private string _letterNumber ;
        public string LetterNumber
        {
            get { return _letterNumber; }
            set {
                
                if (value != "")
                    _letterNumber = value;
                else
                    _letterNumber = null;
                RaisePropertyChanged("LetterNumber");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _officeReceiptDate;
        public string OfficeReceiptDate
        {
            get { return _officeReceiptDate; }
            set
            {
                
                _officeReceiptDate = value;
                RaisePropertyChanged("OfficeReceiptDate");
                CancelRecord.RaiseCanExecuteChanged();
                AddNewRecord.RaiseCanExecuteChanged();
            }
        }

        private string _officeDispatchDate;
        public string OfficeDispatchDate
        {
            get { return _officeDispatchDate; }
            set
            {
               
                _officeDispatchDate = value;
                RaisePropertyChanged("OfficeDispatchDate");
                CancelRecord.RaiseCanExecuteChanged();
                AddNewRecord.RaiseCanExecuteChanged();
            }
        }

        private string _officeDispatchNumber;
        public string OfficeDispatchNumber
        {
            get { return _officeDispatchNumber; }
            set {
                if (value != "")
                    _officeDispatchNumber = value;
                else
                    _officeDispatchNumber = null;
                
                RaisePropertyChanged("OfficeDispatchNumber");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _psDispatchDate;
        public string PsDispatchDate
        {
            get { return _psDispatchDate; }
            set
            {
               
                _psDispatchDate = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("PsDispatchDate");

            }
        }

        private string _psDispatchNumber;
        public string PsDispatchNumber
        {
            get { return _psDispatchNumber; }
            set
            {
                if (value != "")
                    _psDispatchNumber = value;
                else
                    _psDispatchNumber = null;

                RaisePropertyChanged("PsDispatchNumber");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _sanhaDetail;
        public string SanhaDetail
        {
            get { return _sanhaDetail; }
            set
            {
                _sanhaDetail = value;
                RaisePropertyChanged("SanhaDetail");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _verificationDetail;
        public string VerificationDetail
        {
            get { return _verificationDetail; }
            set
            {
                _verificationDetail = value;
                RaisePropertyChanged("VerificationDetail");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _caseNumber;
        public string CaseNumber
        {
            get { return _caseNumber; }
            set
            {
                if (value != "")
                    _caseNumber = value;
                else
                    _caseNumber = null;
                RaisePropertyChanged("CaseNumber");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }


        private string _organizationName;
        public string OrganizationName
        {
            get { return _organizationName; }
            set
            {
                _organizationName = value;
                RaisePropertyChanged("OrganizationName");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("Remarks");
            }
        }


        private PoliceStation _selectedPS;
        public PoliceStation SelectedPS
        {
          get { return _selectedPS; }
          set {
                _selectedPS = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedPS");
              }
        }

        private TopicsAndArea _selectedTA;
        public TopicsAndArea SelectedTA
        {
            get { return _selectedTA; }
            set {
                _selectedTA = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedTA");
            }
        }

        private Alpha _selectedAlpha;
        public Alpha SelectedAlpha
        {
            get { return _selectedAlpha; }
            set {
                _selectedAlpha = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedAlpha");
            }
        }


        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedSubject");
            }
        }


        private Source _selectedSource;
        public Source SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                _selectedSource = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedSource");
            }
        }

        private string _saveText;
        public string SaveText
        {
            get { return _saveText; }
            set { _saveText = value; RaisePropertyChanged("SaveText"); }
        }
        #endregion

        #region Methods
        public void LoadData()
        {
            try
            {
                var AlphaList = Alpharepo.GetAlphasAsync().Result.ToList();
                ObservableCollection<Alpha> POData = new ObservableCollection<Alpha>();
                foreach (var item in AlphaList)
                    POData.Add(item);
                Alphas = POData;

                var PSList = PSrepo.GetPoliceStationsAsync().Result.ToList();
                ObservableCollection<PoliceStation> PSData = new ObservableCollection<PoliceStation>();
                foreach (var item in PSList)
                    PSData.Add(item);
                PoliceStations = PSData;

                var TAList = TArepo.GetTopicAndAreasAsync().Result.ToList();
                ObservableCollection<TopicsAndArea> TAData = new ObservableCollection<TopicsAndArea>();
                foreach (var item in TAList)
                    TAData.Add(item);
                TopicsAndAreas = TAData;

                var SourceList = SourceRepo.GetSourcesAsync().Result.ToList();
                ObservableCollection<Source> SourceData = new ObservableCollection<Source>();
                foreach (var item in SourceList)
                    SourceData.Add(item);
                Sources = SourceData;

                var SubjectList = SubRepo.GetSubectsAsync().Result.ToList();
                ObservableCollection<Subject> SubjectData = new ObservableCollection<Subject>();
                foreach (var item in SubjectList)
                    SubjectData.Add(item);
                Subjects = SubjectData;

                SelectedPS = PoliceStations.First(x => x.Id.Equals(1));
                SelectedTA = TopicsAndAreas.First(x => x.Id.Equals(1));
                SelectedAlpha = Alphas.First(x => x.Id.Equals(1));
                SelectedSource = Sources.First(x => x.Id.Equals(1));
                SelectedSubject = Subjects.First(x => x.Id.Equals(1));

            }
            catch (Exception e)
            {
                _logger.Error("Some error have AddRecordViewModel Load data , stacktarce =" + e.StackTrace);
                _logger.Error("AddRecordViewModel error message is " + e.Message + " inner error is " + e.InnerException.Message);
            }
        }

        private bool CanAdd()
        {
            return LetterNumber != null;

        }

        private void OnAdd()
        {
            LetterRecord record = new LetterRecord
            {
                LetterNumber = LetterNumber
            };

            if (OfficeDispatchNumber != null)
                record.OfficeDispatchNumber = OfficeDispatchNumber;

            if (SelectedSource != null)
                record.SourceID = SelectedSource.Id;
           
            if(OfficeDispatchDate!=null)
                record.OfficeDispatchDate = FormatDate(OfficeDispatchDate);

            if(OfficeReceiptDate!=null)
                record.OfficeReceiptDate = FormatDate(OfficeReceiptDate);

            if(OrganizationName!=null)
                record.OrganizationName = OrganizationName;

            if(SanhaDetail!=null)
                record.SanhaDetail = SanhaDetail;

            if(VerificationDetail != null)
                record.VerificationDetail = VerificationDetail;

            if (SelectedSubject != null)
                record.SubjectID = SelectedSubject.Id;
            
            if(PsDispatchNumber!=null)
                record.PsDispatchNumber = PsDispatchNumber;

            if(PsDispatchDate!=null)
                record.PsDispatchDate = FormatDate(PsDispatchDate);

            if (SelectedTA != null)
                record.TopicAreaID = SelectedTA.Id;
           
            if (SelectedAlpha != null)
                record.AlphaID = SelectedAlpha.Id;
           
            if (SelectedPS != null)
                record.PoliceStationID = SelectedPS.Id;
            
            record.StatusID = 1;
            record.Remarks = Remarks;

            if(CaseNumber!=null)
                record.CaseNumber = CaseNumber;
            try
            {
                _logger.Info("Adding a new record" + record.ToString());
                AddRepo.AddLetterRecordAsync(record).Wait();
                SaveText = "Record added successfully.";
            }
            catch(Exception e)
            {
                _logger.Error("Some error have occured in AddRecordViewModel" + e.StackTrace);
                _logger.Error("Error messgage" + e.Message+" \n inner exception"+e.InnerException.Message);
                SaveText = "Unable to add record.";
            }
            ResetUI();
        }

        private bool CanCancel()
        {
            return SelectedPS != null || SelectedTA != null || SelectedAlpha != null 
                || LetterNumber!=null || OfficeDispatchNumber != null|| OfficeDispatchDate != null 
                || OfficeReceiptDate != null || SelectedSource!=null || SelectedSubject!=null || PsDispatchDate!=null
                || PsDispatchNumber!=null || SanhaDetail!=null|| VerificationDetail!=null || CaseNumber!=null
                || OrganizationName!=null|| Remarks!=null;
        }

        private void OnCancel()
        {
            ResetUI();
            SaveText = null;
        }

        private void ResetUI()
        {
            SelectedPS = PoliceStations.First(x => x.Id.Equals(1));
            SelectedTA = TopicsAndAreas.First(x => x.Id.Equals(1));
            SelectedAlpha = Alphas.First(x => x.Id.Equals(1));
            SelectedSource = Sources.First(x => x.Id.Equals(1));
            SelectedSubject = Subjects.First(x => x.Id.Equals(1));
            LetterNumber = null;
            OfficeDispatchNumber = null;
            OfficeDispatchDate = null;
            OfficeReceiptDate = null;
           
            PsDispatchDate = null;
            PsDispatchNumber = null;
            SanhaDetail = null;
            VerificationDetail = null;
            CaseNumber = null;
            OrganizationName = null;
            Remarks = null;
            
        }

        private string FormatDate(string dateTime)
        {
            try
            {
                _logger.Info(dateTime);
                var dty = DateTime.Parse(dateTime, CultureInfo.InvariantCulture);
                return dty.ToString("yyyy-MM-dd");
            }
            catch(Exception e)
            {
                _logger.Error("Some error occured while formatting " + e.StackTrace);
                _logger.Error("error message is  " + e.Message);
                return null;
            }
           
        }
        #endregion
    }
}
