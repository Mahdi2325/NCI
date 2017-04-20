using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.DC.Report;
using System;
using KM.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Implement;
using KMHC.SLTC.Business.Interface;
using System.Data;
using KMHC.SLTC.Business.Entity;

namespace KMHC.SLTC.WebController
{
    public class NCI_ReportController : ReportBaseController
    {
       

        public ActionResult NCI_ReportPreview()
        {
            string feeNo = Request["feeNo"];
            string id = Request["id"];
            string templateName = Request["templateName"];
            ReportRequest request = new ReportRequest();
            request.id =Convert.ToInt32( id);

            if (templateName != null)
            {
                switch (templateName)
                {   
                    //住民申请-资格申请表
                    case "ApplyBILL":
                        
                         this.GeneratePDF("P31Apply", this.printApplyBILL, request);
                        break;
                    //住民申请-评定量表
                    case "ADLBILL":
                        this.GeneratePDF("p32adl", this.printADLBILL, request);
                        break;
                    //住民申请-住院审批表
                    case "AuditingBILL":
                        this.GeneratePDF("p33Audit", this.printAuditingBILL, request);
                        break;
                }
            }


            return View("Preview");
        }

        string ConvertmuchDisease(string disease, string TYPECODE)
        {
            var disCodeDtl = string.Empty;
            IDictManageService _dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();

            String[] dislist = disease.Split(',');
            foreach (var code in dislist)
            {
                disCodeDtl += _dictManageService.GetCode(code, TYPECODE).Data.ItemName + "  ";
            }

            return disCodeDtl;
        }


        string ConvertDisease(string disease,string TYPECODE)
        {
            IDictManageService _dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            Dictionary<string,string> _diseaseCodeCache = new Dictionary<string, string>();
            if (_diseaseCodeCache.ContainsKey(disease))
            {
                return _diseaseCodeCache[disease];
            }

            var disCodeDtl = _dictManageService.GetCode(disease, TYPECODE);
            if (disCodeDtl == null || disCodeDtl.Data == null)
            {
                return "未知";
            }
            _diseaseCodeCache.Add(disease, disCodeDtl.Data.ItemName);

            return disCodeDtl.Data.ItemName;
        }

