using apiProducts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InformationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("InformationList")]

        public Response GetAllInformation ()
        {
            List<InformationCustomer> lstinformation = new List<InformationCustomer>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM InformationCustomer", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if(dt.Rows.Count > 0) {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    InformationCustomer information = new InformationCustomer();
                    information.PhoneNumber = Convert.ToString(dt.Rows[i]["PhoneNumber"]);
                    information.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    information.Address = Convert.ToString(dt.Rows[i]["Address"]);
                    information.ListCart = Convert.ToString(dt.Rows[i]["ListCart"]);
                    information.TotalPrice = Convert.ToDecimal(dt.Rows[i]["TotalPrice"]);
                    lstinformation.Add(information);
                }
                if (lstinformation.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Data found";
                    response.listcustomers = lstinformation;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No data found";
                    response.listcustomers = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listcustomers = null;
            }
            return response;
        }

        [HttpDelete]
        [Route("DeleteInformation/{phoneNumber}")]
        public Response DeleteInformation(string phoneNumber)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM InformationCustomer WHERE PhoneNumber = @PhoneNumber";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Information deleted successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Information not found or failed to delete";
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
