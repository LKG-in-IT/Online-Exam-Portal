﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.PackageModel
{
   public class Package: CommonDetailsEntity
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Double Prize { get; set; }

        public int Duration { get; set; }
    }
}
