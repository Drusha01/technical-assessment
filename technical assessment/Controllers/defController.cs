using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace technical_assessment.Controllers
{
    public class defController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public string Get(int id)
        {
            return "valuadsfasdfe";
        }
    }
}
