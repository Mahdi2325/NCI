using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Entity
{
    public static class SoftDeleteEntityExtension
    {
        public static void TrackingPropertyCreate(this ISoftDeleteEntity entity, string createBy = null)
        {
            createBy = createBy ?? SecurityHelper.CurrentPrincipal.UserId.ToString();
            entity.CreateBy = createBy;
            entity.UpdateBy = createBy;
            entity.CreateTime = entity.UpdateTime = DateTime.Now;
            entity.IsDelete = false;
        }

        public static void TrackingPropertyUpdate(this ISoftDeleteEntity entity, string updateBy = null)
        {
            updateBy = updateBy ?? SecurityHelper.CurrentPrincipal.UserId.ToString();
            entity.UpdateBy = updateBy;
            entity.UpdateTime = DateTime.Now;
        }
    }
}
