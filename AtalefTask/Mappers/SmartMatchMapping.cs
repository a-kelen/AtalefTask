using AtalefTask.DTOs;
using AtalefTask.Models;
using AtalefTask.ViewModels;
using AutoMapper;

namespace AtalefTask.Mappers
{
    public class SmartMatchMapping : Profile
    {
        public SmartMatchMapping()
        {
            CreateMap<SmartMatchViewModel, SmartMatchItem>();
            CreateMap<SmartMatchItem, SmartMatchDTO>();
        }
    }
}
