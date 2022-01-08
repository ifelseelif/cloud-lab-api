using Api.Database.Models;
using AutoMapper;
using Cloud_Lab.Entities.Requests;

namespace Cloud_Lab.DataAccess.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCredential, User>();
        }
    }
}