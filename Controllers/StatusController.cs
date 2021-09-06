using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ReportServiceWeb02.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportServiceWeb02.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusTask _statusTask;

        public StatusController(StatusTask statusTask)
        {
            _statusTask = statusTask;
        }

        // GET: api/v1/<StatusController>
        [HttpGet]
        public JsonResult Get()
        {   
            var st = new { statuscode = 200, reports = _statusTask.Reportes };

            return new JsonResult(st);
        }

        // GET api/v1/<StatusController>/5
        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            var result = new
            {
                statuscode = 200,
                report = id,
                response = _statusTask.Check(id)
            };

            return new JsonResult(result);
        }

        // PUT api/v1/<StatusController>/5
        [HttpPut("{id}")]
        public JsonResult Put(string id, [FromBody] string value)
        {
            var result = new
            {
                statuscode = 406,
                report = id,
                response = "Estado No Aceptado"
            };


            if (value == "Habilitar")
            {
                _statusTask.Habilitar(id);

                result = new
                {
                    statuscode = 200,
                    report = id,
                    response = _statusTask.Check(id)
                };

            }
            else if (value == "Deshabilitar")
            {
                _statusTask.Deshabilitar(id);

                result = new
                {
                    statuscode = 200,
                    report = id,
                    response = _statusTask.Check(id)
                };

            }

            return new JsonResult(result);
        }
    }
}
