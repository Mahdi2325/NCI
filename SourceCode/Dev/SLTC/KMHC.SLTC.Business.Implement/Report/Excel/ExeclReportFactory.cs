using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Implement.Report.Excel;

namespace KMHC.SLTC.Business.Implement.Report
{
    public  class ExeclReportFactory
    {
        private static readonly string DllName = "KMHC.SLTC.Business.Implement.Report.Excel.";

        public static BaseExeclReport CreateReport(string type, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("type为空");
            }

            BaseExeclReport report = (BaseExeclReport)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(DllName + type, false);
            if (report == null)
            {
                throw new Exception("创建对象失败！");
            }
            report.TemplateName = type;
            report.StartTime = startTime;
            report.EndTime = endTime;
            return report;
        }

        public static TaskBaseExeclReport CreateReport(string type, DateTime startTime, DateTime endTime, int status)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("type为空");
            }
            TaskBaseExeclReport report = (TaskBaseExeclReport)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(DllName + type, false);
            if (report == null)
            {
                throw new Exception("创建对象失败！");
            }
            report.TemplateName = type;
            report.StartTime = startTime;
            report.EndTime = endTime;
            return report;
        }
        public static MonthFeeBaseExeclReport CreateReport(string type, DateTime startTime, DateTime endTime, string nsno)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("type为空");
            }
            MonthFeeBaseExeclReport report = (MonthFeeBaseExeclReport)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(DllName + type, false);
            if (report == null)
            {
                throw new Exception("创建对象失败！");
            }
            report.TemplateName = type;
            report.StartTime = startTime;
            report.EndTime = endTime;
            report.NsId = nsno;
            return report;
        }

        public static BaseExeclReport CreateReport(string type, string name, string idno, string nsId, string status)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("type为空");
            }

            BaseExeclReport report = (BaseExeclReport)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(DllName + type, false);
            if (report == null)
            {
                throw new Exception("创建对象失败！");
            }
            report.TemplateName = type;
            report.Name = name;
            report.IdNo = idno;
            report.NsId = nsId;
            report.Status = status;
            return report;
        }
    }
}
