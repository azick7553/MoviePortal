using MoviePortal.Models.Movy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Services.Mapper
{
    public class MapperService : AutoMapper.Profile
    {
        public MapperService()
        {
            this.CreateMap<MoviePortal.Models.Movy.Movie, MovieDTO>()
                .ForMember(m => m.Categories, option => option.Ignore())
                .ForMember(m => m.CategoryId, option => option.MapFrom(m => m.CategoryId))
                .ForMember(m => m.CategoryName, option => option.MapFrom(m => m.MovieCategory.Name))
                .ForMember(m => m.Description, option => option.MapFrom(m => m.Description))
                .ForMember(m => m.Director, option => option.MapFrom(m => m.Director))
                .ForMember(m => m.Id, option => option.MapFrom(m => m.Id))
                .ForMember(m => m.ImageFile, option => option.Ignore())
                .ForMember(m => m.ImageName, option => option.MapFrom(m => m.Image))
                .ForMember(m => m.InsertDateTime, option => option.MapFrom(m => m.InsertDateTime))
                .ForMember(m => m.InsertUserId, option => option.MapFrom(m => m.InsertUserId))
                .ForMember(m => m.ReleaseDate, option => option.MapFrom(m => m.ReleaseDate))
                .ForMember(m => m.Title, option => option.MapFrom(m => m.Title))
                .ForMember(m => m.UpdateDate, option => option.MapFrom(m => m.UpdateDate))
                .ForMember(m => m.UserId, option => option.MapFrom(m => m.InsertUserId))
                .ForMember(m => m.UserName, option => option.MapFrom(m => m.User.UserName))
                .ReverseMap()
                .ForMember(m => m.CategoryId, option => option.MapFrom(m => m.CategoryId))
                .ForMember(m => m.Description, option => option.MapFrom(m => m.Description))
                .ForMember(m => m.Director, option => option.MapFrom(m => m.Director))
                .ForMember(m => m.Id, option => option.MapFrom(m => m.Id))
                .ForMember(m => m.Image, option => option.MapFrom(m => m.ImageName))
                .ForMember(m => m.InsertDateTime, option => option.MapFrom(m => m.InsertDateTime))
                .ForMember(m => m.InsertUserId, option => option.MapFrom(m => m.InsertUserId))
                .ForMember(m => m.IsDelete, option => option.Ignore())
                .ForMember(m => m.ReleaseDate, option => option.MapFrom(m => m.ReleaseDate))
                .ForMember(m => m.Title, option => option.MapFrom(m => m.Title))
                .ForMember(m => m.UpdateDate, option => option.MapFrom(m => m.UpdateDate))
                .ForMember(m => m.User, option => option.Ignore());
        }
    }
}
