using AbpSeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Api.Controllers.Api
{
    public class MenuController:BaseController
    {
        private readonly DBContext _dbContext;

        public MenuController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }
    }
}
