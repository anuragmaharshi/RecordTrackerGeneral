using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.SqliteDataLayer
{
    public class LetterRecord
    {
        public long Id { get; set; }
        public long StatusID { get; set; }
        public string Zeus { get; set; }
        public string EndDate { get; set; }
        public long GammaID { get; set; }
        public long BetaID { get; set; }
        public long AlphaID { get; set; }
        public string Hera { get; set; }
   
        public long DeltaID { get; set; }
        public string BeginDate { get; set; }
        public string Poseidon { get; set; }
        public string Ares { get; set; }
        public string Athena { get; set; }

        public long ThetaID { get; set; }
        public string Artemis { get; set; }
        public string TargetDate { get; set; }
        public string Apollo { get; set; }

        public string Remarks { get; set; }

        public string FormatBeginDate
        {
            get
            {
                try
                {
                    var dty = DateTime.Parse(BeginDate, CultureInfo.InvariantCulture);
                    return dty.ToString("yyyy-MM-dd");
                }
                catch 
                {
                    return null;
                }
               
            }
           
        }

        public string FormatEndDate
        {
            get
            {
                try
                {
                    var dty = DateTime.Parse(EndDate, CultureInfo.InvariantCulture);
                    return dty.ToString("yyyy-MM-dd");
                }
                catch
                {
                    return "";
                }
              
            }

        }

        public string FormatTargetDate
        {
            get
            {
                try
                {
                    var dty = DateTime.Parse(TargetDate, CultureInfo.InvariantCulture);
                    return dty.ToString("yyyy-MM-dd");
                }
                catch
                {
                    return "";
                }
              
            }

        }

        public override string ToString()
        {
            string value = string.Format("letter number {0} , OfficeReceiptDate = {1}, TopicAreaID = {2} ," +
                " OfficeDispatchNumber = {3},FormatPsDispatchDate = {4}",
                Zeus, EndDate, GammaID, Hera, FormatTargetDate);
            return value;
        }

    }
}
