using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Report.Excel;
using System.Collections;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;

namespace KMHC.SLTC.Business.Implement
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IDictManageService _dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
        private static Dictionary<string, string> _diseaseCodeCache = new Dictionary<string, string>();
        private static Dictionary<int, string> _careTypeCache;
        //private static Dictionary<int, int> _careTypeCacheTwo;
        private string ConvertCareType(int dbValue)
        {
            var outPut = "";
            if (_careTypeCache == null)
            {
                _careTypeCache = new Dictionary<int, string>();
                var careTypeList = unitOfWork.GetRepository<NCI_CARETYPE>().dbSet.ToList();
                foreach (var careType in careTypeList)
                {
                    _careTypeCache.Add(careType.CARETYPEID, careType.CARETYPENAME);
                }
            }
            if (_careTypeCache.ContainsKey(dbValue))
            {
                outPut = _careTypeCache[dbValue];
            }

            return outPut;
        }

        //private int ConvertCareTypeTwo(int dbValue)
        //{
        //    if (_careTypeCacheTwo == null)
        //    {
        //        _careTypeCacheTwo = new Dictionary<int, int>();
        //        var careTypeList = unitOfWork.GetRepository<NCI_CARETYPE>().dbSet.ToList();
        //        foreach (var careType in careTypeList)
        //        {
        //            _careTypeCacheTwo.Add(careType.CARETYPEID, careType.CARETYPE);
        //        }
        //    }
        //    if (!_careTypeCacheTwo.ContainsKey(dbValue))
        //    {
        //        throw new Exception(string.Format("Invalid careTypeId:{0}", dbValue));
        //    }

        //    return _careTypeCacheTwo[dbValue];
        //}
        private string ConvertDisease(string disease)
        {
            if (_diseaseCodeCache.ContainsKey(disease))
            {
                return _diseaseCodeCache[disease];
            }

            var disCodeDtl = _dictManageService.GetCode(disease, "A003");
            if (disCodeDtl == null || disCodeDtl.Data == null)
            {
                return "未知";
            }
            _diseaseCodeCache.Add(disease, disCodeDtl.Data.ItemName);

            return disCodeDtl.Data.ItemName;
        }

        private void GetFreeReportComSourceData(DateTime? starTime, DateTime? endTime
                                                , out List<NCIP_NSMONFEE> totalNsMonFee
                                                , out List<NCIP_RESIDENTMONFEE> totalResMonFee
                                                , out List<NCIP_NSMONFEE> curNsMonFee
                                                , out List<NCIP_RESIDENTMONFEE> curResMonFee)
        {
            var startYearMon = "";
            var endYearMon = "";
            if (starTime.HasValue)
            {
                startYearMon = starTime.Value.ToString("yyyy-MM");
            }
            if (starTime.HasValue)
            {
                endYearMon = endTime.Value.ToString("yyyy-MM");
            }
            totalNsMonFee = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.ISDELETE != true && (m.STATUS == (int)NCIPStatusEnum.Passed || m.STATUS == (int)NCIPStatusEnum.Appropriated)).ToList();
            totalResMonFee = totalNsMonFee.SelectMany(m => m.NCIP_RESIDENTMONFEE).ToList();

            curNsMonFee = totalNsMonFee.Where(m => m.YEARMONTH.CompareTo(startYearMon) >= 0 && m.YEARMONTH.CompareTo(endYearMon) <= 0).ToList();
            curResMonFee = curNsMonFee.SelectMany(m => m.NCIP_RESIDENTMONFEE).ToList();
        }
        private FeeReportComItem FillFeeReportComItem(IEnumerable<NCIP_RESIDENTMONFEE> resData, IEnumerable<NCIP_SERVICEDEPOSITGRANT> svcGrantData = null)
        {
            var item = new FeeReportComItem();
            if (resData == null || resData.Count() <= 0)
            {
                return item;
            }

            item.ResNum = resData.Count();
            item.HospDay = resData.Sum(m => m.HOSPDAY);
            item.Fee = resData.Sum(m => m.TOTALAMOUNT);

            var medFeeTypeStr = FeeType.药品.ToString();

            var medData = resData.SelectMany(r => r.NCIP_RESIDENTMONFEEDTL
                                        .Where(d => d.FEETYPE == medFeeTypeStr))
                                .ToList();


            item.MedFee = medData.Sum(m => m.AMOUNT);
            item.NciPay = resData.Sum(m => m.NCIPAY);


            if (svcGrantData != null)
            {
                item.SvcDep = svcGrantData.Sum(m => m.DUEOFPAY);
                item.Decut = svcGrantData.Sum(m => m.DECUT ?? 0);
                item.AccNciPay = svcGrantData.Sum(m => m.ACTRUALPAY);
            }

            return item;
        }
        public FeeReportComData GetFeeByCareTypeReport(DateTime? starTime, DateTime? endTime)
        {
            var startYearMon = "";
            var endYearMon = "";
            if (starTime.HasValue)
            {
                startYearMon = starTime.Value.ToString("yyyy-MM");
            }
            if (starTime.HasValue)
            {
                endYearMon = endTime.Value.ToString("yyyy-MM");
            }
            var reportData = new FeeReportComData();
            List<NCIP_NSMONFEE> totalNsMonFee, curNsMonFee;
            List<NCIP_RESIDENTMONFEE> totalResMonFee, curResMonFee;
            GetFreeReportComSourceData(starTime, endTime, out totalNsMonFee, out totalResMonFee, out curNsMonFee, out curResMonFee);
            //var totalSvcDepGrant = unitOfWork.GetRepository<NCIP_SERVICEDEPOSITGRANT>().dbSet.Where(m => m.ISDELETE != true).ToList();
            //var curSvcDepGrant = totalSvcDepGrant.Where(m => m.CREATETIME >= starTime && m.CREATETIME <= endTime).ToList();

            reportData.StartDate = startYearMon;
            reportData.EndDate = endYearMon;
            reportData.SumCurrent = FillFeeReportComItem(curResMonFee);
            reportData.SumTotal = FillFeeReportComItem(totalResMonFee);

            var totalDetail = GetFeeByCareTypeReportDetail(totalResMonFee, false);
            var curDetail = GetFeeByCareTypeReportDetail(curResMonFee, true);

            var q = from c in curDetail
                    from t in totalDetail
                    where c.GroupColumn1 == t.GroupColumn1 && c.GroupColumn2 == t.GroupColumn2
                    select new FeeReportComDetail()
                    {
                        GroupColumn1 = c.GroupColumn1,
                        GroupColumn2 = c.GroupColumn2,
                        Current = c.Current,
                        Total = t.Total
                    };
            reportData.DataDetail = q.OrderBy(m => m.GroupColumn1).ThenBy(m => m.GroupColumn2).ToList();
            var index = 1;
            reportData.DataDetail.ForEach(p => p.Index = index++);
            return reportData;
        }
        private List<FeeReportComDetail> GetFeeByCareTypeReportDetail(IEnumerable<NCIP_RESIDENTMONFEE> resData, bool isCur)
        {
            var q = from r in resData
                    join Applicant in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.ISDELETE == false) on r.RESIDENTSSID equals Applicant.IDNO
                    join a in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(m => m.ISDELETE != true && m.AGENCYRESULT == 6) on Applicant.APPLICANTID equals a.APPLICANTID
                    join n in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(m => m.ISDELETE != true) on r.NSID equals n.NSID
                    group r by new { n.NSNAME, a.CARETYPE } into grp
                    select new
                    {
                        OrgName = grp.Key.NSNAME,
                        ResCareType = grp.Key.CARETYPE,
                        ReMonFeeList = (from v in grp select v).ToList()
                    };
            var resExtendData = q.ToList();
            var genDetail = resExtendData.Select(x =>
            {
                var detail = new FeeReportComDetail();
                detail.GroupColumn1 = x.OrgName;
                detail.GroupColumn2 = ConvertCareType(x.ResCareType);
                if (isCur)
                {
                    detail.Current = FillFeeReportComItem(x.ReMonFeeList);
                }
                else
                {
                    detail.Total = FillFeeReportComItem(x.ReMonFeeList);
                }
                return detail;
            }).ToList();

            return genDetail;
        }
        public FeeReportComData GetFeeByOrgReport(DateTime? starTime, DateTime? endTime)
        {
            var startYearMon = "";
            var endYearMon = "";
            if (starTime.HasValue)
            {
                startYearMon = starTime.Value.ToString("yyyy-MM");
            }
            if (starTime.HasValue)
            {
                endYearMon = endTime.Value.ToString("yyyy-MM");
            }
            var reportData = new FeeReportComData();
            List<NCIP_NSMONFEE> totalNsMonFee, curNsMonFee;
            List<NCIP_RESIDENTMONFEE> totalResMonFee, curResMonFee;
            GetFreeReportComSourceData(starTime, endTime, out totalNsMonFee, out totalResMonFee, out curNsMonFee, out curResMonFee);

            reportData.StartDate = startYearMon;
            reportData.EndDate = endYearMon;
            reportData.SumCurrent = FillFeeReportComItem(curResMonFee);
            reportData.SumTotal = FillFeeReportComItem(totalResMonFee);

            var totalDetail = GetFeeByOrgReportDetail(totalResMonFee, false);
            var curDetail = GetFeeByOrgReportDetail(curResMonFee, true);

            var q = from c in curDetail
                    from t in totalDetail
                    where c.GroupColumn1 == t.GroupColumn1 && c.GroupColumn2 == t.GroupColumn2
                    select new FeeReportComDetail()
                    {
                        GroupColumn1 = c.GroupColumn1,
                        GroupColumn2 = c.GroupColumn2,
                        Current = c.Current,
                        Total = t.Total
                    };
            reportData.DataDetail = q.ToList();
            var index = 1;
            reportData.DataDetail.ForEach(p => p.Index = index++);
            return reportData;
        }
        private List<FeeReportComDetail> GetFeeByOrgReportDetail(IEnumerable<NCIP_RESIDENTMONFEE> resData, bool isCur)
        {
            var q = from r in resData
                    join n in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(m => m.ISDELETE != true) on r.NSID equals n.NSID
                    group r by n.NSNAME into grp
                    select new
                    {
                        OrgName = grp.Key,
                        ReMonFeeList = (from v in grp select v).ToList()
                    };
            var resExtendData = q.ToList();
            var genDetail = resExtendData.Select(x =>
            {
                var detail = new FeeReportComDetail();
                detail.GroupColumn1 = x.OrgName;
                detail.GroupColumn2 = string.Empty;
                if (isCur)
                {
                    detail.Current = FillFeeReportComItem(x.ReMonFeeList);
                }
                else
                {
                    detail.Total = FillFeeReportComItem(x.ReMonFeeList);
                }
                return detail;
            }).ToList();

            return genDetail;
        }

        public FeeReportComData GetFeeByLevelReport(DateTime? starTime, DateTime? endTime)
        {
            var startYearMon = "";
            var endYearMon = "";
            if (starTime.HasValue)
            {
                startYearMon = starTime.Value.ToString("yyyy-MM");
            }
            if (starTime.HasValue)
            {
                endYearMon = endTime.Value.ToString("yyyy-MM");
            }
            var reportData = new FeeReportComData();
            List<NCIP_NSMONFEE> totalNsMonFee, curNsMonFee;
            List<NCIP_RESIDENTMONFEE> totalResMonFee, curResMonFee;
            GetFreeReportComSourceData(starTime, endTime, out totalNsMonFee, out totalResMonFee, out curNsMonFee, out curResMonFee);

            reportData.StartDate = startYearMon;
            reportData.EndDate = endYearMon;
            reportData.SumCurrent = FillFeeReportComItem(curResMonFee);
            reportData.SumTotal = FillFeeReportComItem(totalResMonFee);

            var totalDetail = GetFeeByLevelReportDetail(totalResMonFee, false);
            var curDetail = GetFeeByLevelReportDetail(curResMonFee, true);

            var q = from c in curDetail
                    from t in totalDetail
                    where c.GroupColumn1 == t.GroupColumn1 && c.GroupColumn2 == t.GroupColumn2
                    select new FeeReportComDetail()
                    {
                        GroupColumn1 = c.GroupColumn1,
                        GroupColumn2 = c.GroupColumn2,
                        Current = c.Current,
                        Total = t.Total
                    };
            reportData.DataDetail = q.ToList();
            var index = 1;
            reportData.DataDetail.ForEach(p => p.Index = index++);
            return reportData;
        }
        private List<FeeReportComDetail> GetFeeByLevelReportDetail(IEnumerable<NCIP_RESIDENTMONFEE> resData, bool isCur)
        {
            var q = from r in resData
                    join Applicant in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.ISDELETE == false) on r.RESIDENTSSID equals Applicant.IDNO
                    join a in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(m => m.ISDELETE != true && m.AGENCYRESULT == 6) on Applicant.APPLICANTID equals a.APPLICANTID
                    group r by a.CARETYPE into grp
                    select new
                    {
                        CareType = grp.Key,
                        ReMonFeeList = (from v in grp select v).ToList()
                    };
            var resExtendData = q.ToList();
            var genDetail = resExtendData.Select(x =>
            {
                var detail = new FeeReportComDetail();
                detail.GroupColumn1 = ConvertCareType(x.CareType);
                detail.GroupColumn2 = string.Empty;
                if (isCur)
                {
                    detail.Current = FillFeeReportComItem(x.ReMonFeeList);
                }
                else
                {
                    detail.Total = FillFeeReportComItem(x.ReMonFeeList);
                }
                return detail;
            }).ToList();

            return genDetail;
        }

        //TODO FeeByDiseaseReportData 因为目前数据无法区分乡镇 县级 和县级以上, 所以暂时只用县级数据统计
        public FeeByDiseaseReportData GetFeeByDiseaseReport(DateTime? starTime, DateTime? endTime)
        {
            var startYearMon = "";
            var endYearMon = "";
            if (starTime.HasValue)
            {
                startYearMon = starTime.Value.ToString("yyyy-MM");
            }
            if (starTime.HasValue)
            {
                endYearMon = endTime.Value.ToString("yyyy-MM");
            }
            var reportData = new FeeByDiseaseReportData();
            var resData = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.ISDELETE != true
                                                                           && m.YEARMONTH.CompareTo(startYearMon) >= 0 &&
                                                                           m.YEARMONTH.CompareTo(endYearMon) <= 0 && (m.STATUS == (int)NCIPStatusEnum.Passed || m.STATUS == (int)NCIPStatusEnum.Appropriated))
                                                                           .SelectMany(x => x.NCIP_RESIDENTMONFEE).ToList();
            var q = from r in resData
                    join Applicant in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.ISDELETE == false) on r.RESIDENTSSID equals Applicant.IDNO
                    join a in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(m => m.ISDELETE != true && m.AGENCYRESULT == 6) on Applicant.APPLICANTID equals a.APPLICANTID
                    group r by a.DISEASE into grp
                    select new
                    {
                        Disease = grp.Key,
                        ReMonFeeList = (from v in grp select v).ToList()
                    };
            var resExtendData = q.ToList();
            var countyDetail = resExtendData.Select(x => FillFeeByDiseaseReportItem(x.ReMonFeeList, x.Disease)).ToList();
            reportData.StartDate = startYearMon;
            reportData.EndDate = endYearMon;
            reportData.DataDetail = countyDetail;
            var index = 1;
            reportData.DataDetail.ForEach(p => p.Index = index++);
            return reportData;
        }
        private FeeByDiseaseReportItem FillFeeByDiseaseReportItem(IEnumerable<NCIP_RESIDENTMONFEE> resData, string disease)
        {
            var item = new FeeByDiseaseReportItem();
            if (resData == null || resData.Count() <= 0)
            {
                return item;
            }
            item.Disease = ConvertDisease(disease);
            item.ResNum = resData.Count();
            item.Fee = resData.Sum(m => m.TOTALAMOUNT);
            item.NciPay = resData.Sum(m => m.NCIPAY);
            return item;
        }

        public FeeDetailData GetFeeDetailReport(DateTime? starTime, DateTime? endTime)
        {
            var response = new FeeDetailData();
            try
            {
                var startYearMon = "";
                var endYearMon = "";
                if (starTime.HasValue)
                {
                    startYearMon = starTime.Value.ToString("yyyy-MM");
                }
                if (starTime.HasValue)
                {
                    endYearMon = endTime.Value.ToString("yyyy-MM");
                }

                var feeDataDetail = new List<FeeDetail>();
                response.DataDetail = new List<FeeDetail>();
                var q = from m in unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet
                        join Applicant in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.ISDELETE == false) on m.RESIDENTSSID equals Applicant.IDNO
                        join s in unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.ISDELETE == false && (m.STATUS == (int)NCIPStatusEnum.Passed || m.STATUS == (int)NCIPStatusEnum.Appropriated)) on m.NSMONFEEID equals s.NSMONFEEID
                        join o in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on m.NSID equals o.NSID into oNursinghome
                        join l in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(m => m.ISDELETE == false && m.AGENCYRESULT == 6) on Applicant.APPLICANTID equals l.APPLICANTID into lApphosp
                        from Nursinghome in oNursinghome.DefaultIfEmpty()
                        from Apphosp in lApphosp.DefaultIfEmpty()
                        select new FeeDetail
                        {
                            RsmonfeeId = m.RSMONFEEID,
                            Name = Applicant.NAME,
                            Sex = Applicant.GENDER,
                            Age = DateTime.Now.Year - Applicant.BIRTHDATE.Year + 1,
                            NCINo = Applicant.SSNO,

                            IDNo = Applicant.IDNO,
                            Phone = Applicant.PHONE,
                            Address = Applicant.ADDRESS,
                            NSName = Nursinghome.NSNAME,
                            McTypeCode = Applicant.MCTYPE,
                            CareTypeCode = Apphosp.CARETYPE,
                            dHospInDate = m.HOSPENTRYDATE,
                            dHospOutDate = m.HOSPDISCHARGEDATE,
                            HospDay = m.HOSPDAY,
                            DiseaseCode = Applicant.DISEASE,
                            TotalAmount = m.TOTALAMOUNT,
                            NCILevel = m.NCIPAYLEVEL,
                            NCIScale = m.NCIPAYSCALE,
                            YearMonth = s.YEARMONTH,
                            IsDelete = m.ISDELETE,
                        };
                q = q.Where(m => m.IsDelete == false);
                q = q.Where(m => m.YearMonth.CompareTo(startYearMon) >= 0 && m.YearMonth.CompareTo(endYearMon) <= 0);
                feeDataDetail = q.ToList();


                feeDataDetail.ForEach(p =>
                {
                    var mcTypeCode = p.McTypeCode.ToString();
                    var mcTypeModel = unitOfWork.GetRepository<NCI_CODEDTL>().dbSet.Where(m => m.ITEMTYPE == "A002" && m.ITEMCODE == mcTypeCode).FirstOrDefault();
                    if (mcTypeModel != null) p.McType = mcTypeModel.ITEMNAME;
                    var DiseaseModle = unitOfWork.GetRepository<NCI_CODEDTL>().dbSet.Where(m => m.ITEMTYPE == "A003" && m.ITEMCODE == p.DiseaseCode).FirstOrDefault();
                    if (DiseaseModle != null) p.Disease = DiseaseModle.ITEMNAME;
                    p.CareType = ConvertCareType(p.CareTypeCode);
                    var drugFeeList = unitOfWork.GetRepository<NCIP_RESIDENTMONFEEDTL>().dbSet.Where(m => m.RSMONFEEID == p.RsmonfeeId && m.FEETYPE == "药品").ToList();
                    if (drugFeeList != null)
                    {
                        p.DrugFee = drugFeeList.Sum(m => m.AMOUNT);          //算出住民每个月用药的费用
                    }
                    var NCIdrugFeeList = unitOfWork.GetRepository<NCIP_RESIDENTMONFEEDTL>().dbSet.Where(m => m.RSMONFEEID == p.RsmonfeeId && m.FEETYPE == "药品" && m.MCCODE != "无" && m.MCCODE != "").ToList();
                    if (NCIdrugFeeList != null)
                    {
                        p.NCIDrugFee = NCIdrugFeeList.Sum(m => m.AMOUNT);          //算出住民每个月用药可报销的费用
                    }

                });

                var distinctIdNoList = feeDataDetail.GroupBy(x => new
                {
                    x.IDNo,
                    x.CareType
                }).Select(g => g.First()).ToList();

                var index = 1;
                distinctIdNoList.ForEach(p =>
                {
                    var feeDetailModel = new FeeDetail();
                    var feeGroup = feeDataDetail.Where(m => m.IDNo == p.IDNo && m.CareType == p.CareType);
                    feeDetailModel.Name = feeGroup.FirstOrDefault().Name;
                    feeDetailModel.Sex = feeGroup.FirstOrDefault().Sex;
                    feeDetailModel.Age = feeGroup.FirstOrDefault().Age;
                    feeDetailModel.NCINo = feeGroup.FirstOrDefault().NCINo;
                    feeDetailModel.IDNo = feeGroup.FirstOrDefault().IDNo;
                    feeDetailModel.Phone = feeGroup.FirstOrDefault().Phone;
                    feeDetailModel.Address = feeGroup.FirstOrDefault().Address;
                    feeDetailModel.NSName = feeGroup.FirstOrDefault().NSName;
                    feeDetailModel.McType = feeGroup.FirstOrDefault().McType;
                    feeDetailModel.CareType = feeGroup.FirstOrDefault().CareType;
                    feeDetailModel.dHospInDate = feeGroup.Min(m => m.dHospInDate);
                    feeDetailModel.dHospOutDate = feeGroup.Max(m => m.dHospOutDate);
                    if (feeDetailModel.dHospInDate.HasValue)
                    {
                        feeDetailModel.HospInDate = feeDetailModel.dHospInDate.Value.ToString("yyyy-MM-dd");
                    }
                    if (feeDetailModel.dHospOutDate.HasValue)
                    {
                        feeDetailModel.HospOutDate = feeDetailModel.dHospOutDate.Value.ToString("yyyy-MM-dd");
                    }
                    feeDetailModel.HospDay = feeGroup.Sum(m => m.HospDay);
                    feeDetailModel.Disease = feeGroup.FirstOrDefault().Disease;
                    feeDetailModel.DrugFee = feeGroup.Sum(m => m.DrugFee);           //算出住民几个月用药的费用加总
                    feeDetailModel.NCIDrugFee = feeGroup.Sum(m => m.NCIDrugFee);     //算出住民几个月用药可报销的费用加总
                    feeDetailModel.TotalAmount = feeGroup.Sum(m => m.TotalAmount);   //算出住民几个月费用加总
                    feeDetailModel.NCILevel = feeGroup.FirstOrDefault().NCILevel;
                    feeDetailModel.NCIScale = feeGroup.FirstOrDefault().NCIScale;
                    feeDetailModel.Index = index++;
                    response.DataDetail.Add(feeDetailModel);
                });
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public FeeReportComData GetFeeByRegionReport(DateTime? starTime, DateTime? endTime)
        {
            var startYearMon = "";
            var endYearMon = "";
            if (starTime.HasValue)
            {
                startYearMon = starTime.Value.ToString("yyyy-MM");
            }
            if (starTime.HasValue)
            {
                endYearMon = endTime.Value.ToString("yyyy-MM");
            }
            var reportData = new FeeReportComData();
            List<NCIP_NSMONFEE> totalNsMonFee, curNsMonFee;
            List<NCIP_RESIDENTMONFEE> totalResMonFee, curResMonFee;
            GetFreeReportComSourceData(starTime, endTime, out totalNsMonFee, out totalResMonFee, out curNsMonFee, out curResMonFee);

            reportData.StartDate = startYearMon;
            reportData.EndDate = endYearMon;
            reportData.SumCurrent = FillFeeReportComItem(curResMonFee);
            reportData.SumTotal = FillFeeReportComItem(totalResMonFee);

            var totalDetail = GetFeeByOrgReportDetail(totalResMonFee, false);
            var curDetail = GetFeeByOrgReportDetail(curResMonFee, true);

            var q = from c in curDetail
                    from t in totalDetail
                    where c.GroupColumn1 == t.GroupColumn1 && c.GroupColumn2 == t.GroupColumn2
                    select new FeeReportComDetail()
                    {
                        GroupColumn1 = c.GroupColumn1,
                        GroupColumn2 = c.GroupColumn2,
                        Current = c.Current,
                        Total = t.Total
                    };
            var orgFeeDetialList = q.ToList();
            //目前所有机构都属于统筹区内：
            var regionFeeDetialList = new List<FeeReportComDetail>();
            var regionDetial = new FeeReportComDetail()
                              {
                                  GroupColumn1 = "统筹区内",
                                  GroupColumn2 = "",
                                  Current = new FeeReportComItem
                                  {
                                      ResNum = orgFeeDetialList.Sum(m => m.Current.ResNum),
                                      HospDay = orgFeeDetialList.Sum(m => m.Current.HospDay),
                                      Fee = orgFeeDetialList.Sum(m => m.Current.Fee),
                                      MedFee = orgFeeDetialList.Sum(m => m.Current.MedFee),
                                      NciPay = orgFeeDetialList.Sum(m => m.Current.NciPay),
                                      SvcDep = orgFeeDetialList.Sum(m => m.Current.SvcDep),
                                      Decut = orgFeeDetialList.Sum(m => m.Current.Decut),
                                      AccNciPay = orgFeeDetialList.Sum(m => m.Current.AccNciPay),
                                  },
                                  Total = new FeeReportComItem
                                  {
                                      ResNum = orgFeeDetialList.Sum(m => m.Total.ResNum),
                                      HospDay = orgFeeDetialList.Sum(m => m.Total.HospDay),
                                      Fee = orgFeeDetialList.Sum(m => m.Total.Fee),
                                      MedFee = orgFeeDetialList.Sum(m => m.Total.MedFee),
                                      NciPay = orgFeeDetialList.Sum(m => m.Total.NciPay),
                                      SvcDep = orgFeeDetialList.Sum(m => m.Total.SvcDep),
                                      Decut = orgFeeDetialList.Sum(m => m.Total.Decut),
                                      AccNciPay = orgFeeDetialList.Sum(m => m.Total.AccNciPay),

                                  }
                              };
            regionFeeDetialList.Add(regionDetial);
            reportData.DataDetail = regionFeeDetialList;
            var index = 1;
            reportData.DataDetail.ForEach(p => p.Index = index++);
            return reportData;
        }


        public FeeByCareTypeMain GetFeeByCareTypeMainReport(DateTime? starTime, DateTime? endTime)
        {
            var startYearMon = "";
            var endYearMon = "";
            var ResponseData = new FeeByCareTypeMain();
            if (starTime.HasValue)
            {
                startYearMon = starTime.Value.ToString("yyyy-MM");
                ResponseData.StartDate = starTime.Value.ToString("yyyy年MM月dd日");
            }
            if (starTime.HasValue)
            {
                endYearMon = endTime.Value.ToString("yyyy-MM");
                ResponseData.EndDate = endTime.Value.ToString("yyyy年MM月dd日");
            }
            var reportData = new FeeReportComData();
            List<NCIP_NSMONFEE> totalNsMonFee, curNsMonFee;
            List<NCIP_RESIDENTMONFEE> totalResMonFee, curResMonFee;
            GetFreeReportComSourceData(starTime, endTime, out totalNsMonFee, out totalResMonFee, out curNsMonFee, out curResMonFee);
            var curDetail = GetFeeByCareTypeDetailData(curResMonFee);

            var distinctIdNoList = curDetail.GroupBy(x => new
            {
                x.OrgName,
            }).Select(g => g.First()).ToList();

            var index = 1;
            ResponseData.Detail = distinctIdNoList.Select(p =>
            {
                var feeByCareTypeDetail = new FeeByCareTypeDetail();
                feeByCareTypeDetail.Index = index;
                index++;
                feeByCareTypeDetail.OrgName = p.OrgName;
                feeByCareTypeDetail.SpecCareResNum = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 1).Sum(m => m.ResNum);
                feeByCareTypeDetail.SpecCareHospDay = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 1).Sum(m => m.HospDay);
                feeByCareTypeDetail.SpecCareFee = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 1).Sum(m => m.Fee);
                feeByCareTypeDetail.SpecCareNciPay = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 1).Sum(m => m.NciPay);
                feeByCareTypeDetail.OrgCareResNum = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 2).Sum(m => m.ResNum);
                feeByCareTypeDetail.OrgCareHospDay = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 2).Sum(m => m.HospDay);
                feeByCareTypeDetail.OrgCareFee = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 2).Sum(m => m.Fee);
                feeByCareTypeDetail.OrgCareNciPay = curDetail.Where(m => m.OrgName == p.OrgName && m.CareType == 2).Sum(m => m.NciPay);
                feeByCareTypeDetail.ResNum = feeByCareTypeDetail.SpecCareResNum + feeByCareTypeDetail.OrgCareResNum;
                feeByCareTypeDetail.Fee = feeByCareTypeDetail.SpecCareFee + feeByCareTypeDetail.OrgCareFee;
                feeByCareTypeDetail.NciPay = feeByCareTypeDetail.SpecCareNciPay + feeByCareTypeDetail.OrgCareNciPay;
                return feeByCareTypeDetail;
            }).ToList();
            ResponseData.SumTotal = new FeeByCareTypeDetail()
            {
                SpecCareResNum = ResponseData.Detail.Sum(m => m.SpecCareResNum),
                SpecCareHospDay = ResponseData.Detail.Sum(m => m.SpecCareHospDay),
                SpecCareFee = ResponseData.Detail.Sum(m => m.SpecCareFee),
                SpecCareNciPay = ResponseData.Detail.Sum(m => m.SpecCareNciPay),
                OrgCareResNum = ResponseData.Detail.Sum(m => m.OrgCareResNum),
                OrgCareHospDay = ResponseData.Detail.Sum(m => m.OrgCareHospDay),
                OrgCareFee = ResponseData.Detail.Sum(m => m.OrgCareFee),
                OrgCareNciPay = ResponseData.Detail.Sum(m => m.OrgCareNciPay),
                ResNum = ResponseData.Detail.Sum(m => m.ResNum),
                Fee = ResponseData.Detail.Sum(m => m.Fee),
                NciPay = ResponseData.Detail.Sum(m => m.NciPay)
            };
            return ResponseData;
        }
        private List<TempFeeByCareTypeDetail> GetFeeByCareTypeDetailData(IEnumerable<NCIP_RESIDENTMONFEE> resData)
        {
            var q = from r in resData
                    join m in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.ISDELETE != true) on r.RESIDENTSSID equals m.IDNO
                    join a in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(m => m.ISDELETE != true && m.STATUS == 6) on m.APPLICANTID equals a.APPLICANTID
                    join n in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(m => m.ISDELETE != true) on r.NSID equals n.NSID
                    group r by new { n.NSNAME, a.AGENCYAPPROVEDCARETYPE } into grp
                    select new
                    {
                        OrgName = grp.Key.NSNAME,
                        ResCareType = grp.Key.AGENCYAPPROVEDCARETYPE,
                        ReMonFeeList = (from v in grp select v).ToList()
                    };
            var resExtendData = q.ToList();
            var genDetail = resExtendData.Select(x =>
            {
                var detail = new TempFeeByCareTypeDetail();
                detail.OrgName = x.OrgName;
                detail.CareType = x.ResCareType;
                var currentDetail = FillFeeReportComItem(x.ReMonFeeList);
                detail.ResNum = currentDetail.ResNum;
                detail.HospDay = currentDetail.HospDay;
                detail.Fee = currentDetail.Fee;
                detail.NciPay = currentDetail.NciPay;
                return detail;
            }).ToList();

            return genDetail;
        }
        public List<PrintMonthFee> GetPrintData(BaseResponse<IList<TreatmentAccount>> res, List<Ipdregout> ipdList)
        {
            var num = 1;
            var q = from s in res.Data
                    join i in ipdList on s.FeeNo equals i.FeeNo
                    select new PrintMonthFee
                    {
                        Index = num++,
                        InDate = i.InDate,
                        OutDate = i.OutDate,
                        Name = s.Name,
                        NCIPay = s.Ncipay,
                        NCIPayLevel = s.Ncipaylevel,
                        HospDay = s.Hospday,
                        TotalAmount = s.Totalamount,
                        ResidentSSId = s.Residentssid,
                        Sex = s.Gender,
                        CareTypeId = s.NsappcareTypeName,
                        EvaluationTime = s.EvaluationTime,
                        BrithPlace = s.Residence,
                        RsStatus = s.McTypeName,
                        DiseaseDiag = s.Disease,
                        ApplyHosTime = s.CertStartTime,
                        yearMonthArr = s.yearMonthArr,
                    };
            var list = q.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var yearList = list[i].yearMonthArr.Split(',');
                var yearMin = DateTime.Parse(yearList.Min() + "-01");
                var yearMax = DateTime.Parse(yearList.Max() + "-01");
                var startDate = DictHelper.GetFeeIntervalStartDateByDate(yearMin);
                var EndDate = DictHelper.GetFeeIntervalEndDateByDate(yearMax);
                list[i].InDate = DateTime.Compare(startDate, list[i].InDate.Value) >= 0 ? startDate : list[i].InDate.Value;
                list[i].OutDate = list[i].OutDate.HasValue ? (DateTime.Compare(EndDate, list[i].OutDate.Value) <= 0 ? EndDate : list[i].OutDate.Value) : EndDate;
            }
            return list;
        }

    }
}