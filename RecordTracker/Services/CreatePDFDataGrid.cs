using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;
using NLog;
using System.Data;
using RecordTracker.SqliteDataLayer;
using System.Collections.ObjectModel;
using iTextSharp.text.pdf.draw;

namespace RecordTracker.Services
{
    class CreatePDFDataGrid
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        PdfPTable filter,records;
        Document doc;
        PdfWriter writer;
        public CreatePDFDataGrid(string FileName)
        {
            InitialiseFilter();
            InitailseRecords();
            doc = new Document(iTextSharp.text.PageSize.LETTER, 25, 25, 30, 30);
            writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(FileName, System.IO.FileMode.Create));
            doc.Open();
           
        }
        public CreatePDFDataGrid(string FileName, int NoOfCols)
        {
            InitialiseFilter();
            InitailseRecords(NoOfCols);
            doc = new Document(iTextSharp.text.PageSize.LETTER, 25, 25, 30, 30);
            writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(FileName, System.IO.FileMode.Create));
            doc.Open();

        }
        private void InitialiseFilter()
        {
            filter = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Filter selected for records"));
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            filter.AddCell(cell);
        }

        private void InitailseRecords(int NoOfCols=9)
        {
            
            records = new PdfPTable(NoOfCols);
            PdfPCell cell = new PdfPCell(new Phrase("List of Records:"));
            cell.Colspan = NoOfCols;
            cell.HorizontalAlignment = 1;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            records.AddCell(cell);
        }

        public void AddHeader(List<string> Headers)
        {
           
            foreach (string str in Headers)
            {
                records.AddCell(str);
            }
        }

        public void AddFilters(string FilterName,string FilterValue)
        {
            filter.AddCell(FilterName);
            filter.AddCell(FilterValue);
        }

        public void AddRecords(ObservableCollection<LetterRecord> ReportRecords, ObservableCollection<Alpha> Alphas,
            ObservableCollection<PoliceStation> PoliceStations, ObservableCollection<TopicsAndArea> TopicsAndAreas)
        {
            foreach(var item in ReportRecords)
            {
                records.AddCell(item.OfficeReceiptDate.ToString());
                records.AddCell(item.LetterNumber.ToString());
                
                records.AddCell(item.OfficeDispatchNumber == null ? "" : item.OfficeDispatchNumber.ToString());
                records.AddCell(item.OfficeDispatchDate==null?"":item.OfficeDispatchDate.ToString());
                records.AddCell(Alphas.First(x=>x.Id== item.AlphaID).Name);
                records.AddCell(PoliceStations.First(x=>x.Id== item.PoliceStationID).Name);
                records.AddCell(TopicsAndAreas.First(x=>x.Id== item.TopicAreaID).Name);
                records.AddCell(item.StatusID.ToString());
                records.AddCell(item.Remarks);
            }
        }


        public void AddRecords(List<List<string>> AllData)
        {
            foreach (var item in AllData)
            {
                foreach(string str in item)
                {
                    records.AddCell(str);
                }
            }
        }
        public void SaveAndClose()
        {

            doc.Add(filter);
            Paragraph linebreak = new Paragraph(" ");
            doc.Add(linebreak);
            doc.Add(records);
            doc.Close();
        }



















        //    public void ExportToPdf(DataGrid grid)
        //    {
        //        PdfPTable table = new PdfPTable(grid.Columns.Count);
        //        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
        //        PdfWriter writer = PdfWriter.GetInstance(doc, new System.IO.FileStream("Test.pdf", System.IO.FileMode.Create));
        //        doc.Open();
        //        for (int j = 0; j < grid.Columns.Count; j++)
        //        {
        //            table.AddCell(new Phrase(grid.Columns[j].Header.ToString()));
        //        }
        //        table.HeaderRows = 1;
        //        IEnumerable itemsSource = grid.ItemsSource as IEnumerable;
        //        if (itemsSource != null)
        //        {
        //            foreach (var item in itemsSource)
        //            {
        //                DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
        //                if (row != null)
        //                {
        //                    DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
        //                    for (int i = 0; i < grid.Columns.Count; ++i)
        //                    {
        //                        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(i);
        //                        TextBlock txt = cell.Content as TextBlock;
        //                        if (txt != null)
        //                        {
        //                            table.AddCell(new Phrase(txt.Text));
        //                        }
        //                    }
        //                }
        //            }

        //            doc.Add(table);
        //            doc.Close();
        //        }
        //    }

        //    public static childItem FindVisualChild<childItem>(DependencyObject obj)
        //where childItem : DependencyObject
        //    {
        //        foreach (childItem child in FindVisualChildren<childItem>(obj))
        //        {
        //            return child;
        //        }

        //        return null;
        //    }

        //    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
        //   where T : DependencyObject
        //    {
        //        if (depObj != null)
        //        {
        //            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        //            {
        //                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
        //                if (child != null && child is T)
        //                {
        //                    yield return (T)child;
        //                }

        //                foreach (T childOfChild in FindVisualChildren<T>(child))
        //                {
        //                    yield return childOfChild;
        //                }
        //            }
        //        }
        //    }

        //    public void PrintDataGrid(DataGrid grid)
        //    {
        //        IEnumerable itemsSource = grid.ItemsSource as IEnumerable;
        //        //var datac = grid.DataContext as RecordVew;
        //        if (itemsSource != null)
        //        {
        //            foreach (var item in itemsSource)
        //            {
        //                DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
        //                if (row != null)
        //                {
        //                    var obj = (LetterRecord)row.Item;
        //                    _logger.Info(obj.LetterNumber);

        //                }
        //            }
        //        }


        //    }

        //    public void PrintHeader(DataGrid grid)
        //    {
        //        //header
        //        for (int i = 0; i < grid.Columns.Count; i++)
        //        {
        //            _logger.Info(grid.Columns[i].Header.ToString());
        //        }
        //    }
        //    public IEnumerable<System.Windows.Controls.DataGridRow> GetDataGridRows(System.Windows.Controls.DataGrid grid)
        //    {
        //        var itemsSource = grid.ItemsSource as IEnumerable;
        //        if (null == itemsSource) yield return null;
        //        foreach (var item in itemsSource)
        //        {
        //            var row = grid.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.DataGridRow;
        //            if (null != row) yield return row;
        //        }
        //    }
    }
}



