using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public interface ISoftDeleteEntity
    {
        string CreateBy { get; set; }
        DateTime? CreateTime { get; set; }
        string UpdateBy { get; set; }
        DateTime? UpdateTime { get; set; }
        bool? IsDelete { get; set; }
    }
}
