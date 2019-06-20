using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RecordTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _logger.Info("Starting Application- App.xaml");
            SetupExceptionHandling();

            //Style dpStyle = new Style(typeof(System.Windows.Controls.DatePicker));
            //dpStyle.Setters.Add(new Setter(System.Windows.Controls.DatePicker.LanguageProperty, System.Windows.Markup.XmlLanguage.GetLanguage("en-US")));
            //this.Resources.Add(typeof(System.Windows.Controls.DatePicker), dpStyle);
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException += (s, e) =>
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception in LogUnhandledException");
            }
            finally
            {
                _logger.Error(exception, message);
                _logger.Error("Error messge is "+ exception.Message);
                _logger.Error("Inner Error messge is " + exception.InnerException.Message);
                _logger.Error("Stack trace messge is " + exception.InnerException.StackTrace);
            }
        }

        //void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        //{
        //    _logger.Error(e.Exception.Message);
        //    e.Handled = true;
        //}
    }
}
