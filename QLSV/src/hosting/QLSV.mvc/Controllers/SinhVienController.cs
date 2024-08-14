using Microsoft.AspNetCore.Mvc;

namespace QLSV.mvc.Controllers
{
    public class SinhVienController : Controller {
        [HttpGet]
        public IActionResult Index(){
            return View();
        }
    }
}