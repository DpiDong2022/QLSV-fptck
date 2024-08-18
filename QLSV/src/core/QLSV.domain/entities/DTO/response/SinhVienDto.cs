namespace QLSV.domain.entities.DTO.response
{
    public class SinhVienDto {
        public int id { get; set; }
        public string hoten { get; set; } = "";
        public string ngaysinh { get; set; } = "";
        public int gioitinh { get; set; }
        public string diachi { get; set; } = "";
        public string sdt { get; set; } = "";
        public string email { get; set; } = "";
        public string hinhanh_url { get; set; } = "";
    }
}