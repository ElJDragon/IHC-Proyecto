using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Application.Models;
using GestionIncidentes.Domain.Entities;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionIncidentes.Application.Mapping
{
    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Workload, opt => opt.MapFrom(src => src.Workload));
        }
    }
}
