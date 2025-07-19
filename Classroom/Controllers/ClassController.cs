using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Classroom.Controllers
{
    public class ClassController : Controller
    {
        private readonly IConfiguration _configuration;
        public ClassController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: ClassController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClassController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClassController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClassController/Create
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                string subject = collection["Subject"].ToString();
                string room = collection["Room"].ToString();
                string schedule = collection["Schedule"].ToString();
                int? teacher = 1;
                bool isValidUser = false;

                string? connectionString = _configuration.GetConnectionString("DefaultConnection")?.ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Classes (Subject, Room, Schedule, TeacherId) VALUES (@subject, @room, @schedule, @teacherId)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@subject", subject);
                        cmd.Parameters.AddWithValue("@room", room);
                        cmd.Parameters.AddWithValue("@schedule", schedule);
                        cmd.Parameters.AddWithValue("@teacherId", teacher);

                        int count = (int)cmd.ExecuteNonQuery();
                        isValidUser = count > 0;
                    }
                }

                if (isValidUser)
                {
                    return RedirectToAction("Index", "Home");
                }


                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the class. Please try again.\r\n" + ex.Message;
                return View();
            }
        }

        // GET: ClassController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClassController/Edit/5
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

        // GET: ClassController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClassController/Delete/5
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
