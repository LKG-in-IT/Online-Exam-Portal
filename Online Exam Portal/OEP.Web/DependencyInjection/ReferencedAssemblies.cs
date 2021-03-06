﻿using System.Reflection;

namespace OEP.Web.DependencyInjection
{
    public static class ReferencedAssemblies
    {
        public static Assembly Services
        {
            get { return Assembly.Load("OEP.Services"); }
        }

        public static Assembly Repositories
        {
            get { return Assembly.Load("OEP.Data"); }
        }

        public static Assembly Resources
        {
            get
            {
                return Assembly.Load("OEP.Resources");
            }
        }

        public static Assembly Core
        {
            get
            {
                return Assembly.Load("OEP.Core");
            }
        }
    }
}