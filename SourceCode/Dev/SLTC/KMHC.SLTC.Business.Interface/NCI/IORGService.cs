using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCI;
using KMHC.SLTC.Business.Entity.NCI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCI
{
    public interface IORGService : IBaseService
    {

        BaseResponse<IList<Agency>> QueryOrgAgency(BaseRequest<ORGFilter> request);
        BaseResponse<IList<NursingHome>> QueryOrgNsHome(BaseRequest<ORGFilter> request);
        /// <summary>
        /// 经办机构人员 信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NCI_User>> QueryAgencyUserExtend(BaseRequest<NCI_UserFilter> request);
        /// <summary>
        /// 定点服务机构人员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NCI_User>> QueryNsHomeUserExtend(BaseRequest<NCI_UserFilter> request);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NCI_User>> QueryUser(BaseRequest<NCI_UserFilter> request);
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        BaseResponse<NCI_User> GetUser(int userID);

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<NCI_User> SaveUser(NCI_User request);

        BaseResponse DeleteUser(int userID);
        BaseResponse ChangePassWord(string orgID, string logonName, string oldPassword, string newPassword);
    }
}
