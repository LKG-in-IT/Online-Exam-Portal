using System.Collections.Generic;
using OEP.Resources.Admin;

namespace OEP.Resources.Common
{
    public class HomePageResource
    {
        public IEnumerable<PackageResource> Packages { get; set; }
    }
}
