using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ProductList")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<ProductsPcLaptop> lstproducts = new List<ProductsPcLaptop>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsPCLapTop ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsPcLaptop products = new ProductsPcLaptop();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Discription = Convert.ToString(dt.Rows[i]["Discription"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.CPU = Convert.ToString(dt.Rows[i]["CPU"]);
                    products.RAM = Convert.ToString(dt.Rows[i]["RAM"]);
                    products.ManHinh = Convert.ToString(dt.Rows[i]["ManHinh"]);
                    products.HeDieuHanh = Convert.ToString(dt.Rows[i]["HeDieuHanh"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.CardDoHoa = Convert.ToString(dt.Rows[i]["CardDoHoa"]);
                    products.BanPhim = Convert.ToString(dt.Rows[i]["BanPhim"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.LuuTru = Convert.ToString(dt.Rows[i]["LuuTru"]);
                    products.PhuKien = Convert.ToString(dt.Rows[i]["PhuKien"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listproducts = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listproducts = null;
            }

            return response;
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public Response GetProductById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsPCLapTop WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        ProductsPcLaptop product = new ProductsPcLaptop()
                        {
                            ProductID = Convert.ToInt32(row["ProductID"]),
                            ProductName = Convert.ToString(row["ProductName"]),
                            Discription = Convert.ToString(row["Discription"]),
                            Brand = Convert.ToString(row["Brand"]),
                            Discount = Convert.ToDecimal(row["Discount"]),
                            Price = Convert.ToDecimal(row["Price"]),
                            Image = Convert.ToString(row["Image"]),
                            Type = Convert.ToString(row["Type"]),
                            BaoHanh = Convert.ToString(row["BaoHanh"]),
                            CPU = Convert.ToString(row["CPU"]),
                            RAM = Convert.ToString(row["RAM"]),
                            ManHinh = Convert.ToString(row["ManHinh"]),
                            HeDieuHanh = Convert.ToString(row["HeDieuHanh"]),
                            KhoiLuong = Convert.ToString(row["KhoiLuong"]),
                            CardDoHoa = Convert.ToString(row["CardDoHoa"]),
                            BanPhim = Convert.ToString(row["BanPhim"]),
                            MauSac = Convert.ToString(row["MauSac"]),
                            NhuCau = Convert.ToString(row["NhuCau"]),
                            LuuTru = Convert.ToString(row["LuuTru"]),
                            PhuKien = Convert.ToString(row["PhuKien"]),
                            KieuKetNoi = Convert.ToString(row["KieuKetNoi"])
                        };

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.listproducts = new List<ProductsPcLaptop> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.listproducts = null;
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


        [HttpPost]
        [Route("AddProduct")]

        public Response AddProduct(ProductsPcLaptop obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsPCLapTop (ProductName, Discription, Brand, Discount, Price, Image, Type, BaoHanh, CPU, RAM, ManHinh, HeDieuHanh, KhoiLuong, CardDoHoa, BanPhim, MauSac, NhuCau, LuuTru, PhuKien, KieuKetNoi) " +
                               "VALUES (@ProductName, @Discription, @Brand, @Discount, @Price, @Image, @Type, @BaoHanh, @CPU, @RAM, @ManHinh, @HeDieuHanh, @KhoiLuong, @CardDoHoa, @BanPhim, @MauSac, @NhuCau, @LuuTru, @PhuKien, @KieuKetNoi)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductName", obj.ProductName);
                    cmd.Parameters.AddWithValue("@Discription", obj.Discription);
                    cmd.Parameters.AddWithValue("@Brand", obj.Brand);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@Image", obj.Image);
                    cmd.Parameters.AddWithValue("@Type", obj.Type);
                    cmd.Parameters.AddWithValue("@BaoHanh", obj.BaoHanh);
                    cmd.Parameters.AddWithValue("@CPU", obj.CPU);
                    cmd.Parameters.AddWithValue("@RAM", obj.RAM);
                    cmd.Parameters.AddWithValue("@ManHinh", obj.ManHinh);
                    cmd.Parameters.AddWithValue("@HeDieuHanh", obj.HeDieuHanh);
                    cmd.Parameters.AddWithValue("@KhoiLuong", obj.KhoiLuong);
                    cmd.Parameters.AddWithValue("@CardDoHoa", obj.CardDoHoa);
                    cmd.Parameters.AddWithValue("@BanPhim", obj.BanPhim);
                    cmd.Parameters.AddWithValue("@MauSac", obj.MauSac);
                    cmd.Parameters.AddWithValue("@NhuCau", obj.NhuCau);
                    cmd.Parameters.AddWithValue("@LuuTru", obj.LuuTru);
                    cmd.Parameters.AddWithValue("@PhuKien", obj.PhuKien);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", obj.KieuKetNoi);

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

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public Response DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM ProductsPCLapTop WHERE ProductID = @ProductID";

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

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public Response UpdateProduct(int id, ProductsPcLaptop updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsPCLapTop " +
                               "SET ProductName = @ProductName, Discription = @Discription, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, Image = @Image, Type = @Type, " +
                               "BaoHanh = @BaoHanh, CPU = @CPU, RAM = @RAM, " +
                               "ManHinh = @ManHinh, HeDieuHanh = @HeDieuHanh, KhoiLuong = @KhoiLuong, " +
                               "CardDoHoa = @CardDoHoa, BanPhim = @BanPhim, MauSac = @MauSac, " +
                               "NhuCau = @NhuCau, LuuTru = @LuuTru, PhuKien = @PhuKien, KieuKetNoi = @KieuKetNoi " +
                               "WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    cmd.Parameters.AddWithValue("@ProductName", updatedProduct.ProductName);
                    cmd.Parameters.AddWithValue("@Discription", updatedProduct.Discription);
                    cmd.Parameters.AddWithValue("@Brand", updatedProduct.Brand);
                    cmd.Parameters.AddWithValue("@Discount", updatedProduct.Discount);
                    cmd.Parameters.AddWithValue("@Price", updatedProduct.Price);
                    cmd.Parameters.AddWithValue("@Image", updatedProduct.Image);
                    cmd.Parameters.AddWithValue("@Type", updatedProduct.Type);
                    cmd.Parameters.AddWithValue("@BaoHanh", updatedProduct.BaoHanh);
                    cmd.Parameters.AddWithValue("@CPU", updatedProduct.CPU);
                    cmd.Parameters.AddWithValue("@RAM", updatedProduct.RAM);
                    cmd.Parameters.AddWithValue("@ManHinh", updatedProduct.ManHinh);
                    cmd.Parameters.AddWithValue("@HeDieuHanh", updatedProduct.HeDieuHanh);
                    cmd.Parameters.AddWithValue("@KhoiLuong", updatedProduct.KhoiLuong);
                    cmd.Parameters.AddWithValue("@CardDoHoa", updatedProduct.CardDoHoa);
                    cmd.Parameters.AddWithValue("@BanPhim", updatedProduct.BanPhim);
                    cmd.Parameters.AddWithValue("@MauSac", updatedProduct.MauSac);
                    cmd.Parameters.AddWithValue("@NhuCau", updatedProduct.NhuCau);
                    cmd.Parameters.AddWithValue("@LuuTru", updatedProduct.LuuTru);
                    cmd.Parameters.AddWithValue("@PhuKien", updatedProduct.PhuKien);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", updatedProduct.KieuKetNoi);

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
    }
}
