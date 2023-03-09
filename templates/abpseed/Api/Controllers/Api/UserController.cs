using Abp.ObjectMapping;

using App.Entities.Enums;
using App.Entities;
using App.Models.Request.Rbac.User;
using App.Models.ViewModels.Rbac.DncUser;
using Microsoft.AspNetCore.Mvc;
using App.Extensions.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.EFCore.MySQL;

namespace App.Api.Controllers.Api
{
    [ApiController]
    [Route("api/v1/rbac/[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly MySQLDBContext _dbContext;
        public IObjectMapper _mapper { get; set; }
        public UserController(MySQLDBContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(UserRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.DncUser.Where(x => x.UserType != UserType.SuperAdministrator);
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.Kw.Trim()) || x.DisplayName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.Status > UserStatus.All)
                {
                    query = query.Where(x => x.Status == payload.Status);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<UserJsonModel>);

                return JsonList(totalCount, data);
            }
         }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
            [ProducesResponseType(200)]
            public IActionResult Create(UserCreateViewModel model)
            {

                if (model.LoginName.Trim().Length <= 0)
                {

                    return JsonFail("请输入登录名称");
                }
                using (_dbContext)
                {
                    if (_dbContext.DncUser.Count(x => x.LoginName == model.LoginName) > 0)
                    {
   
                    return JsonFail("登录名已存在");
                    }
                    var entity = _mapper.Map<DncUser>(model);
                    entity.CreatedOn = DateTime.Now;
                    entity.Guid = Guid.NewGuid();
                    entity.Status = model.Status;
                    _dbContext.DncUser.Add(entity);
                    _dbContext.SaveChanges();

                    return JsonSuccess();
                }
            }

            /// <summary>
            /// 编辑用户
            /// </summary>
            /// <param name="guid">用户GUID</param>
            /// <returns></returns>
            [HttpGet("{guid}")]
            [ProducesResponseType(200)]
            public IActionResult Edit(Guid guid)
            {
                using (_dbContext)
                {
                    var entity = _dbContext.DncUser.FirstOrDefault(x => x.Guid == guid);

                return JsonData(_mapper.Map<UserEditViewModel>(entity));
                }
            }

            /// <summary>
            /// 保存编辑后的用户信息
            /// </summary>
            /// <param name="model">用户视图实体</param>
            /// <returns></returns>
            [HttpPost]
            [ProducesResponseType(200)]
            public IActionResult Edit(UserEditViewModel model)
            {
             
                using (_dbContext)
                {
                    var entity = _dbContext.DncUser.FirstOrDefault(x => x.Guid == model.Guid);
                    if (entity == null)
                    {
     
                    return JsonFail("用户不存在");
                    }
                    entity.DisplayName = model.DisplayName;
                    entity.IsDeleted = model.IsDeleted;
                    entity.IsLocked = model.IsLocked;
                    //entity.ModifiedByUserGuid = AuthContextService.CurrentUser.Guid;
                    //entity.ModifiedByUserName = AuthContextService.CurrentUser.DisplayName;
                    entity.ModifiedOn = DateTime.Now;
                    entity.Password = model.Password;
                    entity.Status = model.Status;
                    entity.UserType = model.UserType;
                    entity.Description = model.Description;
                    _dbContext.SaveChanges();

                return JsonSuccess();
                }
            }

            /// <summary>
            /// 删除用户
            /// </summary>
            /// <param name="ids">用户GUID,多个以逗号分隔</param>
            /// <returns></returns>
            [HttpGet("{ids}")]
            [ProducesResponseType(200)]
            public IActionResult Delete(string ids)
            {
        
                return UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            }

            /// <summary>
            /// 恢复用户
            /// </summary>
            /// <param name="ids">用户GUID,多个以逗号分隔</param>
            /// <returns></returns>
            [HttpGet("{ids}")]
            [ProducesResponseType(200)]
            public IActionResult Recover(string ids)
            {
                var response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                return Ok(response);
            }

            /// <summary>
            /// 批量操作
            /// </summary>
            /// <param name="command"></param>
            /// <param name="ids">用户ID,多个以逗号分隔</param>
            /// <returns></returns>
            [HttpGet]
            [ProducesResponseType(200)]
            public IActionResult Batch(string command, string ids)
            {
                
                switch (command)
                {
                    case "delete":
                        return UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                      
                    case "recover":
                        return UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                        
                    case "forbidden":
               
                        return UpdateStatus(UserStatus.Forbidden, ids);
                 
                    case "normal":
                        return UpdateStatus(UserStatus.Normal, ids);
                
                    default:
                        break;
                }
                return JsonSuccess();
            }

            #region 用户-角色
            /// <summary>
            /// 保存用户-角色的关系映射数据
            /// </summary>
            /// <param name="model"></param>
            /// <returns></returns>
            [HttpPost("/api/v1/rbac/user/save_roles")]
            public IActionResult SaveRoles(SaveUserRolesViewModel model)
            {
                var roles = model.AssignedRoles.Select(x => new DncUserRoleMapping
                {
                    UserGuid = model.UserGuid,
                    CreatedOn = DateTime.Now,
                    RoleCode = x.Trim()
                }).ToList();
                //_dbContext.Database.ExecuteSqlRaw("DELETE FROM DncUserRoleMapping WHERE UserGuid={0}", model.UserGuid);
                var rm = _dbContext.DncUserRoleMapping.Where(x => x.UserGuid == model.UserGuid).ToList();
                _dbContext.DncUserRoleMapping.RemoveRange(rm);
                _dbContext.SaveChanges();
                var success = true;
                if (roles.Count > 0)
                {
                    _dbContext.DncUserRoleMapping.AddRange(roles);
                    success = _dbContext.SaveChanges() > 0;
                }

                if (success)
                {
                return JsonSuccess();
                }
                else
                {
                return JsonFail("保存用户角色数据失败");
                }
               
            }
            #endregion

            #region 私有方法
            /// <summary>
            /// 删除用户
            /// </summary>
            /// <param name="isDeleted"></param>
            /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
            /// <returns></returns>
            private IActionResult UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
            {
                using (_dbContext)
                {
                    //var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    //var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                    //var sql = string.Format("UPDATE DncUser SET IsDeleted=@IsDeleted WHERE Guid IN ({0})", parameterNames);
                    //parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                    //_dbContext.Database.ExecuteSqlRaw(sql, parameters);
                    var idList = ids.Split(",").Select(x => new Guid(x)).ToList();
                    var users = _dbContext.DncUser.Where(x => idList.Contains(x.Guid)).ToList();
                    foreach (var user in users)
                    {
                        user.IsDeleted = isDeleted;
                    }
                    _dbContext.SaveChanges();

                    return JsonSuccess();
                }
            }

            /// <summary>
            /// 删除用户
            /// </summary>
            /// <param name="status">用户状态</param>
            /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
            /// <returns></returns>
            private IActionResult UpdateStatus(UserStatus status, string ids)
            {
                using (_dbContext)
                {
                    //var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    //var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                    //var sql = string.Format("UPDATE DncUser SET Status=@Status WHERE Guid IN ({0})", parameterNames);
                    //parameters.Add(new SqlParameter("@Status", (int)status));
                    //_dbContext.Database.ExecuteSqlRaw(sql, parameters);

                    var idList = ids.Split(",").Select(x => new Guid(x)).ToList();
                    var users = _dbContext.DncUser.Where(x => idList.Contains(x.Guid)).ToList();
                    foreach (var user in users)
                    {
                        user.Status = status;
                    }

                    _dbContext.SaveChanges();

                    
                    return JsonSuccess();
                }
            }
            #endregion
        }
    
}
