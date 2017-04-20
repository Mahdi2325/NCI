using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class ResidentMonfeeService : BaseService, IResidentMonfeeService
    {
        /// <summary>
        /// 修改居民状态
        /// </summary>
        /// <param name="monfeeId">MonFeeId</param>
        /// <param name="status">状态</param>
        public void SaveResidentMonfeeStatus(int monfeeId, int status)
        {
            var resMonfee = unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet.Where(m => m.ISDELETE == false && m.NSMONFEEID == monfeeId).ToList();
            if (resMonfee != null)
            {
                foreach (var itemresMonfee in resMonfee)
                {
                    itemresMonfee.STATUS = status;
                    itemresMonfee.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    itemresMonfee.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().Update(itemresMonfee);
                }
                unitOfWork.Save();
            }
        }

        public void SaveDeductionMonfeeStatus(int monfeeId, int status)
        {
            var resMonfee = unitOfWork.GetRepository<NCIA_DEDUCTION>().dbSet.Where(m => m.ISDELETE == false && m.NSMONFEEID == monfeeId).ToList();
            if (resMonfee != null)
            {
                foreach (var itemresMonfee in resMonfee)
                {
                    itemresMonfee.STATUS = status;
                    itemresMonfee.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    itemresMonfee.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<NCIA_DEDUCTION>().Update(itemresMonfee);
                }
                unitOfWork.Save();
            }
        }
    }
}
