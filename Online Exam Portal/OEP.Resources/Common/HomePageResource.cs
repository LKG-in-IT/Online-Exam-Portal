using System;
using System.Collections.Generic;
using OEP.Core.DomainModels.Identity;
using OEP.Resources.Admin;

namespace OEP.Resources.Common
{
    public class HomePageResource
    {
        public IEnumerable<PackageResource> Packages { get; set; }
    }

    public class PackagePageResource
    {
        public IEnumerable<PackageResource> Packages { get; set; }

        public ApplicationUserResource User { get; set; }

        public bool Expired { get; set; }

        public DateTime ExpiryDate { get; set; }
        
    }
}
