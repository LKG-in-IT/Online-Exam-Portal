using AutoMapper;
using OEP.Core.DomainModels.CategoryModel;
using OEP.Resources.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OEP.Core.DomainModels.EducationModels;

namespace OEP.Web.Mapping
{
    public static class MappingDefinitions
    {
        public static void CreateMap()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<Category, CategoryResource>();
                m.CreateMap<CategoryResource, Category>();

                m.CreateMap<YearResource, YearDetails>();
                m.CreateMap<YearDetails, YearResource>();
            });
        }
    }
}