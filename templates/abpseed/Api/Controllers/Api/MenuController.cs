
using App.EFCore.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Api.Controllers.Api
{
    public class MenuController:BaseController 
    {
        private readonly MySQLDBContext _dbContext;

        public MenuController(MySQLDBContext dBContext)
        {
            _dbContext = dBContext;
        }
    }
}
