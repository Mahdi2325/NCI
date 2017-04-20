/*
创建人: 姚丙慧
创建日期:2016-03-18
说明:康健
*/


using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface INursingRecord : IBaseService
    {
        #region 複健

        // <summary>
        /// 获取复建列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Rehabilitrec>> QueryRehabilition(BaseRequest<RecordFilter> request);
        /// <summary>
        /// 删除复健列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteRehabilition(int id);

        /// <summary>
        /// 删除新的列表
        /// </summary>
        /// <returns></returns>
        BaseResponse insertRehabilition(Rehabilitrec baseRequest);



        /// <summary>
        /// 根据id获取相关信息
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        BaseResponse<IList<Rehabilitrec>> GetRehabilition(BaseRequest<tt> request);


        #endregion

        #region 轉診

        BaseResponse<IList<TranSferVisit>> QueryReferralLis(BaseRequest<RecordFilter> request);


        BaseResponse DeleteReferralLis(int id);


        BaseResponse insertReferralLis(TranSferVisit baseRequest);

        BaseResponse<IList<Employee>> GetName();

        


        #endregion

        #region 生化检查

        /// <summary>
        /// 獲得參數跟子參數的數據
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CheckRec>> QueryBiochemistry(BaseRequest<RecordFilter> request);

        /// <summary>
        /// 刪除的子參數
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteBiochemistry(int id);





        //删除字表下面的内容

        BaseResponse DeletesBiochemistry(int id,int type);


        BaseResponse insertCheckRecdtl(CheckRecdtl baseRequest);

        BaseResponse insertCheckRecdtls(IList<CheckRecdtl> baseRequest);


        BaseResponse insertCheckRec(CheckRecCollection baseRequest);


        BaseResponse<IList<CheckGroup>> GetProduceCode();

        /// <summary>
        /// 根據類型的id 獲取下面的項目
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<CheckItem>> GetCheckType(string TYPECODE);


        //获取组下面的东西

        BaseResponse<IList<CheckItem>> produceitem();

        BaseResponse<IList<CheckItem>> Checkitem(string code);


        // 获得范围

        BaseResponse<IList<CheckItem>> GetCheckitem(string code);

        


        //插入子类的信息

        #endregion

        #region 護士日報表

        BaseResponse<IList<NurseRpttpr>> QueryNurseRpttpr(BaseRequest<RecordFilter> request);

        //删除护士日报表内的东西

        BaseResponse DeleteNurseDailyReport(int id);

        //根据时间,班别 加载生命体征了

        BaseResponse<IList<Vitalsign>> GetNurseDailyReport(int feeno);



        // 插入时间

        BaseResponse insertNurseRpttpr(NurseRpttpr baseRequest);
            
        //LTC_NURSERPTTPR

        //加载管路

        BaseResponse<IList<RecordFilter>> GetNurseDailyReportpipe(int feeno);

     

        #endregion


        #region  常用语设置

        BaseResponse<IList<CommFile>> QueryCommFile(BaseRequest<RecordFilter> request);
        //添加常用语
        BaseResponse insertCommfile(List<CommFile> baseRequest);

        BaseResponse DeleteCOMMFILE(int id);
        BaseResponse MulDeleteCOMMFILE(List<CommFile> cfs);
        
        //根据id获取相关信息

        BaseResponse<IList<CommFile>> GetCommfile(string id);

        #endregion

        #region
        // 獲取人員基本信息
        BaseResponse<Person> GetPr(int regon);

        #endregion

    }
}
