using apiProducts.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoShopController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InfoShopController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetInfoShop")]
        public Response GetProductById()
        {
            int defaultId = 1; // ID mặc định là 1
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM InfoShop WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", defaultId);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        InfoShop product = new InfoShop()
                        {
                            SDT = Convert.ToString(row["SDT"]),
                            Address = Convert.ToString(row["Address"]),
                            Logo = Convert.ToString(row["Logo"]),
                            Map = Convert.ToString(row["Map"]),
                        };

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.InfoShop = new List<InfoShop> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.InfoShop = null;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        [HttpPut]
        [Route("UpdateInfoShop")]
        public Response UpdateInfoShop(InfoShop updatedInfoShop)
        {
            int defaultId = 1; // ID mặc định là 1
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE InfoShop " +
                               "SET SDT = @SDT, Address = @Address, Logo = @Logo, Map = @Map " +
                               "WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", defaultId);
                    cmd.Parameters.AddWithValue("@SDT", updatedInfoShop.SDT);
                    cmd.Parameters.AddWithValue("@Address", updatedInfoShop.Address);
                    cmd.Parameters.AddWithValue("@Logo", updatedInfoShop.Logo);
                    cmd.Parameters.AddWithValue("@Map", updatedInfoShop.Map);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "InfoShop updated successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "InfoShop not found or failed to update";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }


    }
}
