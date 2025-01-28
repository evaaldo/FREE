using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using UsersDocker.Entites.Models;

namespace UsersDocker.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : ControllerBase
    {
        private readonly string _con;

        public UserController(string con)
        {
            _con = con;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            using (IDbConnection connection = new NpgsqlConnection(_con))
            {
                var users = connection.Query("SELECT * FROM Users");
                return Ok(users);
            }
        }

        [HttpPost]
        public IActionResult PostUser(User user)
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_con))
                {
                    var sql = "INSERT INTO Users (ID, NAME, PASSWORD) VALUES (@Id, @Name, @Password)";
                    connection.Execute(sql, new { Id = user.ID, Name = user.Name, Password = user.Password });

                    return Ok("User created!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult PutUser(User user)
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_con))
                {
                    var sql = "UPDATE Users SET NAME = @Name, PASSWORD = @Password WHERE ID= @Id";
                    connection.Execute(sql, new { Name = user.Name, Password = user.Password, Id = user.ID });

                    return Ok("User updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_con))
                {
                    var sql = "DELETE FROM Users WHERE ID = @Id";
                    connection.Execute(sql, new { Id = id });

                    return Ok("User removed!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
