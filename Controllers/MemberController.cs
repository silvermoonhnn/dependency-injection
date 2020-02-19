using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DependencyInjection.Controllers
{
    [ApiController]
    [Route("member")]
    public class MemberController : ControllerBase
    {
        private static List<IdMember> Members = new List<IdMember>()
        {
            new IdMember(){Id=1, Username="qwerty", Password="mnbv12", Email="email@gmail.com", FullName="hahaha", Popularity="80"},
            new IdMember(){Id=2, Username="qwerty", Password="mnbv12", Email="email@gmail.com", FullName="hahaha", Popularity="80"},
            new IdMember(){Id=3, Username="qwerty", Password="mnbv12", Email="email@gmail.com", FullName="hahaha", Popularity="80"},
            new IdMember(){Id=4, Username="qwerty", Password="mnbv12", Email="email@gmail.com", FullName="hahaha", Popularity="80"},
            new IdMember(){Id=5, Username="qwerty", Password="mnbv12", Email="email@gmail.com", FullName="hahaha", Popularity="80"},
        };

        private readonly ILogger<MemberController> _logger;

        public MemberController(ILogger<MemberController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status="success", message="success get Data", data = Members});
        }
        
        [HttpPost]
        public IActionResult MemberAdd(IdMember mem)
        {
            var addMember = new IdMember(){Id=mem.Id, Username = mem.Username, Password = mem.Password, Email = mem.Email, FullName = mem.FullName, Popularity = mem.Popularity};
            Members.Add(addMember);
            return Ok(new { status = "success", message = "success add Data", data = Members});
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(Members.Find( e => e.Id == id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMember(int id)
        {
            var del = Members.Find( e => e.Id == id);
            Members.Remove(del);
            return Ok(new { status = "deleted", message = "success delete some data", data = Members});
        }

        [HttpPatch("{id}")]
        public IActionResult PatchMember(int id, [FromBody] JsonPatchDocument<IdMember> patchMem)
        {
            patchMem.ApplyTo(Members.Find(e => e.Id == id));
            return Ok( new { status = "updated", message = "success update data", data = Members.Find(e => e.Id == id) });
        }
    }
}