        private void printApplyBILL(WordDocument doc, ReportRequest request)
        {

            

            IAuditAppcertService service = IOCContainer.Instance.Resolve<IAuditAppcertService>();
            string ls_type = string.Empty;
            var tempresponse = new BaseResponse<AppcertEntity>();
            tempresponse = service.QueryAppcert(request.id);

            var response = new BaseResponse<List<object>>(new List<object>());
            if (tempresponse.Data.NsappcareType==(int)CareType.机构护理)
            { ls_type = CareType.机构护理.ToString(); }
            else if (tempresponse.Data.NsappcareType == (int)CareType.专护)
            { ls_type = CareType.专护.ToString(); }
            string ls_McType =ConvertDisease(Convert.ToString(tempresponse.Data.McType),"A002");
            string LS_Disease = ConvertmuchDisease(Convert.ToString(tempresponse.Data.Disease), "A003");
            response.Data.Add
               (new
               {
                    Name =tempresponse.Data.Name,// "王大明",
                    Age1 = tempresponse.Data.Age,//"87",//年龄
                    SsNo = tempresponse.Data.SsNo,//社会保障卡号
                    Gender =tempresponse.Data.Gender,// "男",//
                    McType = ls_McType,//tempresponse.Data.McType,  // 人员身份 :□在职     □ 退休    □城乡居民,
                    Address = tempresponse.Data.Address,// 
                    Phone = tempresponse.Data.Phone,//
                    AppReason = tempresponse.Data.AppReason,//申请原因
                    NsComment = tempresponse.Data.NsComment,//定点服务机构意见
                    IcComment = tempresponse.Data.IcComment,//承办保险机构意见 
                    AgencyComment = tempresponse.Data.AgencyComment,//护理保险经办机构意见：

                    type = ls_type,//类别  □专护  机构护理  
                    Disease = LS_Disease,//tempresponse.Data.Disease,//病种
               });
            BindData(response.Data[0], doc);
        }
        private void printADLBILL(WordDocument doc, ReportRequest request)
        {
            IAuditAppcertService service = IOCContainer.Instance.Resolve<IAuditAppcertService>();//经办机构 
            //经办机构
            var request2 = new BaseRequest<AgencyAsstRecordDataFilter>()
            {
                Data = new AgencyAsstRecordDataFilter
                {
                    AppcertId = request.id  ,
                }
            };

            //定点机构
            IAppcertService service2 = IOCContainer.Instance.Resolve<IAppcertService>();//定点机构
            var request3 = new BaseRequest<NursingHomeAsstRecordDataFilter>()
            {
                Data = new NursingHomeAsstRecordDataFilter
                {
                    AppcertId = request.id,
                }
            };

            //定点
            string lsMakerId310 = ""; 
            string lsMakerId315=""; 
            string lsMakerId3110="";  
            string lsMakerId350="";  
            string lsMakerId355=""; 
            string lsMakerId330="";  
            string lsMakerId335=""; 
            string lsMakerId380=""; 
            string lsMakerId385=""; 
            string lsMakerId3810=""; 
            string lsMakerId390=""; 
            string lsMakerId395=""; 
            string lsMakerId3910=""; 
            string lsMakerId400=""; 
            string lsMakerId405=""; 
            string lsMakerId4010=""; 
            string lsMakerId340=""; 
            string lsMakerId345=""; 
            string lsMakerId3410=""; 
            string lsMakerId320=""; 
            string lsMakerId325=""; 
            string lsMakerId3210=""; 
            string lsMakerId3215=""; 
            string lsMakerId360=""; 
            string lsMakerId365=""; 
            string lsMakerId3610=""; 
            string lsMakerId3615=""; 
            string lsMakerId370=""; 
            string lsMakerId375=""; 
            string lsMakerId3710="";

            //经办机构
            string lsOrgid310 = "";
            string lsOrgid315 = "";
            string lsOrgid3110 = "";
            string lsOrgid350 = "";
            string lsOrgid355 = "";
            string lsOrgid330 = "";
            string lsOrgid335 = "";
            string lsOrgid380 = "";
            string lsOrgid385 = "";
            string lsOrgid3810 = "";
            string lsOrgid390 = "";
            string lsOrgid395 = "";
            string lsOrgid3910 = "";
            string lsOrgid400 = "";
            string lsOrgid405 = "";
            string lsOrgid4010 = "";
            string lsOrgid340 = "";
            string lsOrgid345 = "";
            string lsOrgid3410 = "";
            string lsOrgid320 = "";
            string lsOrgid325 = "";
            string lsOrgid3210 = "";
            string lsOrgid3215 = "";
            string lsOrgid360 = "";
            string lsOrgid365 = "";
            string lsOrgid3610 = "";
            string lsOrgid3615 = "";
            string lsOrgid370 = "";
            string lsOrgid375 = "";
            string lsOrgid3710 = ""; 

            var responsecode = service.GetQue("ADL");
            var responsedata = service.GetAdlRec(request2);//经办机构评估数据

            var responsedata2 = service2.GetAdlRec(request3);//定点机构数据
            decimal lssumval = 0;//定点机构评估分数合计
            decimal lsOrgidsum = 0;//经办机构合计
            var response = new BaseResponse<List<object>>(new List<object>());

            string tempstr;
            lssumval = 0;//定点机构评估分数合计
             lsOrgidsum = 0;//经办机构合计
            //定点机构统计
            for (int i = 0; i < responsedata2.Data.NursingHomeAsstRecordDetail.Count; i++)
            { 
                for (int j = 0; j < responsecode.Data.MakerItem.Count; j++)
                {

                    if (responsedata2.Data.NursingHomeAsstRecordDetail[i].MakerId == responsecode.Data.MakerItem[j].MakerId)
                    {

                        tempstr = "MakerId" + Convert.ToString(responsedata2.Data.NursingHomeAsstRecordDetail[i].MakerId) + Convert.ToString(Convert.ToInt32(responsedata2.Data.NursingHomeAsstRecordDetail[i].MakerValue));
                        if (tempstr == "MakerId310") { lsMakerId310 = "√"; lsMakerId315 = ""; lsMakerId3110 = ""; }
                        if (tempstr == "MakerId315") { lsMakerId310 = ""; lsMakerId315 = "√"; lsMakerId3110 = ""; }
                        if (tempstr == "MakerId3110") { lsMakerId310 = ""; lsMakerId315 = ""; lsMakerId3110 = "√"; }
                        

                        if (tempstr == "MakerId320") { lsMakerId320 = "√"; lsMakerId325 = ""; lsMakerId3210 = ""; lsMakerId3215 = ""; }
                        if (tempstr == "MakerId325") { lsMakerId320 = ""; lsMakerId325 = "√"; lsMakerId3210 = ""; lsMakerId3215 = ""; }
                        if (tempstr == "MakerId3210") { lsMakerId320 = ""; lsMakerId325 = ""; lsMakerId3210 = "√"; lsMakerId3215 = ""; }
                        if (tempstr == "MakerId3215") { lsMakerId320 = ""; lsMakerId325 = ""; lsMakerId3210 = ""; lsMakerId3215 = "√"; }

                        if (tempstr == "MakerId330") { lsMakerId330 = "√"; lsMakerId335 = ""; }
                        if (tempstr == "MakerId335") { lsMakerId330 = ""; lsMakerId335 = "√"; }

                        if (tempstr == "MakerId340") { lsMakerId340 = "√"; lsMakerId345 = ""; lsMakerId3410 = ""; }
                        if (tempstr == "MakerId345") { lsMakerId340 = ""; lsMakerId345 = "√"; lsMakerId3410 = ""; }
                        if (tempstr == "MakerId3410") { lsMakerId340 = ""; lsMakerId345 = ""; lsMakerId3410 = "√"; }

                        if (tempstr == "MakerId350") { lsMakerId350 = "√"; lsMakerId355 = ""; }
                        if (tempstr == "MakerId355") { lsMakerId350 = ""; lsMakerId355 = "√"; }


                        if (tempstr == "MakerId360") { lsMakerId360 = "√"; lsMakerId365 = ""; lsMakerId3610 = ""; lsMakerId3615 = ""; }
                        if (tempstr == "MakerId365") { lsMakerId360 = ""; lsMakerId365 = "√"; lsMakerId3610 = ""; lsMakerId3615 = ""; }
                        if (tempstr == "MakerId3610") { lsMakerId360 = ""; lsMakerId365 = ""; lsMakerId3610 = "√"; lsMakerId3615 = ""; }
                        if (tempstr == "MakerId3615") { lsMakerId360 = ""; lsMakerId365 = ""; lsMakerId3610 = ""; lsMakerId3615 = "√"; }

                        if (tempstr == "MakerId370") { lsMakerId370 = "√"; lsMakerId375 = "";  lsMakerId3710 = ""; }
                        if (tempstr == "MakerId375") { lsMakerId370 = "";  lsMakerId375 = "√"; lsMakerId3710 = ""; }
                        if (tempstr == "MakerId3710") { lsMakerId370 = ""; lsMakerId375 = "";  lsMakerId3710 = "√"; }

                        if (tempstr == "MakerId380")  { lsMakerId380 = "√"; lsMakerId385 = ""; lsMakerId3810 = ""; }
                        if (tempstr == "MakerId385")  { lsMakerId380 = ""; lsMakerId385 = "√"; lsMakerId3810 = ""; }
                        if (tempstr == "MakerId3810") { lsMakerId380 = ""; lsMakerId385 = ""; lsMakerId3810 = "√"; }

                        if (tempstr == "MakerId390") { lsMakerId390 = "√"; lsMakerId395 = ""; lsMakerId3910 = ""; }
                        if (tempstr == "MakerId395") { lsMakerId390 = ""; lsMakerId395 = "√"; lsMakerId3910 = ""; }
                        if (tempstr == "MakerId3910") { lsMakerId390 = ""; lsMakerId395 = ""; lsMakerId3910 = "√"; }

                        if (tempstr == "MakerId400") { lsMakerId400 = "√"; lsMakerId405 = ""; lsMakerId4010 = ""; }
                        if (tempstr == "MakerId405") { lsMakerId400 = ""; lsMakerId405 = "√"; lsMakerId4010 = ""; }
                        if (tempstr == "MakerId4010") { lsMakerId400 = ""; lsMakerId405 = ""; lsMakerId4010 = "√"; }
                         
                         
                            //lssumval = "60";
                        lssumval = lssumval + responsedata2.Data.NursingHomeAsstRecordDetail[i].MakerValue;
                        lssumval = (int)lssumval;
                    }
                }
            }
            if (responsedata.Data.AgencyAsstRecordDetail != null)
            {
                //经办机构统计
                lsOrgidsum = 0;
                for (int i = 0; i < responsedata.Data.AgencyAsstRecordDetail.Count; i++)
                {
                    for (int j = 0; j < responsecode.Data.MakerItem.Count; j++)
                    {

                        if (responsedata.Data.AgencyAsstRecordDetail[i].MakerId == responsecode.Data.MakerItem[j].MakerId)
                        {

                            tempstr = "Orgid" + Convert.ToString(responsedata.Data.AgencyAsstRecordDetail[i].MakerId) + Convert.ToString(Convert.ToInt32(responsedata.Data.AgencyAsstRecordDetail[i].MakerValue));
                            if (tempstr == "Orgid310") { lsOrgid310 = "√"; lsOrgid315 = ""; lsOrgid3110 = ""; }
                            if (tempstr == "Orgid315") { lsOrgid310 = ""; lsOrgid315 = "√"; lsOrgid3110 = ""; }
                            if (tempstr == "Orgid3110") { lsOrgid310 = ""; lsOrgid315 = ""; lsOrgid3110 = "√"; }

                            if (tempstr == "Orgid320") { lsOrgid320 = "√"; lsOrgid325 = ""; lsOrgid3210 = ""; lsOrgid3215 = ""; }
                            if (tempstr == "Orgid325") { lsOrgid320 = ""; lsOrgid325 = "√"; lsOrgid3210 = ""; lsOrgid3215 = ""; }
                            if (tempstr == "Orgid3210") { lsOrgid320 = ""; lsOrgid325 = ""; lsOrgid3210 = "√"; lsOrgid3215 = ""; }
                            if (tempstr == "Orgid3215") { lsOrgid320 = ""; lsOrgid325 = ""; lsOrgid3210 = ""; lsOrgid3215 = "√"; }

                            if (tempstr == "Orgid330") { lsOrgid330 = "√"; lsOrgid335 = ""; }
                            if (tempstr == "Orgid335") { lsOrgid330 = ""; lsOrgid335 = "√"; }

                            if (tempstr == "Orgid340") { lsOrgid340 = "√"; lsMakerId345 = ""; lsMakerId3410 = ""; }
                            if (tempstr == "Orgid345") { lsOrgid340 = ""; lsOrgid345 = "√"; lsOrgid3410 = ""; }
                            if (tempstr == "Orgid3410") { lsOrgid340 = ""; lsOrgid345 = ""; lsOrgid3410 = "√"; }

                            if (tempstr == "Orgid350") { lsOrgid350 = "√"; lsOrgid355 = ""; }
                            if (tempstr == "Orgid355") { lsOrgid350 = ""; lsOrgid355 = "√"; }


                            if (tempstr == "Orgid360") { lsOrgid360 = "√"; lsOrgid365 = ""; lsOrgid3610 = ""; lsOrgid3615 = ""; }
                            if (tempstr == "Orgid365") { lsOrgid360 = ""; lsOrgid365 = "√"; lsOrgid3610 = ""; lsOrgid3615 = ""; }
                            if (tempstr == "Orgid3610") { lsOrgid360 = ""; lsOrgid365 = ""; lsOrgid3610 = "√"; lsOrgid3615 = ""; }
                            if (tempstr == "Orgid3615") { lsOrgid360 = ""; lsOrgid365 = ""; lsOrgid3610 = ""; lsOrgid3615 = "√"; }

                            if (tempstr == "Orgid370") { lsOrgid370 = "√"; lsMakerId375 = ""; lsMakerId3710 = ""; }
                            if (tempstr == "Orgid375") { lsOrgid370 = ""; lsOrgid375 = "√"; lsOrgid3710 = ""; }
                            if (tempstr == "Orgid3710") { lsOrgid370 = ""; lsOrgid375 = ""; lsOrgid3710 = "√"; }

                            if (tempstr == "Orgid380") { lsOrgid380 = "√"; lsOrgid385 = ""; lsOrgid3810 = ""; }
                            if (tempstr == "Orgid385") { lsOrgid380 = ""; lsOrgid385 = "√"; lsOrgid3810 = ""; }
                            if (tempstr == "Orgid3810") { lsOrgid380 = ""; lsOrgid385 = ""; lsOrgid3810 = "√"; }

                            if (tempstr == "Orgid390") { lsOrgid390 = "√"; lsOrgid395 = ""; lsOrgid3910 = ""; }
                            if (tempstr == "Orgid395") { lsOrgid390 = ""; lsOrgid395 = "√"; lsOrgid3910 = ""; }
                            if (tempstr == "Orgid3910") { lsOrgid390 = ""; lsOrgid395 = ""; lsOrgid3910 = "√"; }

                            if (tempstr == "Orgid400") { lsOrgid400 = "√"; lsOrgid405 = ""; lsOrgid4010 = ""; }
                            if (tempstr == "Orgid405") { lsOrgid400 = ""; lsOrgid405 = "√"; lsOrgid4010 = ""; }
                            if (tempstr == "Orgid4010") { lsOrgid400 = ""; lsOrgid405 = ""; lsOrgid4010 = "√"; }

                            lsOrgidsum = lsOrgidsum + responsedata.Data.AgencyAsstRecordDetail[i].MakerValue;
                            lsOrgidsum = (int)lsOrgidsum;

                        }
                    }
                }
            }
           

            IAuditAppcertService servicejg = IOCContainer.Instance.Resolve<IAuditAppcertService>(); 
            var tempresponse = new BaseResponse<AppcertEntity>();
            tempresponse = servicejg.QueryAppcert(request.id);

            string ls_McType = ConvertDisease(Convert.ToString(tempresponse.Data.McType), "A002");
            string LS_Disease = ConvertmuchDisease(Convert.ToString(tempresponse.Data.Disease), "A003");

                    response.Data.Add
                       (new {

                           Name = tempresponse.Data.Name,//患者姓名
                           sex = tempresponse.Data.Gender,//性别
                           Age = tempresponse.Data.Age,//年 龄
                           SsNo = tempresponse.Data.SsNo,//社会保障卡号
                           McType = ls_McType,//tempresponse.Data.McType,//在职\退休\居民 
                           Disease = LS_Disease,//tempresponse.Data.Disease,//病种 病情描述及诊断
                            MakerId310 =  lsMakerId310,
                            MakerId315 = lsMakerId315,
                            MakerId3110 = lsMakerId3110,
 
                            MakerId320 = lsMakerId320,
                            MakerId325 = lsMakerId325,
                            MakerId3210 = lsMakerId3210,
                            MakerId3215 = lsMakerId3215,

                           
                            MakerId330 =lsMakerId330,  
                            MakerId335 =lsMakerId335,

                            MakerId340 = lsMakerId340,
                            MakerId345 = lsMakerId345,
                            MakerId3410 = lsMakerId3410,

                            MakerId350 =lsMakerId350, 
                            MakerId355 =lsMakerId355, 

                            MakerId360 = lsMakerId360,
                            MakerId365 = lsMakerId365,
                            MakerId3610 = lsMakerId3610,
                            MakerId3615 = lsMakerId3615,

                            MakerId370 = lsMakerId370,
                            MakerId375 = lsMakerId375,
                            MakerId3710 = lsMakerId3710,

                            MakerId380 =lsMakerId380,
                            MakerId385 =lsMakerId385, 
                            MakerId3810 =lsMakerId3810,

                            MakerId390 =lsMakerId390,
                            MakerId395 = lsMakerId395,
                            MakerId3910 = lsMakerId3910,

                            MakerId400 = lsMakerId400,
                            MakerId405 = lsMakerId405,
                            MakerId4010 = lsMakerId4010,

                           sumval = Convert.ToString(lssumval),

                            //经办机构
                             Orgid310 =  lsOrgid310  ,
                            Orgid315 = lsOrgid315,
                            Orgid3110 = lsMakerId3110, 

                            Orgid350 =lsOrgid350, 
                            Orgid355 =lsOrgid355, 
                            Orgid330 =lsOrgid330,  
                            Orgid335 =lsOrgid335,
                            Orgid380 =lsOrgid380,
                            Orgid385 =lsOrgid385, 
                            Orgid3810 =lsOrgid3810,
                            Orgid390 =lsOrgid390,
                            Orgid395 = lsOrgid395,
                            Orgid3910 = lsOrgid3910,
                            Orgid400 = lsOrgid400,
                            Orgid405 = lsOrgid405,
                            Orgid4010 = lsOrgid4010,
                            Orgid340 = lsOrgid340,
                            Orgid345 = lsOrgid345,
                            Orgid3410 = lsOrgid3410,
                            Orgid320 = lsOrgid320,
                            Orgid325 = lsOrgid325,
                            Orgid3210 = lsOrgid3210,
                            Orgid3215 = lsOrgid3215,
                            Orgid360 = lsOrgid360,
                            Orgid365 = lsOrgid365,
                            Orgid3610 = lsOrgid3610,
                            Orgid3615 = lsOrgid3615,
                            Orgid370 = lsOrgid370,
                            Orgid375 = lsOrgid375,
                            Orgid3710 = lsOrgid3710,
                           Orgidsum = Convert.ToString(lsOrgidsum),
                             

                             
                         });
            
        
            BindData(response.Data[0], doc);
        }
        private void printAuditingBILL(WordDocument doc, ReportRequest request)
        {
            IAppHospService service = IOCContainer.Instance.Resolve<IAppHospService>();
            
            var tempresponse = service.QueryAppShopInfo(request.id);
            var response = new BaseResponse<List<object>>(new List<object>());
            string ls_type="";
            if (tempresponse.Data.CareType == 1)
            { ls_type = "一级专护"; }
            else if (tempresponse.Data.CareType == 2)
            {
                ls_type = "二级专护";
            }
            else if (tempresponse.Data.CareType == 3)
            { ls_type = "机构护理"; }  //1  专护"  2- 医养 机构护理";
            string ls_McType = ConvertDisease(Convert.ToString(tempresponse.Data.Mctype), "A002");
            string LS_Disease = ConvertmuchDisease(Convert.ToString(tempresponse.Data.Disease), "A003");

            response.Data.Add
              (new
              {


                  Name11 = tempresponse.Data.Name,// "王大明",
                  Age11 = tempresponse.Data.Age,//"87",//年龄
                  SsNo = tempresponse.Data.SsNO,//社会保障卡号
                  Gender = tempresponse.Data.Gender,// "男",//
                  McType = ls_McType,//tempresponse.Data.Mctype,  // 人员身份 :□在职     □ 退休    □城乡居民,

                  Address = tempresponse.Data.Address,// 
                  EntryTime= tempresponse.Data.EntryTime,
                  FamilyMemberName = tempresponse.Data.FamilyMemberName,
                  DoctorName=  tempresponse.Data.DoctorName,
                  Phone = tempresponse.Data.Phone,//
                  AppReason = tempresponse.Data.AppReason,//申请原因
                  NsComment = tempresponse.Data.NsComment,//定点服务机构意见
                  IcComment = tempresponse.Data.Iccomment,//承办保险机构意见 
                  AgencyComment = tempresponse.Data.AgencyComment,//护理保险经办机构意见：
                  type = ls_type,//类别  □专护  机构护理  
                  Disease =LS_Disease,// tempresponse.Data.Disease,//病种
              });
            BindData(response.Data[0], doc);
             
        }

    }
}
