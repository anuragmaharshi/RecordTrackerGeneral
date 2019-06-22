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
        #region variables
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

        #endregion

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
      
        private string _zeus ;
        public string Zeus
        {
            get { return _zeus; }
            set {
                
                if (value != "")
                    _zeus = value;
                else
                    _zeus = null;
                RaisePropertyChanged("Zeus");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _endDate;
        public string EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RaisePropertyChanged("EndDate");
                CancelRecord.RaiseCanExecuteChanged();
                AddNewRecord.RaiseCanExecuteChanged();
            }
        }

        private string _beginDate;
        public string BeginDate
        {
            get { return _beginDate; }
            set
            {

                _beginDate = value;
                RaisePropertyChanged("BeginDate");
                CancelRecord.RaiseCanExecuteChanged();
                AddNewRecord.RaiseCanExecuteChanged();
            }
        }

        private string _hera;
        public string Hera
        {
            get { return _hera; }
            set {
                if (value != "")
                    _hera = value;
                else
                    _hera = null;
                
                RaisePropertyChanged("Hera");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _targetDate;
        public string TargetDate
        {
            get { return _targetDate; }
            set
            {

                _targetDate = value;
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
                RaisePropertyChanged("TargetDate");

            }
        }

        private string _artemis;
        public string Artemis
        {
            get { return _artemis; }
            set
            {
                if (value != "")
                    _artemis = value;
                else
                    _artemis = null;

                RaisePropertyChanged("Artemis");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _ares;
        public string Ares
        {
            get { return _ares; }
            set
            {
                _ares = value;
                RaisePropertyChanged("Ares");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _athena;
        public string Athena
        {
            get { return _athena; }
            set
            {
                _athena = value;
                RaisePropertyChanged("Athena");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }

        private string _apollo;
        public string Apollo
        {
            get { return _apollo; }
            set
            {
                if (value != "")
                    _apollo = value;
                else
                    _apollo = null;
                RaisePropertyChanged("Apollo");
                AddNewRecord.RaiseCanExecuteChanged();
                CancelRecord.RaiseCanExecuteChanged();
            }
        }


        private string _poseidon;
        public string Poseidon
        {
            get { return _poseidon; }
            set
            {
                _poseidon = value;
                RaisePropertyChanged("Poseidon");
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
            return Zeus != null;

        }

        private void OnAdd()
        {
            LetterRecord record = new LetterRecord
            {
                Zeus = Zeus
            };

            if (Hera != null)
                record.Hera = Hera;

            if (SelectedDelta != null)
                record.DeltaID = SelectedDelta.Id;
           
            if(BeginDate != null)
                record.BeginDate = FormatDate(BeginDate);

            if(EndDate != null)
                record.EndDate = FormatDate(EndDate);

            if(Poseidon != null)
                record.Poseidon = Poseidon;

            if (Ares != null)
                record.Ares = Ares;

            if (Athena != null)
                record.Athena = Athena;

            if (SelectedTheta != null)
                record.ThetaID = SelectedTheta.Id;
            
            if(Artemis != null)
                record.Artemis = Artemis;

            if (TargetDate != null)
                record.TargetDate = FormatDate(TargetDate);

            if (SelectedGamma != null)
                record.GammaID = SelectedGamma.Id;
           
            if (SelectedAlpha != null)
                record.AlphaID = SelectedAlpha.Id;
           
            if (SelectedBeta != null)
                record.BetaID = SelectedBeta.Id;
            
            record.StatusID = 1;
            record.Remarks = Remarks;

            if(Apollo != null)
                record.Apollo = Apollo;
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
                || Zeus!=null || Hera != null|| BeginDate != null 
                || EndDate != null || SelectedDelta!=null || SelectedTheta!=null || TargetDate != null
                || Artemis != null || Ares != null || Athena != null || Apollo != null
                || Poseidon != null || Remarks!=null;
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
            Zeus = null;
            Hera = null;
            BeginDate = null;
            EndDate = null;

            TargetDate = null;
            Artemis = null;
            Ares = null;
            Athena = null;
            Apollo = null;
            Poseidon = null;
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
