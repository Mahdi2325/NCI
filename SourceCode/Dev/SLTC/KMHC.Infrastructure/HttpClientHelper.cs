using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace KMHC.Infrastructure
{
    public class HttpClientHelper
    {
        private static string nciAddress = "";
        public static string NciAddress
        {
            get
            {
                if (string.IsNullOrEmpty(nciAddress))
                {
                    nciAddress = ConfigurationManager.AppSettings["nciAddress"];
                }
                return nciAddress;
            }
        }
        private static readonly object NciLockObj = new object();

        private static HttpClient _nciClient;

        public static HttpClient NciHttpClient
        {
            get
            {
                if (_nciClient == null)
                {
                    lock (NciLockObj)
                    {
                        if (_nciClient == null)
                        {
                            _nciClient = new HttpClient() { BaseAddress = new Uri(NciAddress) };
                        }
                    }
                }

                return _nciClient;
            }
        }
    }
}
