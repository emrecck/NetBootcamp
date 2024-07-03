using AutoMapper;
using NetBootcamp.Repositories.Entities.Roles;
using NetBootcamp.Service.Roles.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBootcamp.Service.Roles.Configurations
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<Role, RoleDto>()
                .ForPath(x => x.CreatedDate, opt => opt.MapFrom(y=>y.CreatedDate.ToShortDateString())).ReverseMap();
        }
    }
}
