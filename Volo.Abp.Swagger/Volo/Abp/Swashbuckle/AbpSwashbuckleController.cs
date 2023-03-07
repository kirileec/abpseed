using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Web.Security.AntiForgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.Swagger
{
    [Area("Abp")]
    [Route("Abp/Swashbuckle/[action]")]
    [DisableAuditing]
    [RemoteService(false)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AbpSwashbuckleController : AbpController
    {
        private readonly IAbpAntiForgeryManager _antiForgeryManager;
        private readonly HttpContext _httpContext;

        public AbpSwashbuckleController(IAbpAntiForgeryManager antiForgeryManager,HttpContext context)
        {
            _antiForgeryManager = antiForgeryManager;
            _httpContext = context;
        }

        [HttpGet]
        public void SetCsrfCookie()
        {
            _antiForgeryManager.SetCookie(_httpContext);
        }
    }

}
