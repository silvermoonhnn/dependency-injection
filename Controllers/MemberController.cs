using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.Extensions.Logging;
using DependencyInjection.Model;

namespace DependencyInjection.Controllers
{
    [ApiController]
    [Route("member")]
    public class MemberController : ControllerBase
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IDatabase _database;
        public MemberController(ILogger<MemberController> logger, IDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _database.Read();
            return Ok(result);
        }
        
        [HttpPost]
        public IActionResult MemberAdd(Member member)
        {
            var result = _database.Create(member);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _database.GetById(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMember(int id)
        {
            var result = _database.Delete(id);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchMember([FromBody]JsonPatchDocument<Member> member, int id)
        {
            var result = _database.Update(member, id);
            return Ok(result);
        }
    }
}