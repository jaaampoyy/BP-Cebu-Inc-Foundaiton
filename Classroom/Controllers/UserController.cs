using Classroom.Models;
using Classroom.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Classroom.Controllers
{
    public class UserController : Controller
    {

        private readonly IConfiguration _configuration;
        private string? _connectionString;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")?.ToString();
        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT UserId, Username, LastLogin, Email FROM Users WHERE UserId = @userId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                { 
                    cmd.Parameters.AddWithValue("@userId", userId);

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        UserModel user = new UserModel();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserModel readUser = new UserModel
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    LastLogin = reader.GetNullableDateTime("LastLogin"),
                                    Email = reader.GetString(reader.GetOrdinal("Email"))
                                };

                                user = readUser;
                            }
                        }

                        query = "SELECT Id, UserId, RoleId, RoleType FROM UserRoles WHERE UserId = @userId";

                        using (SqlCommand roleCmd = new SqlCommand(query, conn))
                        {
                            roleCmd.Parameters.AddWithValue("@userId", userId);
                            List<UserRoleModel> userRoles = new List<UserRoleModel>();
                            using (SqlDataReader roleReader = roleCmd.ExecuteReader())
                            {
                                while (roleReader.Read())
                                {
                                    UserRoleModel userRole = new UserRoleModel
                                    {
                                        Id = roleReader.GetInt32(roleReader.GetOrdinal("Id")),
                                        UserId = roleReader.GetInt32(roleReader.GetOrdinal("UserId")),
                                        RoleId = roleReader.GetInt32(roleReader.GetOrdinal("RoleId")),
                                        RoleType = roleReader.GetString(roleReader.GetOrdinal("RoleType"))
                                    };
                                    userRoles.Add(userRole);
                                }
                            }
                            user.UserRoles = userRoles;
                        }
                        return View(user);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User not found.";
                    }
                }
            }

            return View();
        }

        public ActionResult List()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT UserId, Username, LastLogin, Email FROM Users";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Assuming you want to return a list of users
                        List<UserModel> users = new List<UserModel>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserModel user = new UserModel
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    LastLogin = reader.GetNullableDateTime("LastLogin"),
                                    Email = reader.GetString(reader.GetOrdinal("Email"))
                                };
                                users.Add(user);
                            }
                        }
                        return View(users);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No users found.";
                    }
                }
            }

            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                string username = collection["Username"].ToString();
                string password = collection["Password"].ToString();
                string confirmPassword = collection["PasswordConfirm"].ToString();
                string email = collection["Email"].ToString();
                bool isValid = false;

                if(password != confirmPassword)
                {
                    ViewBag.ErrorMessage = "Passwords do not match.";
                    return View();
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Users (Username, Password, Email) VALUES (@username, @password, @email)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@email", email);

                        int count = (int)cmd.ExecuteNonQuery();
                        isValid = count > 0;
                    }
                }

                if (isValid)
                {
                    return RedirectToAction("Index", "Home");
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the class. Please try again.\r\n" + ex.Message;
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
