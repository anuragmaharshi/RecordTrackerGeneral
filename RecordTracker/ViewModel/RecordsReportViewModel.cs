using GalaSoft.MvvmLight;
using Microsoft.Win32;
using NLog;
using RecordTracker.Services;
using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecordTracker.ViewModel
{
    public class RecordsReportViewModel : ViewModelBase
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #region variable declaration
        IAlphaRepository Alpharepo;
        ObservableCollection<Alpha> _Alphas,_alphaListFilter;
       
        IPoliceStationRepository PSrepo;
        ObservableCollection<PoliceStation> _PoliceStations, _psListFilter;

        ITopicAndAreaRepository TArepo;
        ObservableCollection<TopicsAndArea> _topicsAndAreas, _taListFilter;

        IRecordRepository RecRepo;
        ObservableCollection<LetterRecord> _reportRecords;

        ISubjectRepository SubRepo;
        ObservableCollection<Subject> _subjects,_subjectListFilter;

        ISourceRepository SourceRepo;
        ObservableCollection<Source> _source,_sourceListFilter;

        StatusRepository StatusRepo;
        ObservableCollection<Status> _statusS,_statusFilter;

        LetterRecord _selectedRecord;
        Alpha _selectedAlpha;
        PoliceStation _selectedPoliceStation;
        TopicsAndArea _selectedTopic;
       

        public RelayCommand SaveRecord { get; private set; }

        public RelayCommand SearchRecord { get; private set; }

        public RelayCommand ExportToPDF { get; private set; }

        public RelayCommand DeleteRecord { get; private set; }

        List<LetterRecord> letterRecords;
        #endregion

        #region Constructor
        public RecordsReportViewModel()
        {
            try
            {
                _logger.Info("Inside Records viewer view model construtor");
                if (!ViewModelBase.IsInDesignModeStatic)
                {

                    Alpharepo = new AlphaRepository();
                    PSrepo = new PoliceStationRepository();
                    TArepo = new TopicAndAreaRepository();
                    RecRepo = new RecordRepository();
                    StatusRepo = new StatusRepository();
                    SourceRepo = new SourceRepository();
                    SubRepo = new SubjectRepository();
                    SaveRecord = new RelayCommand(OnSave, canSave);
                    SearchRecord = new RelayCommand(onSearch, canSearch);
                    ExportToPDF = new RelayCommand(onExportToPdf, canExport);
                    DeleteRecord = new RelayCommand(onDelete, canDelete);
                    PdfFilterLetterNumber = true;
                    PdfFilterStatus = true;
                }
            }
            catch(Exception e)
            {
                _logger.Error("Some error have occured in RecordsReportViewModel, stacktrace=" + e.StackTrace);
                _logger.Error("RecordsReportViewModel error message is " + e.Message + " inner error is " + e.InnerException.Message);
            }
           
        }

        private bool canDelete()
        {
            return SelectedRecord != null;
        }

        private void onDelete()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                RecRepo.DeleteRecordsAsync(SelectedRecord.Id);
                ReportRecords.Remove(SelectedRecord);
                SelectedRecord = null;
            }
        }


        #endregion

        #region Observable collections
        public ObservableCollection<Alpha> Alphas
        {
            get { return _Alphas; }
            set { _Alphas = value; RaisePropertyChanged("Alphas"); }

        }

        public ObservableCollection<Alpha> AlphaFilter
        {
            get { return _alphaListFilter; }
            set { _alphaListFilter = value; RaisePropertyChanged("AlphaFilter"); }

        }
        public ObservableCollection<PoliceStation> PoliceStations
        {
            get { return _PoliceStations; }
            set { _PoliceStations = value; RaisePropertyChanged("PoliceStations"); }

        }
        public ObservableCollection<PoliceStation> PSFilter
        {
            get { return _psListFilter; }
            set { _psListFilter = value; RaisePropertyChanged("PSFilter"); }

        }
        public ObservableCollection<TopicsAndArea> TopicsAndAreas
        {
            get { return _topicsAndAreas; }
            set { _topicsAndAreas = value; RaisePropertyChanged("TopicsAndAreas"); }

        }
        public ObservableCollection<TopicsAndArea> TAFilter
        {
            get { return _taListFilter; }
            set { _taListFilter = value; RaisePropertyChanged("TAFilter"); }

        }

        public ObservableCollection<Status> Statuses
        {
            get { return _statusS; }
            set { _statusS = value; RaisePropertyChanged("Statuses"); }
        }

        public ObservableCollection<Status> StatusFilter
        {
            get { return _statusFilter; }
            set { _statusFilter = value; RaisePropertyChanged("StatusFilter"); }
        }

        public ObservableCollection<LetterRecord> ReportRecords
        {
            get { return _reportRecords; }
            set { _reportRecords = value; RaisePropertyChanged("ReportRecords");
                ExportToPDF.RaiseCanExecuteChanged();
            }

        }
        public ObservableCollection<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; RaisePropertyChanged("Subjects"); }

        }
        public ObservableCollection<Subject> SubFilter
        {
            get { return _subjectListFilter; }
            set { _subjectListFilter = value; RaisePropertyChanged("SubFilter"); }

        }
        public ObservableCollection<Source> Sources
        {
            get { return _source; }
            set { _source = value; RaisePropertyChanged("Sources"); }

        }
        public ObservableCollection<Source> SrcFilter
        {
            get { return _sourceListFilter; }
            set { _sourceListFilter = value; RaisePropertyChanged("SrcFilter"); }

        }
        #endregion

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
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
                RaisePropertyChanged("SelectedSource");
            }
        }

        public LetterRecord SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                RaisePropertyChanged("SelectedRecord");
                DeleteRecord.RaiseCanExecuteChanged();
            }
        }

        public Alpha SelectedAlpha
        {
            get { return _selectedAlpha; }
            set
            {
                _selectedAlpha = value;
                RaisePropertyChanged("SelectedAlpha");
            }
        }


        public PoliceStation SelectedPoliceStation
        {
            get { return _selectedPoliceStation; }
            set
            {
                _selectedPoliceStation = value;
                RaisePropertyChanged("SelectedPoliceStation");
            }
        }

        public TopicsAndArea SelectedTopic
        {
            get { return _selectedTopic; }
            set
            {
                _selectedTopic = value;
                RaisePropertyChanged("SelectedTopic");
            }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                RaisePropertyChanged("SelectedStatus");
            }
        }

        private bool _pdfLetterNumberFilter;
        public bool PdfFilterLetterNumber
        {
            get { return _pdfLetterNumberFilter; }
            set { _pdfLetterNumberFilter = value; RaisePropertyChanged("PdfFilterLetterNumber"); }
        }


        private bool _pdfStatusFilter;
        public bool PdfFilterStatus
        {
            get { return _pdfStatusFilter; }
            set { _pdfStatusFilter = value; RaisePropertyChanged("PdfFilterStatus"); }
        }

        private bool _officeReceiptDateFilter;
        public bool PdfFilterOfficeReceiptDate
        {
            get { return _officeReceiptDateFilter; }
            set { _officeReceiptDateFilter = value; RaisePropertyChanged("PdfFilterOfficeReceiptDate"); }
        }

        private bool _topicAreaFilter;
        public bool PdfFilterTopicArea
        {
            get { return _topicAreaFilter; }
            set { _topicAreaFilter = value; RaisePropertyChanged("PdfFilterTopicArea"); }
        }

        private bool _policeStationFilter;
        public bool PdfFilterPoliceStation
        {
            get { return _policeStationFilter; }
            set { _policeStationFilter = value; RaisePropertyChanged("PdfFilterPoliceStation"); }
        }

        private bool _AlphaFilter;
        public bool PdfFilterAlpha
        {
            get { return _AlphaFilter; }
            set { _AlphaFilter = value; RaisePropertyChanged("PdfFilterAlpha"); }
        }

        private bool _officeDispatchNumberFilter;
        public bool PdfFilterOfficeDispatchNumber
        {
            get { return _officeDispatchNumberFilter; }
            set { _officeDispatchNumberFilter = value; RaisePropertyChanged("PdfFilterOfficeDispatchNumber"); }
        }

        private bool _sourceFilter;
        public bool PdfFilterSource
        {
            get { return _sourceFilter; }
            set { _sourceFilter = value; RaisePropertyChanged("PdfFilterSource"); }
        }

        private bool _officeDispatchDateFilter;
        public bool PdfFilterOfficeDispatchDate
        {
            get { return _officeDispatchDateFilter; }
            set { _officeDispatchDateFilter = value; RaisePropertyChanged("PdfFilterOfficeDispatchDate"); }
        }

        private bool _organizationNameFilter;
        public bool PdfFilterOrganizationName
        {
            get { return _organizationNameFilter; }
            set { _organizationNameFilter = value; RaisePropertyChanged("PdfFilterOrganizationName"); }
        }

        private bool _sanhaDetailFilter;
        public bool PdfFilterSanhaDetail
        {
            get { return _sanhaDetailFilter; }
            set { _sanhaDetailFilter = value; RaisePropertyChanged("PdfFilterSanhaDetail"); }
        }

        private bool _verificationDetaillFilter;
        public bool PdfFilterVerificationDetail
        {
            get { return _verificationDetaillFilter; }
            set { _verificationDetaillFilter = value; RaisePropertyChanged("PdfFilterVerificationDetail"); }
        }

        private bool _subjectFilter;
        public bool PdfFilterSubject
        {
            get { return _subjectFilter; }
            set { _subjectFilter = value; RaisePropertyChanged("PdfFilterSubject"); }
        }

        private bool _psDispatchNumberFilter;
        public bool PdfFilterPSDispatchNumber
        {
            get { return _psDispatchNumberFilter; }
            set { _psDispatchNumberFilter = value; RaisePropertyChanged("PdfFilterPSDispatchNumber"); }
        }

        private bool _psDispatchDateFilter;
        public bool PdfFilterPSDispatchDate
        {
            get { return _psDispatchDateFilter; }
            set { _psDispatchDateFilter = value; RaisePropertyChanged("PdfFilterPSDispatchDate"); }
        }

        private bool _caseNumberFilter;
        public bool PdfFilterCaseNumber
        {
            get { return _caseNumberFilter; }
            set { _caseNumberFilter = value; RaisePropertyChanged("PdfFilterCaseNumber"); }
        }

        private Visibility _isWorking;
        public Visibility IsWorking
        {
            get { return _isWorking; }
            set { _isWorking = value;RaisePropertyChanged("IsWorking");
               
            }
        }

        public void LoadData()
        {
            try
            {
                _logger.Info("Inside Records viewer view model Load data");
                IsWorking = Visibility.Hidden;
                 var POList = Alpharepo.GetAlphasAsync().Result.ToList();
                ObservableCollection<Alpha> POData = new ObservableCollection<Alpha>();
                AlphaFilter = new ObservableCollection<Alpha>();
                foreach (var item in POList)
                {
                    POData.Add(item);
                    AlphaFilter.Add(item);
                }
                   
                Alphas = POData;
                AlphaFilter.Add(new Alpha() { Id = 1000, Name = "All" });
                SelectedAlpha = AlphaFilter.First(x => x.Name.Equals("All"));

                var PSList = PSrepo.GetPoliceStationsAsync().Result.ToList();
                ObservableCollection<PoliceStation> PSData = new ObservableCollection<PoliceStation>();
                PSFilter = new ObservableCollection<PoliceStation>();
                foreach (var item in PSList)
                {
                    PSData.Add(item);
                    PSFilter.Add(item);
                }
                   
                PoliceStations = PSData;
                PSFilter.Add(new PoliceStation() { Id = 1000, Name = "All" });
                SelectedPoliceStation = PSFilter.First(x => x.Name.Equals("All"));

                var TAList = TArepo.GetTopicAndAreasAsync().Result.ToList();
                ObservableCollection<TopicsAndArea> TAData = new ObservableCollection<TopicsAndArea>();
                TAFilter = new ObservableCollection<TopicsAndArea>();
                foreach (var item in TAList)
                {
                    TAData.Add(item);
                    TAFilter.Add(item);
                }
                    
                TopicsAndAreas = TAData;
              
                TAFilter.Add(new TopicsAndArea() { Id = 1000, Name = "All" });
                SelectedTopic = TAFilter.First(x => x.Name.Equals("All"));
                

                var StatusList = StatusRepo.GetStatus();
                ObservableCollection<Status> StatusData = new ObservableCollection<Status>();
                StatusFilter = new ObservableCollection<Status>();
                foreach (var item in StatusList)
                {
                    StatusData.Add(item);
                    StatusFilter.Add(item);
                }
                
                Statuses = StatusData;
                StatusFilter.Add(new Status() { Id = 1000, Name = "All" });
                SelectedStatus = StatusFilter.First(x => x.Name.Equals("Open"));

                var SourceList = SourceRepo.GetSourcesAsync().Result.ToList();
                ObservableCollection<Source> SourceData = new ObservableCollection<Source>();
                SrcFilter = new ObservableCollection<Source>();
                foreach (var item in SourceList)
                {
                    SourceData.Add(item);
                    SrcFilter.Add(item);
                }
                    
                Sources = SourceData;
                SrcFilter.Add(new Source() { Id = 1000, Name = "All" });
                SelectedSource= SrcFilter.First(x => x.Name.Equals("All"));

                var SubjectList = SubRepo.GetSubectsAsync().Result.ToList();
                ObservableCollection<Subject> SubjectData = new ObservableCollection<Subject>();
                SubFilter = new ObservableCollection<Subject>();
                foreach (var item in SubjectList)
                {
                    SubjectData.Add(item);
                    SubFilter.Add(item);
                }
                  
                Subjects = SubjectData;
                SubFilter.Add(new Subject() { Id = 1000, Name = "All" });
                SelectedSubject = SubFilter.First(x => x.Name.Equals("All"));
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in Report.xaml" + e.StackTrace);
                _logger.Error("Report.xaml error message is " + e.Message + " inner error is " + e.InnerException.Message);
            }
        }

        private bool canSave()
        {
            return true;
        }

        private void OnSave()
        {
            IsWorking = Visibility.Visible;
          
            RecRepo.UpdateLetterRecordAsync(SelectedRecord);
            onSearch();
            IsWorking = Visibility.Hidden;
        }

        private bool canSearch()
        {
            return true;
        }

        private void onSearch()
        {
       
                IsWorking = Visibility.Visible;

                List<long> idPO, idPS, idTA, idSrc, idSub, idStatus;
                idPO = new List<long>();
                idPS = new List<long>();
                idTA = new List<long>();
                idSrc = new List<long>();
                idSub = new List<long>();
                idStatus = new List<long>();
                if (SelectedAlpha.Name.Equals("All"))
                {
                    foreach (var item in Alphas)
                    {
                        idPO.Add(item.Id);
                    }
                }
                else
                {
                    idPO.Add(SelectedAlpha.Id);
                }

                if (SelectedPoliceStation.Name.Equals("All"))
                {
                    foreach (var item in PoliceStations)
                    {
                        idPS.Add(item.Id);
                    }
                }
                else
                {
                    idPS.Add(SelectedPoliceStation.Id);
                }

                if (SelectedTopic.Name.Equals("All"))
                {
                    foreach (var item in TopicsAndAreas)
                    {
                        idTA.Add(item.Id);
                    }
                }
                else
                {
                    idTA.Add(SelectedTopic.Id);
                }

                if (SelectedSource.Name.Equals("All"))
                {
                    foreach (var item in Sources)
                    {
                        idSrc.Add(item.Id);
                    }
                }
                else
                {
                    idSrc.Add(SelectedSource.Id);
                }

                if (SelectedSubject.Name.Equals("All"))
                {
                    foreach (var item in Subjects)
                    {
                        idSub.Add(item.Id);
                    }
                }
                else
                {
                    idSub.Add(SelectedSubject.Id);
                }

                if (SelectedStatus.Name.Equals("All"))
                {
                    foreach (var item in Statuses)
                    {
                        idStatus.Add(item.Id);
                    }
                }
                else
                {
                    idStatus.Add(SelectedStatus.Id);
                }
                letterRecords = RecRepo.GetRecordsAsync(idPS, idPO, idTA, idSrc, idSub, idStatus).Result.ToList();

                ObservableCollection<SqliteDataLayer.LetterRecord> RecData = new ObservableCollection<SqliteDataLayer.LetterRecord>();
                foreach (var item in letterRecords)
                    RecData.Add(item);
                ReportRecords = RecData;

                IsWorking = Visibility.Hidden;
            }
        

        private bool canExport()
        {
            if (ReportRecords == null)
            {
                return false;
            }
            else
            {
                return ReportRecords.Count > 0;
            }
            
        }


        private void onExportToPdf()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Pdf Files|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                var listOfItems= GetPDFDAta();
                var listOfColumns = GetColumnHeaders();
                int len = listOfColumns.Count;
                CreatePDFDataGrid create = new CreatePDFDataGrid(dialog.FileName, len);
                
            
                create.AddFilters("Alpha", SelectedAlpha.Name);
                create.AddFilters("Police Station", SelectedPoliceStation.Name);
                create.AddFilters("Topic Or Area", SelectedTopic.Name);
                create.AddFilters("Source", SelectedSource.Name);
                create.AddFilters("Subject", SelectedSubject.Name);
                create.AddFilters("Status", SelectedStatus.Name);
                //create.AddRecords(_reportRecords, _Alphas, _PoliceStations, _topicsAndAreas);
                create.AddHeader(listOfColumns);
                create.AddRecords(listOfItems);
                create.SaveAndClose();
            }
   
        }

        public List<List<string>> GetPDFDAta()
        {
            List<List<string>> ListOfRecord = new List<List<string>>();
           
            
            foreach(var item in ReportRecords)
            {
                List<string> SingleRecord = new List<string>();
                if (PdfFilterLetterNumber)
                {
                    SingleRecord.Add(item.LetterNumber);
                }

                if (PdfFilterStatus)
                {
                    SingleRecord.Add(Statuses.First(x=>x.Id.Equals(item.StatusID)).Name.ToString());
                }

                if (PdfFilterOfficeReceiptDate)
                {
                    SingleRecord.Add(item.OfficeReceiptDate);
                }

                if (PdfFilterTopicArea)
                {
                    SingleRecord.Add(TopicsAndAreas.First(x => x.Id.Equals(item.TopicAreaID)).Name.ToString());
                }

                if (PdfFilterPoliceStation)
                {
                    SingleRecord.Add(PoliceStations.First(x => x.Id.Equals(item.PoliceStationID)).Name.ToString());
                }

                if (PdfFilterAlpha)
                {
                    SingleRecord.Add(Alphas.First(x => x.Id.Equals(item.AlphaID)).Name.ToString());
                }

                if (PdfFilterOfficeDispatchNumber)
                {
                    SingleRecord.Add(item.OfficeDispatchNumber);
                }

                if (PdfFilterSource)
                {
                    SingleRecord.Add(Sources.First(x => x.Id.Equals(item.SourceID)).Name.ToString());
                }

                if (PdfFilterOfficeDispatchDate)
                {
                    SingleRecord.Add(item.OfficeDispatchDate);
                }

                if (PdfFilterOrganizationName)
                {
                    SingleRecord.Add(item.OrganizationName);
                }

                if (PdfFilterSanhaDetail)
                {
                    SingleRecord.Add(item.SanhaDetail);
                }

                if (PdfFilterVerificationDetail)
                {
                    SingleRecord.Add(item.VerificationDetail);
                }

                if (PdfFilterSubject)
                {
                    SingleRecord.Add(Subjects.First(x => x.Id.Equals(item.SubjectID)).Name.ToString());
                }

                if (PdfFilterPSDispatchNumber)
                {
                    SingleRecord.Add(item.PsDispatchNumber);
                }

                if (PdfFilterPSDispatchDate)
                {
                    SingleRecord.Add(item.PsDispatchDate);
                }

                if (PdfFilterCaseNumber)
                {
                    SingleRecord.Add(item.CaseNumber);
                }
                ListOfRecord.Add(SingleRecord);
            }

            return ListOfRecord;

        }

        public List<string> GetColumnHeaders()
        {
          List<string> ListOfColumn = new List<string>();
            if (PdfFilterLetterNumber)
            {
                ListOfColumn.Add("Letter number");
            }

            if (PdfFilterStatus)
            {
                ListOfColumn.Add("Status");
            }

            if (PdfFilterOfficeReceiptDate)
            {
                ListOfColumn.Add("Office Receipt Date");
            }

            if (PdfFilterTopicArea)
            {
                ListOfColumn.Add("Topic Or Area");
            }

            if (PdfFilterPoliceStation)
            {
                ListOfColumn.Add("Police Station");
            }

            if (PdfFilterAlpha)
            {
                ListOfColumn.Add("Alpha");
            }

            if (PdfFilterOfficeDispatchNumber)
            {
                ListOfColumn.Add("Office Dispatch Number");
            }

            if (PdfFilterSource)
            {
                ListOfColumn.Add("Source");
            }

            if (PdfFilterOfficeDispatchDate)
            {
                ListOfColumn.Add("Office Dispatch Date");
            }

            if (PdfFilterOrganizationName)
            {
                ListOfColumn.Add("Organization Name");
            }

            if (PdfFilterSanhaDetail)
            {
                ListOfColumn.Add("Sanha Detail");
            }

            if (PdfFilterVerificationDetail)
            {
                ListOfColumn.Add("Verification Detail");
            }

            if (PdfFilterSubject)
            {
                ListOfColumn.Add("Subject");
            }

            if (PdfFilterPSDispatchNumber)
            {
                ListOfColumn.Add("PS Dispatch Number");
            }

            if (PdfFilterPSDispatchDate)
            {
                ListOfColumn.Add("PS Dispatch Date");
            }

            if (PdfFilterCaseNumber)
            {
                ListOfColumn.Add("Case Number");
            }



            return ListOfColumn;
        }
    }
}
