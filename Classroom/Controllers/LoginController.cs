using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Classroom.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        // This action method handles the login page.
        public IActionResult Login(IFormCollection frmc)
        {

            // With these lines:
            string username = frmc["username"].ToString();
            string password = frmc["password"].ToString();
            bool isValidUser = false;

            string connectionString = _configuration.GetConnectionString("DefaultConnection").ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int count = (int)cmd.ExecuteScalar();
                    isValidUser = count > 0;
                }
            }

            if (isValidUser)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View("Index");
        }
    }
}
