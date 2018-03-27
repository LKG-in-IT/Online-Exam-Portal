using AutoMapper;
using OEP.Core.DomainModels.CategoryModel;
using OEP.Resources.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEP.Web.Mapping
{
    public static class MappingDefinitions
    {
        public static void CreateMap()
        {
            Mapper.Initialize(m => {
                m.CreateMap<Category, CategoryResource>();
                m.CreateMap<CategoryResource, Category>();
            });
        }
    }
}