using ExcelReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public abstract class TaskBaseExeclReport
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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //机构编码 BobDu添加
        public string nsno { get; set; }

        protected SheetFormatter Formatter { get; set; }
        protected SheetFormatter Formatter1 { get; set; }
        protected SheetFormatter Formatter2 { get; set; }

        public async Task BindData()
        {
            await CreatFormatter();
        }

        protected abstract Task CreatFormatter();

        public void Download()
        {
            ExportHelper.ExportToWeb(TemplateFilePath, TargetFilePath, Formatter, Formatter1, Formatter2);
        }
    }
}
