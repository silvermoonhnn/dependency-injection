using System.Collections.Generic;
using DependencyInjection.Model;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace DependencyInjection
{
    public interface IDatabase
    {
        int Create(Member member);
        List<Member> Read();
        Member GetById(int id);
        int Update([FromBody]JsonPatchDocument<Member> member, int id);
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

        public Member GetById(int id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM member WHERE id={id}";
            var result = command.ExecuteReader();
            result.Read();
            var members = new Member()
            { 
                Id = (int)result[0], Username = (string)result[1], Password = (string)result[2], Email = (string)result[3], FullName = (string)result[4], Popularity = (string)result[5]
            };
            return members;
        }

        public int Update([FromBody]JsonPatchDocument<Member> member, int id)
        {
            var command = _connection.CreateCommand();
            var members = GetById(id);
            _connection.Open();
            member.ApplyTo(members);

            command.CommandText = $"UPDATE member SET (username, pass, email, fullname, popularity) = (@username, @pass, @email, @fullname, @popularity) WHERE id={id}";

            
            command.Parameters.AddWithValue("@username", members.Username);
            command.Parameters.AddWithValue("@pass", members.Password);
            command.Parameters.AddWithValue("@email", members.Email);
            command.Parameters.AddWithValue("@fullname", members.FullName);
            command.Parameters.AddWithValue("@popularity", members.Popularity);

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