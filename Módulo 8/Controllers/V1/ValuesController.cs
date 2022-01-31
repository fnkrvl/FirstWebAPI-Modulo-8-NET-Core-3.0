using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace MyWebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    //[ApiController]
    public class ValuesController
    {
        // GET: api/student
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/student/5
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/student
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/student/5
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/student/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}