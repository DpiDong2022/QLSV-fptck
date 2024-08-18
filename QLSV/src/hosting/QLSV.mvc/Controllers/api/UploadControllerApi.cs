using Microsoft.AspNetCore.Mvc;
using QLSV.domain.entities.DTO.response;
using System.IO;
using System.Net.Http.Headers;

namespace QLSV.mvc.Controllers.api
{
    [ApiController]
    [Route("api/_upload")]
    public class UploadControllerApi : ControllerBase {

        private readonly IHostEnvironment _hostEnvironment;
        public UploadControllerApi(IHostEnvironment hostEnvironment) { 
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("image")] 
        public async Task<IActionResult> ImageUpload(IFormFile file) {

            try
            {
                string fileName = "";
                string rootPath = _hostEnvironment.ContentRootPath;
                string destinationPath = Path.Combine(rootPath, "wwwroot/images");

                if (!Directory.Exists(destinationPath)) {
                    return new JsonResult(
                        new ResError() {
                            errors = new List<Error> {
                                new Error("server_error", "Không tìm thấy thư mục để lưu ảnh")
                            }
                        }
                    ); 
                }

                fileName = generateUniqueFileName(file.FileName);

                using (FileStream stream = new FileStream(Path.Combine(destinationPath, fileName), FileMode.Create)) {
                    await file.CopyToAsync(stream);
                }

                return new JsonResult (
                    new ResData<string> {
                        data  = fileName
                    }
                );
            }
            catch (System.Exception e)
            {
                
                return new JsonResult(
                    new ResError(){
                        errors = new List<Error> {
                            new Error("server_error", e.InnerException == null ? e.Message : e.InnerException.Message)
                        }
                    }
                );
            }
        }

        private string generateUniqueFileName(string originalFileName) {
            string extension = Path.GetExtension(originalFileName);
            string fileName = $"{Guid.NewGuid()}{extension}";
            return fileName;
        }

    }
}