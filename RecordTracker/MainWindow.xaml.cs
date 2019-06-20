using NLog;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace RecordTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public MainWindow()
        {
            try
            {
                _logger.Info("Inside Mainwindow .xaml");
                LogPath();
                if (!IsDBFileExist(Constants.GetDbFilePath()))
                    throw new FileNotFoundException("File not found at path"+ Constants.GetDbFilePath());
                InitializeComponent();
                this.Height = SystemParameters.WorkArea.Height;
                this.Width = SystemParameters.WorkArea.Width;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Maximized;
            }
            catch(FileNotFoundException e)
            {
                _logger.Error("Some error occured - error message is" + e.Message);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch(Exception e)
            {
                
                _logger.Error("Some error occured"+e.StackTrace);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void LogPath()
        {
            var fileName = System.IO.Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), "RecordTracker\\MainApplication.db");
            _logger.Info("Local app path"+fileName);
            _logger.Info("Check file " + IsDBFileExist(fileName));
        }

        private bool IsDBFileExist(string fileName)
        {
           return File.Exists(fileName);
        }
    
    }
}
