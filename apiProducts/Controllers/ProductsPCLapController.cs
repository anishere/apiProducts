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
        [Route("GetLaptopsBrand")]
        public Response GetLaptops(int page = 1, int pageSize = 20, string brand = null)
        {
            List<ProductsPcLaptop> products = new List<ProductsPcLaptop>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsPCLapTop";
                if (!string.IsNullOrEmpty(brand))
                {
                    query += " WHERE Brand = @Brand";
                }
                query += " ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(brand))
                    {
                        cmd.Parameters.AddWithValue("@Brand", brand);
                    }
                    cmd.Parameters.AddWithValue("@StartIndex", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

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
                        product.Hot = Convert.ToString(reader["Hot"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        products.Add(product);
                    }
                }

                if (products.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Products found";
                    response.listproducts = products;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
                    response.listproducts = null;
                }

                return response;
            }
            catch (Exception ex)
            {
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
        [Route("GetAllProducts")]
        public Response GetAllProducts()
        {
            List<ProductsPcLaptop> products = new List<ProductsPcLaptop>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsPCLapTop ORDER BY ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
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
                        product.Hot = Convert.ToString(reader["Hot"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        products.Add(product);
                    }
                }

                Response response = new Response();
                if (products.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Products found";
                    response.listproducts = products;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
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
        [Route("SearchProducts")]
        public Response SearchProducts(string keyword)
        {
            List<ProductsPcLaptop> products = new List<ProductsPcLaptop>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsPCLapTop WHERE ProductName LIKE @Keyword ORDER BY ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

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
                        product.Hot = Convert.ToString(reader["Hot"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                        products.Add(product);
                    }
                }

                Response response = new Response();
                if (products.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Products found";
                    response.listproducts = products;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
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
        [Route("GetLaptops")]
        public Response GetLaptops(int page = 1, int pageSize = 20)
        {
            return GetProductsByType("lap", page, pageSize);
        }

        [HttpGet]
        [Route("GetPCs")]
        public Response GetDesktops(int page = 1, int pageSize = 20)
        {
            return GetProductsByType("pc", page, pageSize);
        }

        private Response GetProductsByType(string type, int page, int pageSize)
        {
            List<ProductsPcLaptop> products = new List<ProductsPcLaptop>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsPCLapTop WHERE Type = @Type ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@StartIndex", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

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
                        product.Hot = Convert.ToString(reader["Hot"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        products.Add(product);
                    }
                }

                Response response = new Response();
                if (products.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Products found";
                    response.listproducts = products;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
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
                        product.Hot = Convert.ToString(reader["Hot"]);
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
        [Route("HotProducts")]
        public Response GetHotProducts()
        {
            List<ProductsPcLaptop> hotProducts = new List<ProductsPcLaptop>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsPCLapTop WHERE Hot = 'hot'";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
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
                        product.Hot = Convert.ToString(reader["Hot"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        hotProducts.Add(product);
                    }
                }

                Response response = new Response();
                if (hotProducts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Hot products found";
                    response.listproducts = hotProducts;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No hot products found";
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
                            KieuKetNoi = Convert.ToString(row["KieuKetNoi"]),
                            Hot = Convert.ToString(row["Hot"]),
                            NgayNhap = Convert.ToDateTime(row["NgayNhap"])
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

        [HttpGet]
        [Route("ProductCountByBrand/{brand}")]
        public Response GetProductCountByBrand(string brand)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM ProductsPCLapTop WHERE Brand = @Brand";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Brand", brand);

                    int brandProductCount = Convert.ToInt32(cmd.ExecuteScalar());

                    response.StatusCode = 200;
                    response.StatusMessage = "Product count by brand found";
                    response.BrandProductCount = brandProductCount;
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
        [Route("GetBrands")]
        public Response GetBrands()
        {
            List<string> brands = new List<string>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT DISTINCT Brand FROM ProductsPCLapTop";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string brand = Convert.ToString(reader["Brand"]);
                        brands.Add(brand);
                    }
                }

                response.StatusCode = 200;
                response.StatusMessage = "Brands found";
                response.Brands = brands;
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

                string query = "INSERT INTO ProductsPCLapTop (ProductName, Description, Brand, Discount, Price, Image, Type, BaoHanh, CPU, RAM, ManHinh, PIN, HeDieuHanh, KhoiLuong, CardDoHoa, BanPhim, MauSac, NhuCau, LuuTru, PhuKien, KieuKetNoi, Hot, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @Image, @Type, @BaoHanh, @CPU, @RAM, @ManHinh, @PIN, @HeDieuHanh, @KhoiLuong, @CardDoHoa, @BanPhim, @MauSac, @NhuCau, @LuuTru, @PhuKien, @KieuKetNoi, @Hot, @NgayNhap)";

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
                    cmd.Parameters.AddWithValue("@Hot", obj.Hot);
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
                               "NhuCau = @NhuCau, LuuTru = @LuuTru, PhuKien = @PhuKien, KieuKetNoi = @KieuKetNoi, Hot = @Hot, NgayNhap = @NgayNhap " +
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
                    cmd.Parameters.AddWithValue("@Hot", updatedProduct.Hot);
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
