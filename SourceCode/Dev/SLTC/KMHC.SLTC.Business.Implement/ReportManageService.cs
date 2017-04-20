/*
创建人: 肖国栋
创建日期:2016-03-18
说明:指標管理
*/
using AutoMapper;
using ExcelReport;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using KMHC.SLTC.Repository.Base;
using NPOI.SS.Formula.Functions;
using System.Data.Linq.SqlClient;
using KMHC.SLTC.Business.Entity.Report.Excel;

namespace KMHC.SLTC.Business.Implement
{
    public class ReportManageService : BaseService, IReportManageService
    {
    }
}