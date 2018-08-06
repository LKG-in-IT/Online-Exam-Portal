using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEP.Web.DependencyInjection.Modules.AttributeFilter
{
    public static class ContainerBuilderExtension
    {
        /// <summary>
        /// Registers the <see cref="AutofacFilterAttributeFilterProvider"/>.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public static void RegisterFilter(this ContainerBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException("builder");

            foreach (var provider in FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().ToArray())
                FilterProviders.Providers.Remove(provider);

            builder.RegisterType<AutofacFilterAttributeFilterProvider>()
                .As<IFilterProvider>()
                .SingleInstance();
        }
    }
}