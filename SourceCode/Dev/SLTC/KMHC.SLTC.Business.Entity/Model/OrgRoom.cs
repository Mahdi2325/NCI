/*
 * 描述:OrgRoom
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/24/2016 11:35:21 AM    张正泉            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class OrgRoom
    {
        public string RoomNo { get; set; }
        public string FloorId { get; set; }
        public string FloorName { get; set; }
        public string RoomName { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
    }
}
