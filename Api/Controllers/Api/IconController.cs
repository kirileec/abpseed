using AbpSeed;
using App.Models.Request.Rbac.Icon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Api.Controllers.Api
{
    [ApiController]
    public class IconController:BaseController
    {
        private readonly DBContext _dbContext;
        public DBContext _dbcontext1 { get; set; }

        public IconController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }
        [HttpPost]
        public void List(IconRequestPayload payload)
        {
            return;
        }

    }
}
