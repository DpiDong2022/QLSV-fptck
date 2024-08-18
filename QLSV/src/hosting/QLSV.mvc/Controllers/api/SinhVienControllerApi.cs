using System.Diagnostics.Eventing.Reader;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QLSV.data.Helpers;
using QLSV.data.Services;
using QLSV.domain.entities;
using QLSV.domain.entities.DTO.response;

namespace QLSV.mvc.Controllers.api
{
    [ApiController]
    [Route("api/sinh-vien")]
    public class SinhVienControllerApi : ControllerBase{
        private readonly IDbService<SinhVien> _db;
        private readonly IMapper _mapper;
        public SinhVienControllerApi(IDbService<SinhVien> db){
            _db = db;
            _mapper = MapperHelper.InitializeAutoMapperSinhVien();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll(){
            try
            {
                var data = await _db.GetAll("GetAllSinhVienProcedure", new {});
                var dataDto = data.Select(sinhVien => _mapper.Map<SinhVienDto>(sinhVien));
                
                return new JsonResult(new ResData<IEnumerable<SinhVienDto>>(){
                    data = dataDto
                });
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResError(){
                    errors = new List<Error>()
                    {
                        new Error("server_error", e.InnerException != null ? e.InnerException.Message : e.Message)
                    }
                    // message = "Hệ thống đang trục trặc, vui lòng thử lại sau!"
                });
            }   
        }

        [HttpPost("datatable")]
        public async Task<IActionResult> GetDatatable([FromBody] DatatableRequest q){
            int totalRecords = await _db.Count("SELECT COUNT(*) FROM SinhVien");
            int totalRecordsFiltered = await _db.CountDatatableRecordsFiltered("CountDatatableRecordsFiltered", new {searchkey = q.search.value});
            var data = await _db.GetListDatatable("GetListDatatableProcedure", new {start=q.start, length = (q.length<=0 ? 10 : q.length), searchkey=q.search.value});
            var dataDto = data.Select(sinhVien => _mapper.Map<SinhVienDto>(sinhVien));

            return new JsonResult(
                new ResDatatable<SinhVienDto>(){
                    draw = q.draw,
                    data = dataDto,
                    recordsFiltered = totalRecordsFiltered,
                    recordsTotal = totalRecords
                }
            );
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
                return new JsonResult(new ResError(){
                    errors = new List<Error>()
                    {
                        new Error("server_error", e.InnerException != null ? e.InnerException.Message : e.Message)
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] SinhVien sinhVien){
            Thread.Sleep(600);
            try
            {
                List<Error> errors = new List<Error>();

                // Kiểm tra sdt và email đã tồn tại
                // đặt tên lỗi phải bằng giá trị name trong input của trường lỗi
                bool isPhoneNumberExisted = await IsPhoneNumberExisted(sinhVien);
                bool isEmailExisted = await IsEmailExisted(sinhVien);

                if(isPhoneNumberExisted) {
                    errors.Add(new Error("sdt", "Số điện thoại đã tồn tại"));
                }

                if(isEmailExisted) {
                    errors.Add(new Error("email", "Địa chỉ email đã tồn tại"));
                }

                if (errors.Count > 0) {
                    return new JsonResult(
                        new ResError(){errors = errors}
                    );
                }

                var id = await _db.Insert("InsertSinhVienProcedure", sinhVien);
                return new JsonResult(
                    new ResData<int> {
                        data = id
                    }
                );
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResError(){
                    errors = new List<Error>()
                    {
                        new Error("server_error", e.InnerException != null ? e.InnerException.Message : e.Message)
                    }
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SinhVien sinhVien){
            try
            {
                List<Error> errors = new List<Error>();

                // Kiểm tra sdt và email đã tồn tại
                // đặt tên lỗi phải bằng giá trị name trong input của trường lỗi
                bool isPhoneNumberExisted = await IsPhoneNumberExisted(sinhVien);
                bool isEmailExisted = await IsEmailExisted(sinhVien);

                if(isPhoneNumberExisted) {
                    errors.Add(new Error("sdt", "Số điện thoại đã tồn tại"));
                }

                if(isEmailExisted) {
                    errors.Add(new Error("email", "Địa chỉ email đã tồn tại"));
                }

                if (errors.Count > 0) {
                    return new JsonResult(
                        new ResError(){errors = errors}
                    );
                }

                var id = await _db.Update("UpdateSinhVienProcedure", sinhVien);
                return new JsonResult(
                    new ResBase()
                );
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResError(){
                    errors = new List<Error>()
                    {
                        new Error("server_error", e.InnerException != null ? e.InnerException.Message : e.Message)
                    }
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
                    return new JsonResult(new ResError(){
                        errors = new List<Error>()
                        {
                            new Error("not found entity", "Thông tin không tồn tại")
                        }
                    });
                }
            }
            catch (System.Exception e)
            {
                return new JsonResult(new ResError(){
                    errors = new List<Error>()
                    {
                        new Error("server_error", e.InnerException != null ? e.InnerException.Message : e.Message)
                    }
                });
            }
        }

        public async Task<bool> IsPhoneNumberExisted (SinhVien sinhVien) {
            var value = 0;
            if (sinhVien.id == 0) {
                value = await _db.QuerySingleOrDefault<int>("SELECT TOP 1 1 FROM SinhVien WHERE sdt LIKE @sdt", new {sdt = sinhVien.sdt});
            }
            else {
                value = await _db.QuerySingleOrDefault<int>("SELECT TOP 1 1 FROM SinhVien WHERE sdt LIKE @sdt AND id NOT LIKE @id", new {sdt = sinhVien.sdt, id = sinhVien.id});
            }
            return value == 1;
        }

        public async Task<bool> IsEmailExisted (SinhVien sinhVien) {
            var value = 0;
            if (sinhVien.id == 0) {
                value = await _db.QuerySingleOrDefault<int>("SELECT TOP 1 1 FROM SinhVien WHERE email LIKE @email", new {email = sinhVien.email});
            }
            else {
                value = await _db.QuerySingleOrDefault<int>("SELECT TOP 1 1 FROM SinhVien WHERE email LIKE @email AND id NOT LIKE @id", new {email = sinhVien.email, id = sinhVien.id});
            }
            return value == 1;
        }
    }
}