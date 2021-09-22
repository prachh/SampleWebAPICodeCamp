using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebAPICodeCamp.Data;
using SampleWebAPICodeCamp.Repository.Provider;

namespace SampleWebAPICodeCamp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IPInfoController : ControllerBase
    {

        private readonly ILogger<IPInfoController> _logger;
        private readonly IIPRepository _IPRepository;

        public IPInfoController(ILogger<IPInfoController> logger, IIPRepository IPRepository)
        {
            _logger = logger;
            _IPRepository = IPRepository;
        }

        [HttpGet]
        public async Task<IPtoCountry> Get(string ip)
        {
            return await _IPRepository.GetIpInfo(ip);
        }
    }
}
