using AutoMapper;
using NetBootcamp.Repositories.Entities.Users;
using NetBootcamp.Service.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBootcamp.Service.Users.Configurations
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDto>()
                .ForPath(x => x.CreatedDate,opt => opt.MapFrom(y=>y.CreatedDate.ToShortDateString())).ReverseMap();
        }
    }
}
