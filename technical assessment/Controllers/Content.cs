using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace technical_assessment.Controllers
{
    
    public class Content
    {
        public int per_page { get; set; }
        public int current_page { get; set; }
        public int total_page { get; set; }
        public object data { get; set; }
    }
}