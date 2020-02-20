using System.Collections.Generic;
using Npgsql;

namespace DependencyInjection
{
    public interface IDatabase
    {
        int Create(IdMember member);
        void Read();
        void Update();
        void Delete();
    }
    class Database : IDatabase
    {   
        NpgsqlConnection _connection;

        public Database (NpgsqlConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public int Create(IdMember member)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO member (id, username, pass, email, fullname, popularity) VALUES (@id, @username, @pass, @email, @fullname, @popularity) RETURNING id";

            command.Parameters.AddWithValue("@id", member.Id);
            command.Parameters.AddWithValue("@username", member.Username);
            command.Parameters.AddWithValue("@pass", member.Password);
            command.Parameters.AddWithValue("@email", member.Email);
            command.Parameters.AddWithValue("@fullname", member.FullName);
            command.Parameters.AddWithValue("@popularity", member.Popularity);

            command.Prepare();

            var result = command.ExecuteScalar();
            _connection.Close();

            return (int)result;
        }

        public List<IdMember> Read()
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM member";
            var result = command.ExecuteReader();
            var Member = new List<IdMember>();

            while (result.Read())
                Member.Add(new IdMember(){

                });

            _connection.Close();

            return Member;
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }

        void IDatabase.Read()
        {
            throw new System.NotImplementedException();
        }
    }
}