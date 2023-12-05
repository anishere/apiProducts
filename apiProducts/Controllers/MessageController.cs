using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MessageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ListMess")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<Message> lstmess = new List<Message>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Message ORDER BY ID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Message mess = new Message();
                    mess.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                    mess.Ten = Convert.ToString(dt.Rows[i]["Ten"]);
                    mess.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    mess.SDT = Convert.ToString(dt.Rows[i]["SDT"]);
                    mess.MessageDetail = Convert.ToString(dt.Rows[i]["MessageDetail"]);
                    lstmess.Add(mess);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listMessage = lstmess;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listMessage = null;
            }

            return response;
        }

        [HttpPost]
        [Route("AddMess")]
        public Response AddMessage(Message message)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO Message (Ten, Email, SDT, MessageDetail) " +
                               "VALUES (@Ten, @Email, @SDT, @MessageDetail)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Ten", message.Ten);
                    cmd.Parameters.AddWithValue("@Email", message.Email);
                    cmd.Parameters.AddWithValue("@SDT", message.SDT);
                    cmd.Parameters.AddWithValue("@MessageDetail", message.MessageDetail);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Message added successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Failed to add message";
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
        [Route("DeleteMess/{id}")]
        public Response DeleteMessage(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM Message WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Message deleted successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Message not found or failed to delete";
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
