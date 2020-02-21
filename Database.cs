using System.Collections.Generic;
using DependencyInjection.Model;
using Npgsql;

namespace DependencyInjection
{
    public interface IDatabase
    {
        int Create(Member member);
        List<Member> Read();
        List<Member> GetById(int id);
        int Update(Member member, int id);
        int Delete(int id);
    }
    class Database : IDatabase
    {   
        NpgsqlConnection _connection;

        public Database (NpgsqlConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public int Create(Member member)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO member (username, pass, email, fullname, popularity) VALUES (@username, @pass, @email, @fullname, @popularity) RETURNING id";

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

        public List<Member> Read()
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM member";
            var result = command.ExecuteReader();
            var members = new List<Member>();

            while (result.Read())
                members.Add(new Member(){
                    Id = (int)result[0], Username = (string)result[1], Password = (string)result[2], Email = (string)result[3], FullName = (string)result[4], Popularity = (string)result[5]
                });

            _connection.Close();

            return members;
        }

        public List<Member> GetById(int id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM member WHERE id={id}";
            var result = command.ExecuteReader();
            var members = new List<Member>();

            return members;
        }

        public int Update(Member member, int id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"UPDATE member (id, username, pass, email, fullname, popularity) SET VALUES (@id, @username, @pass, @email, @fullname, @popularity) WHERE id={id}";

            command.Parameters.AddWithValue("@id", member.Id);
            command.Parameters.AddWithValue("@username", member.Username);
            command.Parameters.AddWithValue("@pass", member.Password);
            command.Parameters.AddWithValue("@email", member.Email);
            command.Parameters.AddWithValue("@fullname", member.FullName);
            command.Parameters.AddWithValue("@popularity", member.Popularity);

            command.Prepare();

            var result = command.ExecuteNonQuery();
            _connection.Close();

            return (int)result;
        }

        public int Delete(int id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"DELETE FROM member WHERE id={id}";
            var result = command.ExecuteNonQuery();
            _connection.Close();
            return (int)result;
        }
    }
}