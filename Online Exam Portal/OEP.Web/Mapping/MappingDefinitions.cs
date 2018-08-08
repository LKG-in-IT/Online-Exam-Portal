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
using OEP.Core.DomainModels.Identity;
using OEP.Core.DomainModels.ResultModel;
using OEP.Core.DomainModels.StudyMaterialsModel;

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

                m.CreateMap<QuestionsLocalizedResource, QuestionsLocalized>();
                m.CreateMap<QuestionsLocalized, QuestionsLocalizedResource>();

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

                m.CreateMap<QuestionTypeResource, QuestionType>();
                m.CreateMap<QuestionType, QuestionTypeResource>();

                m.CreateMap<ApplicationUser, ApplicationUserResource>();
                m.CreateMap<ApplicationUserResource, ApplicationUser>();

                m.CreateMap<Result, ResultResource>();
                m.CreateMap<ResultResource, Result>();

                m.CreateMap<Questions, QuestionsViewResource>().ForPath(x => x.options.a, z => z.MapFrom(u => u.OptionA))
                .ForPath(x => x.options.b, z => z.MapFrom(u => u.OptionB))
                .ForPath(x => x.options.c, z => z.MapFrom(u => u.OptionC))
                .ForPath(x => x.options.d, z => z.MapFrom(u => u.OptionD));

                m.CreateMap<StudyMaterial, StudyMaterialsResources>();
                m.CreateMap<StudyMaterialsResources, StudyMaterial>();

            });
        }
    }
}