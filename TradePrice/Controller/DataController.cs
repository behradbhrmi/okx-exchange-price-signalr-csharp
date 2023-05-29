using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csTradePiceWebSocket.Controller
{
    [ApiController]
    [Route("{controller}")]
    public class DataController : ControllerBase
    {
        public DataController()
        {
            
        }
        [HttpGet("test")]
        public string Test()
        {
            return "Hello world";
        }
        [HttpPost]
        public string Test(string name)
        {
            return $"Hello world, {name}";
        }
    }
}
