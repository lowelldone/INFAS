using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using INFAS.Data;
using INFAS.Models;
using System;
using System.Data;

namespace INFAS.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connStr;
        private readonly ApplicationDbContext _db;

        public UserController(IConfiguration config, ApplicationDbContext db)
        {
            _connStr = config.GetConnectionString("DefaultConnection");
            _db = db;
        }

        // GET: /User
        [HttpGet]
        public IActionResult Index() => View();

        // An attacker could supply username = " ' OR 1=1-- " to always succeed

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var sql = $"SELECT * FROM [Users] WHERE Username = '{username}' AND Password = '{password}'";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ViewBag.Error = "Invalid credentials.";
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    const string sql =
        //      "SELECT UserId, Username, Password " +
        //      "FROM [Users] " +
        //      "WHERE Username = @u AND Password = @p";

        //    using var conn = new SqlConnection(_connStr);
        //    using var cmd = new SqlCommand(sql, conn);
        //    cmd.Parameters.AddWithValue("@u", username);
        //    cmd.Parameters.AddWithValue("@p", password);

        //    conn.Open();

        //    using var reader = cmd.ExecuteReader();
        //    if (reader.Read())
        //        return Content("✅ Safe login: SUCCESS!");

        //    return Content("❌ Safe login: FAILED.");
        //}
    }
}
