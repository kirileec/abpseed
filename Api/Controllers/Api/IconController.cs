using Abp.ObjectMapping;
using AbpSeed;
using App.Entities;
using App.Models.Request.Rbac.Icon;
using App.Models.Request.ViewModels.Rbac.DncIcon;
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
        private readonly IObjectMapper _mapper;
        public DBContext _dbcontext1 { get; set; }

        public IconController(DBContext dBContext,IObjectMapper mapper)
        {
            _dbContext = dBContext;
            _mapper = mapper;
        }
        [HttpPost]
        public void List(IconRequestPayload payload)
        {
            return;
        }
        [HttpGet("/api/rbac/icon/find_list_by_kw/{kw}")]
        public IActionResult FindByKeyword(string kw)
        {
            if (string.IsNullOrEmpty(kw))
            {
                return JsonFail("没有查询到数据");
            }
            using (_dbContext)
            {

                var query = _dbContext.DncIcon.Where(x => x.Code.Contains(kw));

                var list = query.ToList();
                var data = list.Select(x => new { x.Code, x.Color, x.Size });
                return JsonList(data.Count(),data);
            }
        }


        /// <summary>
        /// 创建图标
        /// </summary>
        /// <param name="model">图标视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(IconCreateViewModel model)
        {
           
            if (model.Code.Trim().Length <= 0)
            {
                return JsonFail("请输入图标名称");
            }
            using (_dbContext)
            {
                if (_dbContext.DncIcon.Count(x => x.Code == model.Code) > 0)
                {
                    return JsonFail("图标已存在");
                }
                var entity = _mapper.Map<DncIcon>(model);
                entity.CreatedOn = DateTime.Now;
                //entity.CreatedByUserGuid = AuthContextService.CurrentUser.Guid;
                //entity.CreatedByUserName = AuthContextService.CurrentUser.DisplayName;
                _dbContext.DncIcon.Add(entity);
                _dbContext.SaveChanges();

                return JsonSuccess();
          }
        }

    }
}
