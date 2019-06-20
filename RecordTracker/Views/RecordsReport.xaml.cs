using NLog;
using RecordTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecordTracker.Views
{
    /// <summary>
    /// Interaction logic for RecordsReport.xaml
    /// </summary>
    public partial class RecordsReport : UserControl
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public RecordsReport()
        {
            try
            {
                _logger.Info("Inside RecordsReport.xaml. Initialising it");
                InitializeComponent();
               // LetterDataGrd = SystemParameters.WorkArea.Height;
                LetterDataGrd.Width = SystemParameters.WorkArea.Width-20;
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in Report.xaml" + e.StackTrace);
                _logger.Error("Report.xaml error message is " + e.Message+" inner error is "+e.InnerException.Message);
            }
            
        }

       
    }
}
