using System;
namespace QLSV.domain.entities
{
    public class SinhVien {
        public int id { get; set; }
        public string hoten { get; set; } = "";
        public DateTime ngaysinh { get; set; }
        public int gioitinh { get; set; }
        public string diachi { get; set; } = "";
        public string sdt { get; set; } = "";
        public string email { get; set; } = "";
    }
}