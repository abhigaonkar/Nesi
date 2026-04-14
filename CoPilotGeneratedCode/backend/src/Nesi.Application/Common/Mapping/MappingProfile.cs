using AutoMapper;
using Nesi.Application.DTOs.Auth;
using Nesi.Application.DTOs.Timesheet;
using Nesi.Application.DTOs.User;
using Nesi.Application.DTOs.Customer;
using Nesi.Application.DTOs.WorkOrder;
using Nesi.Domain.Entities;

namespace Nesi.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<User, LoginResponse>();

        // Customer mappings
        CreateMap<Customer, CustomerDto>();

        // WorkOrder mappings
        CreateMap<Domain.Entities.WorkOrder, WorkOrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.Ignore());

        // Timesheet mappings
        CreateMap<TimesheetEntry, TimesheetDto>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.PayTypeName, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrderNumber, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrderDescription, opt => opt.Ignore())
            .ForMember(dest => dest.JobTypeName, opt => opt.Ignore());

        CreateMap<CreateTimesheetRequest, TimesheetEntry>();
        CreateMap<UpdateTimesheetRequest, TimesheetEntry>();
    }
}
