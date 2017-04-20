using System;
using System.Web;
using ExcelReport;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public abstract class BaseExeclReport
    {
        protected abstract string FileNamePrefix { get; }

        public string TemplateName;

        protected static string TemplateRootPath = string.Format(@"{0}Templates\", HttpContext.Current.Server.MapPath(VirtualPathUtility.GetDirectory("~")));

        protected string TemplateFormatterPath
        {
            get
            {
                return string.Format(@"{0}{1}.xml", TemplateRootPath, TemplateName);
            }
        }
        private string TemplateFilePath
        {
            get
            {
                return string.Format(@"{0}{1}.xls", TemplateRootPath, TemplateName);
            }
        }
        private string TargetFilePath
        {
            get
            {
                return string.Format(@"{0}{1:yyyMMddHHmmss}.xls", FileNamePrefix, DateTime.Now);
            }
        }
        public DateTime StartTime{ get; set; }
        public DateTime EndTime { get; set; }
        public string Name{ get; set; }
        public string IdNo { get; set; }
        public string NsId{ get; set; }
        public string Status { get; set; }

        protected SheetFormatter Formatter { get; set; }

        public void BindData()
        {
            CreatFormatter();
        }

        protected abstract void CreatFormatter();

        public void Download()
        {
            ExportHelper.ExportToWeb(TemplateFilePath, TargetFilePath, Formatter);
        }
    }
}
