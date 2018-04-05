using AutoMapper;
using OEP.Core.DomainModels.CategoryModel;
using OEP.Resources.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OEP.Core.DomainModels.EducationModels;
using OEP.Core.DomainModels.SubCategoryModel;
using OEP.Core.DomainModels.PackageModel;
using OEP.Core.DomainModels.QuestionModel;
using OEP.Core.DomainModels.ExamModels;
using OEP.Core.DomainModels.PackageSelectedModels;

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

                m.CreateMap<EducationTypeResource, EducationType>();
                m.CreateMap<EducationType, EducationTypeResource>();

                m.CreateMap<SubCategory, SubCategoryResource>();
                m.CreateMap<SubCategoryResource, SubCategory>();

                m.CreateMap<PackageResource, Package>();
                m.CreateMap<Package, PackageResource>();

                m.CreateMap<QuestionsResource, Questions>();
                m.CreateMap<Questions, QuestionsResource>();

                m.CreateMap<ExamTypeResource, ExamType>();
                m.CreateMap<ExamType, ExamTypeResource>();

                m.CreateMap<ExamResource, Exam>();
                m.CreateMap<Exam, ExamResource>();

                m.CreateMap<ExamQuestionResource, ExamQuestion>();
                m.CreateMap<ExamQuestion, ExamQuestionResource>();


                m.CreateMap<PackageSelectedResource, PackageSelected>();
                m.CreateMap<PackageSelected, PackageSelectedResource>();

                m.CreateMap<Questions, QuestionAutoCompleteResource > ();

                m.CreateMap<EducationDetailsResource, EducationDetails>();
                m.CreateMap<EducationDetails, EducationDetailsResource>();


            });
        }
    }
}