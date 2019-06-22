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

        IBetaRepository Betarepo;
        ObservableCollection<Beta> _Betas;

        IGammaRepository Gammarepo;
        ObservableCollection<Gamma> _Gammas;

        IThetaRepository ThetaRepo;
        ObservableCollection<Theta> _Thetas;

        IDeltaRepository DeltaRepo;
        ObservableCollection<Delta> _Deltas;

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
                    Betarepo = new BetaRepository();
                    Gammarepo = new GammaRepository();
                    DeltaRepo = new DeltaRepository();
                    ThetaRepo = new ThetaRepository();
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
        public ObservableCollection<Beta> Betas
        {
            get { return _Betas; }
            set { _Betas = value; RaisePropertyChanged("Betas"); }

        }

        public ObservableCollection<Gamma> Gammas
        {
            get { return _Gammas; }
            set { _Gammas = value; RaisePropertyChanged("Gammas"); }

        }

        public ObservableCollection<Theta> Thetas
        {
            get { return _Thetas; }
            set { _Thetas = value; RaisePropertyChanged("Thetas"); }

        }

        public ObservableCollection<Delta> Deltas
        {
            get { return _Deltas; }
            set { _Deltas = value; RaisePropertyChanged("Deltas"); }

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


        private Beta _selectedBeta;
        public Beta SelectedBeta
        {
          get { return _selectedBeta; }
          set {
                _selectedBeta = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedBeta");
              }
        }

        private Gamma _selectedGamma;
        public Gamma SelectedGamma
        {
            get { return _selectedGamma; }
            set {
                _selectedGamma = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedGamma");
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


        private Theta _selectedTheta;
        public Theta SelectedTheta
        {
            get { return _selectedTheta; }
            set
            {
                _selectedTheta = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
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
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedDelta");
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

                var BetaList = Betarepo.GetBetasAsync().Result.ToList();
                ObservableCollection<Beta> BetaData = new ObservableCollection<Beta>();
                foreach (var item in BetaList)
                    BetaData.Add(item);
                Betas = BetaData;

                var GammaList = Gammarepo.GetGammasAsync().Result.ToList();
                ObservableCollection<Gamma> GammaData = new ObservableCollection<Gamma>();
                foreach (var item in GammaList)
                    GammaData.Add(item);
                Gammas = GammaData;

                var DeltaList = DeltaRepo.GetDeltasAsync().Result.ToList();
                ObservableCollection<Delta> DeltaData = new ObservableCollection<Delta>();
                foreach (var item in DeltaList)
                    DeltaData.Add(item);
                Deltas = DeltaData;

                var ThetaList = ThetaRepo.GetThetasAsync().Result.ToList();
                ObservableCollection<Theta> ThetaData = new ObservableCollection<Theta>();
                foreach (var item in ThetaList)
                    ThetaData.Add(item);
                Thetas = ThetaData;

                SelectedBeta = Betas.First(x => x.Id.Equals(1));
                SelectedGamma = Gammas.First(x => x.Id.Equals(1));
                SelectedAlpha = Alphas.First(x => x.Id.Equals(1));
                SelectedDelta = Deltas.First(x => x.Id.Equals(1));
                SelectedTheta = Thetas.First(x => x.Id.Equals(1));
                SaveText = null;
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

            if (SelectedDelta != null)
                record.DeltaID = SelectedDelta.Id;
           
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

            if (SelectedTheta != null)
                record.ThetaID = SelectedTheta.Id;
            
            if(PsDispatchNumber!=null)
                record.PsDispatchNumber = PsDispatchNumber;

            if(PsDispatchDate!=null)
                record.PsDispatchDate = FormatDate(PsDispatchDate);

            if (SelectedGamma != null)
                record.GammaID = SelectedGamma.Id;
           
            if (SelectedAlpha != null)
                record.AlphaID = SelectedAlpha.Id;
           
            if (SelectedBeta != null)
                record.BetaID = SelectedBeta.Id;
            
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
            return SelectedBeta != null || SelectedGamma != null || SelectedAlpha != null 
                || LetterNumber!=null || OfficeDispatchNumber != null|| OfficeDispatchDate != null 
                || OfficeReceiptDate != null || SelectedDelta!=null || SelectedTheta!=null || PsDispatchDate!=null
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
            SelectedBeta = Betas.First(x => x.Id.Equals(1));
            SelectedGamma = Gammas.First(x => x.Id.Equals(1));
            SelectedAlpha = Alphas.First(x => x.Id.Equals(1));
            SelectedDelta = Deltas.First(x => x.Id.Equals(1));
            SelectedTheta = Thetas.First(x => x.Id.Equals(1));
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
