using Microsoft.AspNetCore.Mvc;

namespace QLSV.mvc.Models.DTO.response
{
    public class ResBase {
        public string status { get; set; } = "OK";
        public string message { get; set; } = "";
    }

    public class ResData<T> : ResBase {
        public  T data { get; set; }
    }
}