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
        public Response GetAllInformation()
        {
            List<InformationCustomer> lstinformation = new List<InformationCustomer>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM InformationCustomer", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    InformationCustomer information = new InformationCustomer();
                    information.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
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

        [HttpPost]
        [Route("AddInformation")]
        public Response AddInformation(InformationCustomer obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO InformationCustomer (PhoneNumber, Name, Address, ListCart, TotalPrice) " +
                               "VALUES (@PhoneNumber, @Name, @Address, @ListCart, @TotalPrice); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PhoneNumber", obj.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Name", obj.Name);
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                    cmd.Parameters.AddWithValue("@ListCart", obj.ListCart);
                    cmd.Parameters.AddWithValue("@TotalPrice", obj.TotalPrice);

                    // Không cần truyền giá trị ID

                    int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    if (newId > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Information added successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Failed to add information";
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



        [HttpDelete]
        [Route("DeleteInformationById/{id}")]
        public Response DeleteInformationById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM InformationCustomer WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

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
