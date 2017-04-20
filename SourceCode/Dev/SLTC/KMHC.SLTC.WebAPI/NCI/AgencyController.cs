﻿using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCI;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NCI
{
    [RoutePrefix("api/Agency")]
    public class AgencyController : BaseController
    {
        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(BaseRequest<ORGFilter> request)
        {
            var response = service.QueryOrgAgency(request);
            return Ok(response);
        }
    }
}
