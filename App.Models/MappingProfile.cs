using App.Entities;
using App.Entities.QueryModels;
using App.Models.Request.ViewModels.Rbac.DncIcon;
using App.Models.ViewModels.Rbac.DncIcon;
using App.Models.ViewModels.Rbac.DncMenu;
using App.Models.ViewModels.Rbac.DncPermission;
using App.Models.ViewModels.Rbac.DncRole;
using App.Models.ViewModels.Rbac.DncUser;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region DncUser
            CreateMap<DncUser, UserJsonModel>();
            CreateMap<UserCreateViewModel, DncUser>();
            CreateMap<UserEditViewModel, DncUser>();
            CreateMap<DncUser, UserEditViewModel>();
            #endregion

            #region DncRole
            CreateMap<DncRole, RoleJsonModel>();
            CreateMap<RoleCreateViewModel, DncRole>();
            CreateMap<DncRole, RoleCreateViewModel>();
            #endregion

            #region DncMenu
            CreateMap<DncMenu, MenuJsonModel>();
            CreateMap<MenuCreateViewModel, DncMenu>();
            CreateMap<MenuEditViewModel, DncMenu>();
            CreateMap<DncMenu, MenuEditViewModel>();
            CreateMap<DncMenuQueryModel, DncMenu>();
            CreateMap<DncMenu, DncMenuQueryModel>();
            #endregion

            #region DncIcon
            CreateMap<DncIcon, IconCreateViewModel>();
            CreateMap<DncIcon, IconJsonModel>();
            CreateMap<IconCreateViewModel, DncIcon>();
            #endregion

            #region DncPermission
            CreateMap<DncPermission, PermissionJsonModel>()
                .ForMember(d => d.MenuName, s => s.MapFrom(x => x.Menu.Name))
                .ForMember(d => d.PermissionTypeText, s => s.MapFrom(x => x.Type.ToString()));
            CreateMap<PermissionCreateViewModel, DncPermission>();
            CreateMap<PermissionEditViewModel, DncPermission>();
            CreateMap<DncPermission, PermissionEditViewModel>();
            #endregion
        }
    }
}
