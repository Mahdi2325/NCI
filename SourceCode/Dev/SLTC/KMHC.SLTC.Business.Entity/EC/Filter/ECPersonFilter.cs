using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Filter
{
    public class ECPersonFilter
    {
        /// <summary>
        /// 社区Id
        /// </summary>
        public int CommunityId { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 申请人IdNo
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// 社区类型
        /// </summary>
        public int CommunityType { get; set; }

        /// <summary>
        /// 搜索内容
        /// </summary>
        public string keyWords { get; set; }
    }
}
