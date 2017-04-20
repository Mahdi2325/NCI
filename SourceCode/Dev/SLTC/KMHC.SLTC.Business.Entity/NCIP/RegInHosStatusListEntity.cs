using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class WebRegInHosStatusList
    {
        public IList<RegInHosStatusListEntity> Data { get; set; }
        //public int CurrentPage{ get; set; }
        //public int PagesCount{ get; set; }
        //public int RecordsCount{ get; set; }
        //public string ResultMessage { get; set; }
        //public string ResultCode { get; set; }
        //public int Id{ get; set; }
        //public string Token { get; set; }
        //public bool IsSuccess { get; set; }

        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public int RecordsCount { get; set; }
        public string ResultMessage { get; set; }
        public int ResultCode { get; set; }
        public long Id { get; set; }
    }

    public class RegInHosStatusListEntity
    {
        public string Name { get; set; }
        public string IdNo { get; set; }
        public string IpdFlag { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? LeHour { get; set; }               
        
    }
}
