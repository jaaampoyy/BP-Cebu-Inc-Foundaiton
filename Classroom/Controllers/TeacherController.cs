using Classroom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Classroom.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IConfiguration _configuration;
        private string? connectionString;
        public TeacherController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection")?.ToString();
        }

        // GET: TeacherController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TeacherController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult List()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TeacherId, Firstname, Lastname, Email FROM Teachers";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Assuming you want to return a list of teachers
                        List<TeacherModel> teachers = new List<TeacherModel>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TeacherModel teacher = new TeacherModel
                                {
                                    TeacherId = reader.GetInt32(0),
                                    Firstname = reader.GetString(1),
                                    Lastname = reader.GetString(2),
                                    Email = reader.GetString(3)
                                };
                                teachers.Add(teacher);
                            }
                        }
                        return View(teachers);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No teachers found.";
                    }
                }
            }

            return View();
        }

        // GET: TeacherController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeacherController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                string firstname = collection["Firstname"].ToString();
                string lastname = collection["Lastname"].ToString();
                string email = collection["Email"].ToString();
                bool isValid = false;

                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Teachers (Firstname, Lastname, Email) VALUES (@firstname, @lastname, @email)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@lastname", lastname);
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

        // GET: TeacherController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeacherController/Edit/5
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

        // GET: TeacherController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeacherController/Delete/5
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
