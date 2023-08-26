using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsRAMController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsRAMController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ListRAM")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<ProductsRAM> lstproducts = new List<ProductsRAM>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsRAM ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsRAM products = new ProductsRAM();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);                  
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.TheHe = Convert.ToString(dt.Rows[i]["TheHe"]);
                    products.Bus = Convert.ToString(dt.Rows[i]["Bus"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.LoaiHang = Convert.ToString(dt.Rows[i]["LoaiHang"]);
                    products.PartNumber = Convert.ToString(dt.Rows[i]["PartNumber"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.DungLuong = Convert.ToString(dt.Rows[i]["DungLuong"]);
                    products.Vol = Convert.ToString(dt.Rows[i]["Vol"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listram = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listram = null;
            }

            return response;
        }

        [HttpGet]
        [Route("GetRAMById/{id}")]
        public Response GetRAMById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsRAM WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ProductsRAM product = new ProductsRAM();
                        product.ProductID = Convert.ToInt32(reader["ProductID"]);
                        product.ProductName = Convert.ToString(reader["ProductName"]);
                        product.Description = Convert.ToString(reader["Description"]);
                        product.Brand = Convert.ToString(reader["Brand"]);
                        product.Discount = Convert.ToDecimal(reader["Discount"]);
                        product.Price = Convert.ToDecimal(reader["Price"]);
                        product.Image = Convert.ToString(reader["Image"]);
                        product.BaoHanh = Convert.ToString(reader["BaoHanh"]);
                        product.MauSac = Convert.ToString(reader["MauSac"]);
                        product.TheHe = Convert.ToString(reader["TheHe"]);
                        product.Bus = Convert.ToString(reader["Bus"]);
                        product.DenLed = Convert.ToString(reader["DenLed"]);
                        product.LoaiHang = Convert.ToString(reader["LoaiHang"]);
                        product.PartNumber = Convert.ToString(reader["PartNumber"]);
                        product.NhuCau = Convert.ToString(reader["NhuCau"]);
                        product.DungLuong = Convert.ToString(reader["DungLuong"]);
                        product.Vol = Convert.ToString(reader["Vol"]);
                        product.Type = Convert.ToString(reader["Type"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.listram = new List<ProductsRAM> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.listram = null;
                    }

                    reader.Close();
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

        [HttpGet]
        [Route("TotalCount")]
        public Response GetTotalProductCount()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM ProductsRAM";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    int totalCount = Convert.ToInt32(cmd.ExecuteScalar());

                    response.StatusCode = 200;
                    response.StatusMessage = "Total product count found";
                    response.TotalCount = totalCount;
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


        [HttpPost]
        [Route("AddRAM")]
        public Response AddProduct(ProductsRAM obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsRAM (ProductName, Description, Brand, Discount, Price, Image, BaoHanh, MauSac, TheHe, Bus, DenLed, LoaiHang, PartNumber, NhuCau, DungLuong, Vol, Type, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @Image, @BaoHanh, @MauSac, @TheHe, @Bus, @DenLed, @LoaiHang, @PartNumber, @NhuCau, @DungLuong, @Vol, @Type, @NgayNhap)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductName", obj.ProductName);
                    cmd.Parameters.AddWithValue("@Description", obj.Description);
                    cmd.Parameters.AddWithValue("@Brand", obj.Brand);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@Image", obj.Image);
                    cmd.Parameters.AddWithValue("@BaoHanh", obj.BaoHanh);
                    cmd.Parameters.AddWithValue("@MauSac", obj.MauSac);
                    cmd.Parameters.AddWithValue("@TheHe", obj.TheHe);
                    cmd.Parameters.AddWithValue("@Bus", obj.Bus);
                    cmd.Parameters.AddWithValue("@DenLed", obj.DenLed);
                    cmd.Parameters.AddWithValue("@LoaiHang", obj.LoaiHang);
                    cmd.Parameters.AddWithValue("@PartNumber", obj.PartNumber);
                    cmd.Parameters.AddWithValue("@NhuCau", obj.NhuCau);
                    cmd.Parameters.AddWithValue("@DungLuong", obj.DungLuong);
                    cmd.Parameters.AddWithValue("@Vol", obj.Vol);
                    cmd.Parameters.AddWithValue("@Type", obj.Type);
                    cmd.Parameters.AddWithValue("@NgayNhap", obj.NgayNhap);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product added successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Failed to add product";
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
        [Route("UpdateRAM/{id}")]
        public Response UpdateProduct(int id, ProductsRAM updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsRAM " +
                               "SET ProductName = @ProductName, Description = @Description, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, Image = @Image, BaoHanh = @BaoHanh, " +
                               "MauSac = @MauSac, TheHe = @TheHe, Bus = @Bus, " +
                               "DenLed = @DenLed, LoaiHang = @LoaiHang, PartNumber = @PartNumber, " +
                               "NhuCau = @NhuCau, DungLuong = @DungLuong, Vol = @Vol, " +
                               "Type = @Type, NgayNhap = @NgayNhap " +
                               "WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    cmd.Parameters.AddWithValue("@ProductName", updatedProduct.ProductName);
                    cmd.Parameters.AddWithValue("@Description", updatedProduct.Description);
                    cmd.Parameters.AddWithValue("@Brand", updatedProduct.Brand);
                    cmd.Parameters.AddWithValue("@Discount", updatedProduct.Discount);
                    cmd.Parameters.AddWithValue("@Price", updatedProduct.Price);
                    cmd.Parameters.AddWithValue("@Image", updatedProduct.Image);
                    cmd.Parameters.AddWithValue("@BaoHanh", updatedProduct.BaoHanh);
                    cmd.Parameters.AddWithValue("@MauSac", updatedProduct.MauSac);
                    cmd.Parameters.AddWithValue("@TheHe", updatedProduct.TheHe);
                    cmd.Parameters.AddWithValue("@Bus", updatedProduct.Bus);
                    cmd.Parameters.AddWithValue("@DenLed", updatedProduct.DenLed);
                    cmd.Parameters.AddWithValue("@LoaiHang", updatedProduct.LoaiHang);
                    cmd.Parameters.AddWithValue("@PartNumber", updatedProduct.PartNumber);
                    cmd.Parameters.AddWithValue("@NhuCau", updatedProduct.NhuCau);
                    cmd.Parameters.AddWithValue("@DungLuong", updatedProduct.DungLuong);
                    cmd.Parameters.AddWithValue("@Vol", updatedProduct.Vol);
                    cmd.Parameters.AddWithValue("@Type", updatedProduct.Type);
                    cmd.Parameters.AddWithValue("@NgayNhap", updatedProduct.NgayNhap);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product updated successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found or failed to update";
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
        [Route("DeleteRAM/{id}")]
        public Response DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM ProductsRAM WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product deleted successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found or failed to delete";
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
