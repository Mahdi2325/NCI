using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Implement.Report;

namespace KMHC.SLTC.WebController
{
    public partial class ReportController : Controller
    {
        public ActionResult Export(string fileTypeStr, string templateName, string startTimeStr, string endTimeStr)
        {
            if (fileTypeStr == ReportFileType.Xls.ToString().ToLower())
            {
                return this.ExportXls(templateName, startTimeStr, endTimeStr);
            }
            return View();
        }

        public ActionResult ExportRegInHosStatus(string fileTypeStr, string templateName, string name,string idno,string nsId,string status)
        {
            if (fileTypeStr == ReportFileType.Xls.ToString().ToLower())
            {
                return this.ExportXls(templateName,  name, idno, nsId, status);
            }
            return View();
        } 

        public ActionResult ExportXls(string templateName, string startTimeStr, string endTimeStr)
        {
            if (templateName == null) return null;
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;

            DateTime.TryParse(startTimeStr, out startTime);
            DateTime.TryParse(endTimeStr, out endTime);
            var re = ExeclReportFactory.CreateReport(templateName, startTime, endTime);   
            re.BindData();
            re.Download();
            return null;
        }

        public ActionResult ExportXls(string templateName, string name, string idno, string nsId, string status)
        {
            if (templateName == null) return null;
            var re = ExeclReportFactory.CreateReport(templateName, name,  idno,  nsId,  status);
            re.BindData();
            re.Download();
            return null;
        }

        public ActionResult ExportDoc(string templateName, string startTimeStr, string endTimeStr)
        {
            throw new NotImplementedException();
        } 
    }
}
