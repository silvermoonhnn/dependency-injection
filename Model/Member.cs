using System;

namespace DependencyInjection
{
    public class Member
    {
       public string Username { get; set; }

       public string Password { get; set; }

       public string Email { get; set; }

       public string FullName { get; set; }

       public string Popularity { get; set; }
    }

    public class IdMember : Member
    {
        public int Id { get; set; }
    }
}