using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Base
{
    public class BaseResponse
    {
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public int RecordsCount { get; set; }
        public string ResultMessage { get; set; }
        public int ResultCode { get; set; }
        public long Id { get; set; }
    }

    public class BaseResponse<T> : BaseResponse 
    {
        public T Data { get; set; }

        public BaseResponse()
        {
        }

        public BaseResponse(T data)
        {
            this.Data = data;
        }
    }
}
