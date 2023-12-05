using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using apiProducts.Models;
using System.Security.Principal;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using BCrypt.Net;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]

        public Boolean login (Account account)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Account WHERE UserName = '"+account.UserName+"' AND Password = '"+account.Password+"'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [Route("update")]
        
        public Boolean update (string newPassword) 
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            SqlDataAdapter da = new SqlDataAdapter("UPDATE Account SET Password = @NewPassword WHERE UserName = @UserName", connection);
            da.SelectCommand.Parameters.AddWithValue("@NewPassword", newPassword);
            da.SelectCommand.Parameters.AddWithValue("@UserName", "admin");
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
