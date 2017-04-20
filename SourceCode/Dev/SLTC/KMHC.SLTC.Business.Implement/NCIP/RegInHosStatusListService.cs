using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.NCIP;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.NCIP
{
    public class RegInHosStatusListService : BaseService, IRegInHosStatusListService
    {

        public BaseResponse<RegInHosStatusDtlData> QueryRegInHosStatustList(string name, string idno, string nsid, string status, BaseResponse<IList<RegInHosStatusListEntity>> resultContent, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<RegInHosStatusDtlData>();
            response.Data = new RegInHosStatusDtlData();
            var q = from acert in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.STATUS == 6)
                    join applicant in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on acert.APPLICANTID equals applicant.APPLICANTID
                    //where applicant.LASTCERTRESULT == 6
                    //join res in resultContent.Data on acert.IDNO equals res.IdNo into regInHos
                    //from reg in regInHos.DefaultIfEmpty()
                    select new RegInHosStatusDtlEntity
                    {
                        Name = applicant.NAME,
                        Gender = applicant.GENDER,
                        Birthday = applicant.BIRTHDATE,
                        Age = acert.AGE,
                        IdNo = applicant.IDNO,
                        SsNo = applicant.SSNO,
                        Phone = applicant.PHONE,
                        McType = applicant.MCTYPE,
                        Disease = applicant.DISEASE,
                        MaritalStatus = applicant.MARITALSTATUS,
                        FamilyMemberName = applicant.FAMILYMEMBERNAME,
                        FamilyMemberRelationship = applicant.FAMILYMEMBERRELATIONSHIP,
                        FamilyMemberPhone = applicant.FAMILYMEMBERPHONE,
                        //InHosStatus 
                        NsId = applicant.NSID,
                        NsAppCareType = acert.NSAPPCARETYPE,
                        //IpdFlag = reg.IpdFlag,
                        //InDate = reg.InDate,
                        //OutDate = reg.OutDate,
                        //StartDate = reg.StartDate,
                        //ReturnDate = reg.ReturnDate,
                        //LeHour = reg.LeHour
                    };
            
            q = q.OrderByDescending(m => m.Name);
            response.Data.RegInHosStatusDtl = new List<RegInHosStatusDtlEntity>();
            var list = q.ToList();
            if (list != null)
            {
                foreach (var item in list)
                {
                    RegInHosStatusDtlEntity newItem = Mapper.DynamicMap<RegInHosStatusDtlEntity>(item);
                    var selectItem = resultContent.Data.Where(m => m.IdNo == newItem.IdNo).FirstOrDefault();
                    if (selectItem != null)
                    {
                        newItem.IpdFlag = selectItem.IpdFlag;
                        if (selectItem.InDate != null)
                        {
                            newItem.InDate = selectItem.InDate;
                        }
                        if (selectItem.OutDate != null)
                        {
                            newItem.OutDate = selectItem.OutDate;
                        }
                        if (selectItem.StartDate != null)
                        {
                            newItem.StartDate = selectItem.StartDate;
                        }
                        if (selectItem.ReturnDate != null)
                        {
                            newItem.ReturnDate = selectItem.ReturnDate;
                        }
                        if (selectItem.LeHour != null)
                        {
                            newItem.LeHour = selectItem.LeHour;
                        }
                    }
                    if (newItem.IpdFlag == "O")
                    {
                        newItem.InHosStatus = 3;
                    }
                    if (newItem.IpdFlag == "I")
                    {
                        if (newItem.StartDate <= DateTime.Now && newItem.ReturnDate >= DateTime.Now)
                        {
                            newItem.InHosStatus = 2;
                        }
                        if ((newItem.StartDate == null && newItem.ReturnDate == null) || newItem.ReturnDate < DateTime.Now)
                        {
                            newItem.InHosStatus = 0;
                        }
                    }
                    if (newItem.IpdFlag == null)
                    {
                        newItem.InHosStatus = 1;
                    }
                    response.Data.RegInHosStatusDtl.Add(newItem);

                };

            }
            response.Data.InHosCount = response.Data.RegInHosStatusDtl.Where(m => m.InHosStatus == 0).ToList().Count();
            response.Data.OutHosCount = response.Data.RegInHosStatusDtl.Where(m => m.InHosStatus != 0).ToList().Count();



            if (Convert.ToInt32(nsid) != -1)
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.NsId == nsid).ToList();
            }
            if (name != null && name != "")
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.Name.Contains(name)).ToList();
            }
            if (idno != null && idno != "")
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.IdNo.Contains(idno)).ToList();
            }
            if (Convert.ToInt32(status) != -1)
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.InHosStatus == Convert.ToInt32(status)).ToList();
            }
            response.RecordsCount = response.Data.RegInHosStatusDtl.ToList().Count();
            #region 注释掉用委托赋值
            //Action<IList> mapperResponse = (IList list) =>
            //{
            //    foreach (dynamic item in list)
            //    {
            //        RegInHosStatusDtlEntity newItem = Mapper.DynamicMap<RegInHosStatusDtlEntity>(item);
            //        var selectItem = resultContent.Data.Where(m => m.IdNo == newItem.IdNo).FirstOrDefault();
            //        if (selectItem != null)
            //        {
            //            newItem.IpdFlag = selectItem.IpdFlag;
            //            if (selectItem.InDate != null)
            //            {
            //                newItem.InDate = selectItem.InDate;
            //            }
            //            if (selectItem.OutDate != null)
            //            {
            //                newItem.OutDate = selectItem.OutDate;
            //            }
            //            if (selectItem.StartDate != null)
            //            {
            //                newItem.StartDate = selectItem.StartDate;
            //            }

            //            if (selectItem.ReturnDate != null)
            //            {
            //                newItem.ReturnDate = selectItem.ReturnDate;
            //            }

            //            if (selectItem.LeHour != null)
            //            {
            //                newItem.LeHour = selectItem.LeHour;
            //            }

            //        }
            //        if (newItem.IpdFlag == "O")
            //        {
            //            newItem.InHosStatus = 1;
            //        }
            //        if (newItem.IpdFlag == "I")
            //        {
            //            if (newItem.StartDate <= DateTime.Now && newItem.ReturnDate >= DateTime.Now)
            //            {
            //                newItem.InHosStatus = 1;
            //            }
            //            if (newItem.StartDate == null && newItem.ReturnDate == null)
            //            {
            //                newItem.InHosStatus = 0;
            //            }
            //        }
            //        if (newItem.IpdFlag == null)
            //        {
            //            newItem.InHosStatus = 1;
            //        }

            //        if (Convert.ToInt32(status) != -1)
            //        {
            //            if (Convert.ToInt32(status) != newItem.InHosStatus)
            //            {
            //                continue;
            //            }
            //        }
            //        response.Data.RegInHosStatusDtl.Add(newItem);
            //    }
            //};
            #endregion

            if (PageSize > 0)
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
                //mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<RegInHosStatusDtlData> QueryRegInHosStatustList(string name, string idno, string nsid, string status, BaseResponse<IList<RegInHosStatusListEntity>> resultContent)
        {
            var response = new BaseResponse<RegInHosStatusDtlData>();
            response.Data = new RegInHosStatusDtlData();
            var q = from acert in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.STATUS == 6)
                    join applicant in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on acert.APPLICANTID equals applicant.APPLICANTID
                    //where applicant.LASTCERTRESULT == 6
                    //join res in resultContent.Data on acert.IDNO equals res.IdNo into regInHos
                    //from reg in regInHos.DefaultIfEmpty()
                    select new RegInHosStatusDtlEntity
                    {
                        Name = applicant.NAME,
                        Gender = applicant.GENDER,
                        Birthday = applicant.BIRTHDATE,
                        Age = acert.AGE,
                        IdNo = applicant.IDNO,
                        SsNo = applicant.SSNO,
                        Phone = applicant.PHONE,
                        McType = applicant.MCTYPE,
                        Disease = applicant.DISEASE,
                        MaritalStatus = applicant.MARITALSTATUS,
                        FamilyMemberName = applicant.FAMILYMEMBERNAME,
                        FamilyMemberRelationship = applicant.FAMILYMEMBERRELATIONSHIP,
                        FamilyMemberPhone = applicant.FAMILYMEMBERPHONE,
                        //InHosStatus 
                        NsId = applicant.NSID,
                        NsAppCareType = acert.NSAPPCARETYPE,
                        //IpdFlag = reg.IpdFlag,
                        //InDate = reg.InDate,
                        //OutDate = reg.OutDate,
                        //StartDate = reg.StartDate,
                        //ReturnDate = reg.ReturnDate,
                        //LeHour = reg.LeHour
                    };

            q = q.OrderByDescending(m => m.Name);
            response.Data.RegInHosStatusDtl = new List<RegInHosStatusDtlEntity>();
            var list = q.ToList();
            if (list != null)
            {
                foreach (var item in list)
                {
                    RegInHosStatusDtlEntity newItem = Mapper.DynamicMap<RegInHosStatusDtlEntity>(item);
                    var selectItem = resultContent.Data.Where(m => m.IdNo == newItem.IdNo).FirstOrDefault();
                    if (selectItem != null)
                    {
                        newItem.IpdFlag = selectItem.IpdFlag;
                        if (selectItem.InDate != null)
                        {
                            newItem.InDate = selectItem.InDate;
                        }
                        if (selectItem.OutDate != null)
                        {
                            newItem.OutDate = selectItem.OutDate;
                        }
                        if (selectItem.StartDate != null)
                        {
                            newItem.StartDate = selectItem.StartDate;
                        }
                        if (selectItem.ReturnDate != null)
                        {
                            newItem.ReturnDate = selectItem.ReturnDate;
                        }
                        if (selectItem.LeHour != null)
                        {
                            newItem.LeHour = selectItem.LeHour;
                        }
                    }
                    if (newItem.IpdFlag == "O")
                    {
                        newItem.InHosStatus = 3;
                    }
                    if (newItem.IpdFlag == "I")
                    {
                        if (newItem.StartDate <= DateTime.Now && newItem.ReturnDate >= DateTime.Now)
                        {
                            newItem.InHosStatus = 2;
                        }
                        if ((newItem.StartDate == null && newItem.ReturnDate == null) || newItem.ReturnDate < DateTime.Now)
                        {
                            newItem.InHosStatus = 0;
                        }
                    }
                    if (newItem.IpdFlag == null)
                    {
                        newItem.InHosStatus = 1;
                    }
                    response.Data.RegInHosStatusDtl.Add(newItem);

                };

            }
            response.Data.InHosCount = response.Data.RegInHosStatusDtl.Where(m => m.InHosStatus == 0).ToList().Count();
            response.Data.OutHosCount = response.Data.RegInHosStatusDtl.Where(m => m.InHosStatus != 0).ToList().Count();



            if (Convert.ToInt32(nsid) != -1)
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.NsId == nsid).ToList();
            }
            if (name != null && name != "")
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.Name.Contains(name)).ToList();
            }
            if (idno != null && idno != "")
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.IdNo.Contains(idno)).ToList();
            }
            if (Convert.ToInt32(status) != -1)
            {
                response.Data.RegInHosStatusDtl = response.Data.RegInHosStatusDtl.Where(m => m.InHosStatus == Convert.ToInt32(status)).ToList();
            }
            response.RecordsCount = response.Data.RegInHosStatusDtl.ToList().Count();

            
            return response;
        }


    }
}
