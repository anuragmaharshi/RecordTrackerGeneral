﻿using GalaSoft.MvvmLight;
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
       
        IBetaRepository Betarepo;
        ObservableCollection<Beta> _Betas, _betaListFilter;

        IGammaRepository Gammarepo;
        ObservableCollection<Gamma> _Gammas, _gammaListFilter;

        IRecordRepository RecRepo;
        ObservableCollection<LetterRecord> _reportRecords;

        IThetaRepository ThetaRepo;
        ObservableCollection<Theta> _Thetas,_thetaListFilter;

        IDeltaRepository DeltaRepo;
        ObservableCollection<Delta> _Deltas,_deltaListFilter;

        StatusRepository StatusRepo;
        ObservableCollection<Status> _statusS,_statusFilter;

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
                    Betarepo = new BetaRepository();
                    Gammarepo = new GammaRepository();
                    RecRepo = new RecordRepository();
                    StatusRepo = new StatusRepository();
                    DeltaRepo = new DeltaRepository();
                    ThetaRepo = new ThetaRepository();
                    SaveRecord = new RelayCommand(OnSave, canSave);
                    SearchRecord = new RelayCommand(onSearch, canSearch);
                    ExportToPDF = new RelayCommand(onExportToPdf, canExport);
                    DeleteRecord = new RelayCommand(onDelete, canDelete);
                    PdfFilterZeus = true;
                    PdfFilterStatus = true;
                }
            }
            catch(Exception e)
            {
                _logger.Error("Some error have occured in RecordsReportViewModel, stacktrace=" + e.StackTrace);
                _logger.Error("RecordsReportViewModel error message is " + e.Message + " inner error is " + e.InnerException.Message);
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
        public ObservableCollection<Beta> Betas
        {
            get { return _Betas; }
            set { _Betas = value; RaisePropertyChanged("Betas"); }

        }
        public ObservableCollection<Beta> BetaFilter
        {
            get { return _betaListFilter; }
            set { _betaListFilter = value; RaisePropertyChanged("BetaFilter"); }

        }
        public ObservableCollection<Gamma> Gammas
        {
            get { return _Gammas; }
            set { _Gammas = value; RaisePropertyChanged("Gammas"); }

        }
        public ObservableCollection<Gamma> GammaFilter
        {
            get { return _gammaListFilter; }
            set { _gammaListFilter = value; RaisePropertyChanged("GammaFilter"); }

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
        public ObservableCollection<Theta> Thetas
        {
            get { return _Thetas; }
            set { _Thetas = value; RaisePropertyChanged("Thetas"); }

        }
        public ObservableCollection<Theta> ThetaFilter
        {
            get { return _thetaListFilter; }
            set { _thetaListFilter = value; RaisePropertyChanged("ThetaFilter"); }

        }
        public ObservableCollection<Delta> Deltas
        {
            get { return _Deltas; }
            set { _Deltas = value; RaisePropertyChanged("Deltas"); }

        }
        public ObservableCollection<Delta> DeltaFilter
        {
            get { return _deltaListFilter; }
            set { _deltaListFilter = value; RaisePropertyChanged("DeltaFilter"); }

        }
        #endregion

        #region Properties
        private Theta _selectedTheta;
        public Theta SelectedTheta
        {
            get { return _selectedTheta; }
            set
            {
                _selectedTheta = value;
                RaisePropertyChanged("SelectedTheta");
            }
        }


        private Delta _selectedDelta;
        public Delta SelectedDelta
        {
            get { return _selectedDelta; }
            set
            {
                _selectedDelta = value;
                RaisePropertyChanged("SelectedDelta");
            }
        }

        private LetterRecord _selectedRecord;
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

        private Alpha _selectedAlpha;
        public Alpha SelectedAlpha
        {
            get { return _selectedAlpha; }
            set
            {
                _selectedAlpha = value;
                RaisePropertyChanged("SelectedAlpha");
            }
        }

        private Beta _selectedBeta;
        public Beta SelectedBeta
        {
            get { return _selectedBeta; }
            set
            {
                _selectedBeta = value;
                RaisePropertyChanged("SelectedBeta");
            }
        }

        private Gamma _selectedGamma;
        public Gamma SelectedGamma
        {
            get { return _selectedGamma; }
            set
            {
                _selectedGamma = value;
                RaisePropertyChanged("SelectedGamma");
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

        private Visibility _isWorking;
        public Visibility IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value; RaisePropertyChanged("IsWorking");

            }
        }
        #endregion

        #region PDF filter
        private bool _pdfZeusFilter;
        public bool PdfFilterZeus
        {
            get { return _pdfZeusFilter; }
            set { _pdfZeusFilter = value; RaisePropertyChanged("PdfFilterZeus"); }
        }


        private bool _pdfStatusFilter;
        public bool PdfFilterStatus
        {
            get { return _pdfStatusFilter; }
            set { _pdfStatusFilter = value; RaisePropertyChanged("PdfFilterStatus"); }
        }

        private bool _pdfEndDateFilter;
        public bool PdfFilterEndDate
        {
            get { return _pdfEndDateFilter; }
            set { _pdfEndDateFilter = value; RaisePropertyChanged("PdfFilterEndDate"); }
        }

        private bool _gammaFilter;
        public bool PdfFilterGamma
        {
            get { return _gammaFilter; }
            set { _gammaFilter = value; RaisePropertyChanged("PdfFilterGamma"); }
        }

        private bool _betaFilter;
        public bool PdfFilterBeta
        {
            get { return _betaFilter; }
            set { _betaFilter = value; RaisePropertyChanged("PdfFilterBeta"); }
        }

        private bool _AlphaFilter;
        public bool PdfFilterAlpha
        {
            get { return _AlphaFilter; }
            set { _AlphaFilter = value; RaisePropertyChanged("PdfFilterAlpha"); }
        }

        private bool _pdfHeraFilter;
        public bool PdfFilterHera
        {
            get { return _pdfHeraFilter; }
            set { _pdfHeraFilter = value; RaisePropertyChanged("PdfFilterHera"); }
        }

        private bool _deltaFilter;
        public bool PdfFilterDelta
        {
            get { return _deltaFilter; }
            set { _deltaFilter = value; RaisePropertyChanged("PdfFilterDelta"); }
        }

        private bool _pdfBeginDateFilter;
        public bool PdfFilterBeginDate
        {
            get { return _pdfBeginDateFilter; }
            set { _pdfBeginDateFilter = value; RaisePropertyChanged("PdfFilterBeginDate"); }
        }

        private bool _pdfPoseidonFilter;
        public bool PdfFilterPoseidon
        {
            get { return _pdfPoseidonFilter; }
            set { _pdfPoseidonFilter = value; RaisePropertyChanged("PdfFilterPoseidon"); }
        }

        private bool _pdfAresFilter;
        public bool PdfFilterAres
        {
            get { return _pdfAresFilter; }
            set { _pdfAresFilter = value; RaisePropertyChanged("PdfFilterAres"); }
        }

        private bool _pdfAthenaFilter;
        public bool PdfFilterAthena
        {
            get { return _pdfAthenaFilter; }
            set { _pdfAthenaFilter = value; RaisePropertyChanged("PdfFilterAthena"); }
        }

        private bool _thetaFilter;
        public bool PdfFilterTheta
        {
            get { return _thetaFilter; }
            set { _thetaFilter = value; RaisePropertyChanged("PdfFilterTheta"); }
        }

        private bool _pdfArtemisFilter;
        public bool PdfFilterArtemis
        {
            get { return _pdfArtemisFilter; }
            set { _pdfArtemisFilter = value; RaisePropertyChanged("PdfFilterArtemis"); }
        }

        private bool _pdfTargetDateFilter;
        public bool PdfFilterTargetDate
        {
            get { return _pdfTargetDateFilter; }
            set { _pdfTargetDateFilter = value; RaisePropertyChanged("PdfFilterTargetDate"); }
        }

        private bool _pdfApolloFilter;
        public bool PdfFilterApollo
        {
            get { return _pdfApolloFilter; }
            set { _pdfApolloFilter = value; RaisePropertyChanged("PdfFilterApollo"); }
        }
#endregion

       

        public void LoadData()
        {
            try
            {
                _logger.Info("Inside Records viewer view model Load data");
                IsWorking = Visibility.Hidden;
                 var AlphaList = Alpharepo.GetAlphasAsync().Result.ToList();
                ObservableCollection<Alpha> AlphaData = new ObservableCollection<Alpha>();
                AlphaFilter = new ObservableCollection<Alpha>();
                foreach (var item in AlphaList)
                {
                    AlphaData.Add(item);
                    AlphaFilter.Add(item);
                }
                   
                Alphas = AlphaData;
                AlphaFilter.Add(new Alpha() { Id = 1000, Name = "All" });
                SelectedAlpha = AlphaFilter.First(x => x.Name.Equals("All"));

                var PSList = Betarepo.GetBetasAsync().Result.ToList();
                ObservableCollection<Beta> BetaData = new ObservableCollection<Beta>();
                BetaFilter = new ObservableCollection<Beta>();
                foreach (var item in PSList)
                {
                    BetaData.Add(item);
                    BetaFilter.Add(item);
                }
                   
                Betas = BetaData;
                BetaFilter.Add(new Beta() { Id = 1000, Name = "All" });
                SelectedBeta = BetaFilter.First(x => x.Name.Equals("All"));

                var GammaList = Gammarepo.GetGammasAsync().Result.ToList();
                ObservableCollection<Gamma> GammaData = new ObservableCollection<Gamma>();
                GammaFilter = new ObservableCollection<Gamma>();
                foreach (var item in GammaList)
                {
                    GammaData.Add(item);
                    GammaFilter.Add(item);
                }
                    
                Gammas = GammaData;
              
                GammaFilter.Add(new Gamma() { Id = 1000, Name = "All" });
                SelectedGamma = GammaFilter.First(x => x.Name.Equals("All"));
                

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

                var DeltaList = DeltaRepo.GetDeltasAsync().Result.ToList();
                ObservableCollection<Delta> DeltaData = new ObservableCollection<Delta>();
                DeltaFilter = new ObservableCollection<Delta>();
                foreach (var item in DeltaList)
                {
                    DeltaData.Add(item);
                    DeltaFilter.Add(item);
                }
                    
                Deltas = DeltaData;
                DeltaFilter.Add(new Delta() { Id = 1000, Name = "All" });
                SelectedDelta= DeltaFilter.First(x => x.Name.Equals("All"));

                var ThetaList = ThetaRepo.GetThetasAsync().Result.ToList();
                ObservableCollection<Theta> ThetaData = new ObservableCollection<Theta>();
                ThetaFilter = new ObservableCollection<Theta>();
                foreach (var item in ThetaList)
                {
                    ThetaData.Add(item);
                    ThetaFilter.Add(item);
                }
                  
                Thetas = ThetaData;
                ThetaFilter.Add(new Theta() { Id = 1000, Name = "All" });
                SelectedTheta = ThetaFilter.First(x => x.Name.Equals("All"));
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

                if (SelectedBeta.Name.Equals("All"))
                {
                    foreach (var item in Betas)
                    {
                        idPS.Add(item.Id);
                    }
                }
                else
                {
                    idPS.Add(SelectedBeta.Id);
                }

                if (SelectedGamma.Name.Equals("All"))
                {
                    foreach (var item in Gammas)
                    {
                        idTA.Add(item.Id);
                    }
                }
                else
                {
                    idTA.Add(SelectedGamma.Id);
                }

                if (SelectedDelta.Name.Equals("All"))
                {
                    foreach (var item in Deltas)
                    {
                        idSrc.Add(item.Id);
                    }
                }
                else
                {
                    idSrc.Add(SelectedDelta.Id);
                }

                if (SelectedTheta.Name.Equals("All"))
                {
                    foreach (var item in Thetas)
                    {
                        idSub.Add(item.Id);
                    }
                }
                else
                {
                    idSub.Add(SelectedTheta.Id);
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
                create.AddFilters("beta", SelectedBeta.Name);
                create.AddFilters("Gamma", SelectedGamma.Name);
                create.AddFilters("Delt", SelectedDelta.Name);
                create.AddFilters("Theta", SelectedTheta.Name);
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
                if (PdfFilterZeus)
                {
                    SingleRecord.Add(item.Zeus);
                }

                if (PdfFilterStatus)
                {
                    SingleRecord.Add(Statuses.First(x=>x.Id.Equals(item.StatusID)).Name.ToString());
                }

                if (PdfFilterEndDate)
                {
                    SingleRecord.Add(item.EndDate);
                }

                if (PdfFilterGamma)
                {
                    SingleRecord.Add(Gammas.First(x => x.Id.Equals(item.GammaID)).Name.ToString());
                }

                if (PdfFilterBeta)
                {
                    SingleRecord.Add(Betas.First(x => x.Id.Equals(item.BetaID)).Name.ToString());
                }

                if (PdfFilterAlpha)
                {
                    SingleRecord.Add(Alphas.First(x => x.Id.Equals(item.AlphaID)).Name.ToString());
                }

                if (PdfFilterHera)
                {
                    SingleRecord.Add(item.Hera);
                }

                if (PdfFilterDelta)
                {
                    SingleRecord.Add(Deltas.First(x => x.Id.Equals(item.DeltaID)).Name.ToString());
                }

                if (PdfFilterBeginDate)
                {
                    SingleRecord.Add(item.BeginDate);
                }

                if (PdfFilterPoseidon)
                {
                    SingleRecord.Add(item.Poseidon);
                }

                if (PdfFilterAres)
                {
                    SingleRecord.Add(item.Ares);
                }

                if (PdfFilterAthena)
                {
                    SingleRecord.Add(item.Athena);
                }

                if (PdfFilterTheta)
                {
                    SingleRecord.Add(Thetas.First(x => x.Id.Equals(item.ThetaID)).Name.ToString());
                }

                if (PdfFilterArtemis)
                {
                    SingleRecord.Add(item.Artemis);
                }

                if (PdfFilterTargetDate)
                {
                    SingleRecord.Add(item.TargetDate);
                }

                if (PdfFilterApollo)
                {
                    SingleRecord.Add(item.Apollo);
                }
                ListOfRecord.Add(SingleRecord);
            }

            return ListOfRecord;

        }

        public List<string> GetColumnHeaders()
        {
          List<string> ListOfColumn = new List<string>();
            if (PdfFilterZeus)
            {
                ListOfColumn.Add("Letter number");
            }

            if (PdfFilterStatus)
            {
                ListOfColumn.Add("Status");
            }

            if (PdfFilterEndDate)
            {
                ListOfColumn.Add("Office Receipt Date");
            }

            if (PdfFilterGamma)
            {
                ListOfColumn.Add("Gamma");
            }

            if (PdfFilterBeta)
            {
                ListOfColumn.Add("Beta");
            }

            if (PdfFilterAlpha)
            {
                ListOfColumn.Add("Alpha");
            }

            if (PdfFilterHera)
            {
                ListOfColumn.Add("Office Dispatch Number");
            }

            if (PdfFilterDelta)
            {
                ListOfColumn.Add("Delta");
            }

            if (PdfFilterBeginDate)
            {
                ListOfColumn.Add("Office Dispatch Date");
            }

            if (PdfFilterPoseidon)
            {
                ListOfColumn.Add("Organization Name");
            }

            if (PdfFilterAres)
            {
                ListOfColumn.Add("Sanha Detail");
            }

            if (PdfFilterAthena)
            {
                ListOfColumn.Add("Verification Detail");
            }

            if (PdfFilterTheta)
            {
                ListOfColumn.Add("Theta");
            }

            if (PdfFilterArtemis)
            {
                ListOfColumn.Add("PS Dispatch Number");
            }

            if (PdfFilterTargetDate)
            {
                ListOfColumn.Add("PS Dispatch Date");
            }

            if (PdfFilterApollo)
            {
                ListOfColumn.Add("Case Number");
            }



            return ListOfColumn;
        }

        private bool canDelete()
        {
            return SelectedRecord != null;
        }

        private void onDelete()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                RecRepo.DeleteRecordsAsync(SelectedRecord.Id);
                ReportRecords.Remove(SelectedRecord);
                SelectedRecord = null;
            }
        }
    }
}
