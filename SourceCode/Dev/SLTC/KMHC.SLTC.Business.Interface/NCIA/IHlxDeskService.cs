using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIA
{
    public interface IHlxDeskService : IBaseService
    {
        object GetHeadMsg(dynamic d);
        object GetAppcertStatistics();
        object GetDeclareState();
        object GetRequireAppItem();
        object GetHlxSbaoHeadMsg();
        object GetHlxSbaoAppcertStatistics();
    }
}
