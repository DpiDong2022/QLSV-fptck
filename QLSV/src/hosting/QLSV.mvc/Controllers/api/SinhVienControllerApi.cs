using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using QLSV.data.Services;
using QLSV.mvc.Models;
using QLSV.mvc.Models.DTO.response;

namespace QLSV.mvc.Controllers.api
{
    [ApiController]
    [Route("api/sinh-vien")]
    public class SinhVienControllerApi : ControllerBase{
        private readonly IDbService<SinhVien> _db;
        public SinhVienControllerApi(IDbService<SinhVien> db){
            _db = db;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll(){
            try
            {
                var data = await _db.GetAll("GetAllSinhVienProcedure", new {});
                return new JsonResult(new ResData<List<SinhVien>>(){
                    data = data
                });
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResBase(){
                    status = "ERROR",
                    message = e.InnerException != null ? e.InnerException.Message : e.Message
                    // message = "Hệ thống đang trục trặc, vui lòng thử lại sau!"
                });
            }   
        }

        [HttpPost("getById")]
        public async Task<IActionResult> GetById([FromBody] int id){
            try
            {
                var data = await _db.GetById("GetSinhVienByIdProcedure", new {id = id});
                return new JsonResult(
                    new ResData<SinhVien> {
                        data = data
                    }
                );
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResBase(){
                    status = "ERROR",
                    message = e.InnerException != null ? e.InnerException.Message : e.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] SinhVien sinhVien){
            Thread.Sleep(1000);
            try
            {
                var id = await _db.Insert("InsertSinhVienProcedure", sinhVien);
                return new JsonResult(
                    new ResData<int> {
                        data = id
                    }
                );
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResBase(){
                    status = "ERROR",
                    message = e.InnerException != null ? e.InnerException.Message : e.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SinhVien sinhVien){
            try
            {
                var id = await _db.Update("UpdateSinhVienProcedure", sinhVien);
                return new JsonResult(
                    new ResBase()
                );
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResBase(){
                    status = "ERROR",
                    message = e.InnerException != null ? e.InnerException.Message : e.Message
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id){
            try
            {
                bool isSuccessed = await _db.Delete("DELETE FROM sinhvien WHERE id = @id", new {id = id});
                if(isSuccessed) {
                    return new JsonResult(new ResBase());
                }
                else {
                    return new JsonResult(new ResBase(){ status= "ERROR", message= "Thông tin không tồn tại!" });
                }
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResBase(){
                    status = "ERROR",
                    message = e.InnerException != null ? e.InnerException.Message : e.Message
                });
            }
        }
    }
}