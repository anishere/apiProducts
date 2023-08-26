using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsPCLapController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsPCLapController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ListLapPc")]
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
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.CPU = Convert.ToString(dt.Rows[i]["CPU"]);
                    products.RAM = Convert.ToString(dt.Rows[i]["RAM"]);
                    products.ManHinh = Convert.ToString(dt.Rows[i]["ManHinh"]);
                    products.PIN = Convert.ToString(dt.Rows[i]["PIN"]);
                    products.HeDieuHanh = Convert.ToString(dt.Rows[i]["HeDieuHanh"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.CardDoHoa = Convert.ToString(dt.Rows[i]["CardDoHoa"]);
                    products.BanPhim = Convert.ToString(dt.Rows[i]["BanPhim"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.LuuTru = Convert.ToString(dt.Rows[i]["LuuTru"]);
                    products.PhuKien = Convert.ToString(dt.Rows[i]["PhuKien"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
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
        [Route("LatestLapPc")]
        public Response GetLatestProducts(int count = 5)
        {
            List<ProductsPcLaptop> latestProducts = new List<ProductsPcLaptop>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "SELECT TOP(@Count) * FROM ProductsPCLapTop ORDER BY NgayNhap DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Count", count);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductsPcLaptop product = new ProductsPcLaptop();
                        product.ProductID = Convert.ToInt32(reader["ProductID"]);
                        product.ProductName = Convert.ToString(reader["ProductName"]);
                        product.Description = Convert.ToString(reader["Description"]);
                        product.Brand = Convert.ToString(reader["Brand"]);
                        product.Discount = Convert.ToDecimal(reader["Discount"]);
                        product.Price = Convert.ToDecimal(reader["Price"]);
                        product.Image = Convert.ToString(reader["Image"]);
                        product.Type = Convert.ToString(reader["Type"]);
                        product.BaoHanh = Convert.ToString(reader["BaoHanh"]);
                        product.CPU = Convert.ToString(reader["CPU"]);
                        product.RAM = Convert.ToString(reader["RAM"]);
                        product.ManHinh = Convert.ToString(reader["ManHinh"]);
                        product.PIN = Convert.ToString(reader["PIN"]);
                        product.HeDieuHanh = Convert.ToString(reader["HeDieuHanh"]);
                        product.KhoiLuong = Convert.ToString(reader["KhoiLuong"]);
                        product.CardDoHoa = Convert.ToString(reader["CardDoHoa"]);
                        product.BanPhim = Convert.ToString(reader["BanPhim"]);
                        product.MauSac = Convert.ToString(reader["MauSac"]);
                        product.NhuCau = Convert.ToString(reader["NhuCau"]);
                        product.LuuTru = Convert.ToString(reader["LuuTru"]);
                        product.PhuKien = Convert.ToString(reader["PhuKien"]);
                        product.KieuKetNoi = Convert.ToString(reader["KieuKetNoi"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        latestProducts.Add(product);
                    }
                }

                Response response = new Response();
                if (latestProducts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Latest products found";
                    response.listproducts = latestProducts;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No latest products found";
                    response.listproducts = null;
                }

                return response;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                response.listproducts = null;
                return response;
            }
            finally
            {
                connection.Close();
            }
        }

        [HttpGet]
        [Route("GetLapPcById/{id}")]
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
                            Description = Convert.ToString(row["Description"]),
                            Brand = Convert.ToString(row["Brand"]),
                            Discount = Convert.ToDecimal(row["Discount"]),
                            Price = Convert.ToDecimal(row["Price"]),
                            Image = Convert.ToString(row["Image"]),
                            Type = Convert.ToString(row["Type"]),
                            BaoHanh = Convert.ToString(row["BaoHanh"]),
                            CPU = Convert.ToString(row["CPU"]),
                            RAM = Convert.ToString(row["RAM"]),
                            ManHinh = Convert.ToString(row["ManHinh"]),
                            PIN = Convert.ToString(row["PIN"]),
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

        [HttpGet]
        [Route("TotalCount")]
        public Response GetTotalProductCount()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM ProductsPCLapTop";

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
        [Route("AddLapPc")]

        public Response AddProduct(ProductsPcLaptop obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsPCLapTop (ProductName, Description, Brand, Discount, Price, Image, Type, BaoHanh, CPU, RAM, ManHinh, PIN, HeDieuHanh, KhoiLuong, CardDoHoa, BanPhim, MauSac, NhuCau, LuuTru, PhuKien, KieuKetNoi, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @Image, @Type, @BaoHanh, @CPU, @RAM, @ManHinh, @PIN, @HeDieuHanh, @KhoiLuong, @CardDoHoa, @BanPhim, @MauSac, @NhuCau, @LuuTru, @PhuKien, @KieuKetNoi, @NgayNhap)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductName", obj.ProductName);
                    cmd.Parameters.AddWithValue("@Description", obj.Description);
                    cmd.Parameters.AddWithValue("@Brand", obj.Brand);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@Image", obj.Image);
                    cmd.Parameters.AddWithValue("@Type", obj.Type);
                    cmd.Parameters.AddWithValue("@BaoHanh", obj.BaoHanh);
                    cmd.Parameters.AddWithValue("@CPU", obj.CPU);
                    cmd.Parameters.AddWithValue("@RAM", obj.RAM);
                    cmd.Parameters.AddWithValue("@ManHinh", obj.ManHinh);
                    cmd.Parameters.AddWithValue("@PIN", obj.PIN);
                    cmd.Parameters.AddWithValue("@HeDieuHanh", obj.HeDieuHanh);
                    cmd.Parameters.AddWithValue("@KhoiLuong", obj.KhoiLuong);
                    cmd.Parameters.AddWithValue("@CardDoHoa", obj.CardDoHoa);
                    cmd.Parameters.AddWithValue("@BanPhim", obj.BanPhim);
                    cmd.Parameters.AddWithValue("@MauSac", obj.MauSac);
                    cmd.Parameters.AddWithValue("@NhuCau", obj.NhuCau);
                    cmd.Parameters.AddWithValue("@LuuTru", obj.LuuTru);
                    cmd.Parameters.AddWithValue("@PhuKien", obj.PhuKien);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", obj.KieuKetNoi);
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

        [HttpDelete]
        [Route("DeleteLapPc/{id}")]
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
        [Route("UpdateLapPc/{id}")]
        public Response UpdateProduct(int id, ProductsPcLaptop updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsPCLapTop " +
                               "SET ProductName = @ProductName, Description = @Description, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, Image = @Image, Type = @Type, " +
                               "BaoHanh = @BaoHanh, CPU = @CPU, RAM = @RAM, " +
                               "ManHinh = @ManHinh, PIN = @PIN, HeDieuHanh = @HeDieuHanh, KhoiLuong = @KhoiLuong, " +
                               "CardDoHoa = @CardDoHoa, BanPhim = @BanPhim, MauSac = @MauSac, " +
                               "NhuCau = @NhuCau, LuuTru = @LuuTru, PhuKien = @PhuKien, KieuKetNoi = @KieuKetNoi, NgayNhap = @NgayNhap " +
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
                    cmd.Parameters.AddWithValue("@Type", updatedProduct.Type);
                    cmd.Parameters.AddWithValue("@BaoHanh", updatedProduct.BaoHanh);
                    cmd.Parameters.AddWithValue("@CPU", updatedProduct.CPU);
                    cmd.Parameters.AddWithValue("@RAM", updatedProduct.RAM);
                    cmd.Parameters.AddWithValue("@ManHinh", updatedProduct.ManHinh);
                    cmd.Parameters.AddWithValue("@PIN", updatedProduct.PIN);
                    cmd.Parameters.AddWithValue("@HeDieuHanh", updatedProduct.HeDieuHanh);
                    cmd.Parameters.AddWithValue("@KhoiLuong", updatedProduct.KhoiLuong);
                    cmd.Parameters.AddWithValue("@CardDoHoa", updatedProduct.CardDoHoa);
                    cmd.Parameters.AddWithValue("@BanPhim", updatedProduct.BanPhim);
                    cmd.Parameters.AddWithValue("@MauSac", updatedProduct.MauSac);
                    cmd.Parameters.AddWithValue("@NhuCau", updatedProduct.NhuCau);
                    cmd.Parameters.AddWithValue("@LuuTru", updatedProduct.LuuTru);
                    cmd.Parameters.AddWithValue("@PhuKien", updatedProduct.PhuKien);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", updatedProduct.KieuKetNoi);
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
    }
}
