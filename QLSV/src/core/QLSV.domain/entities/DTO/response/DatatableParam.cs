namespace QLSV.domain.entities.DTO.response
{
    public class DatatableRequest {
        public int length {get; set;} 
        public int start {get; set;} 
        public int draw {get; set;} 
        public DatatableSearch search {get; set;} 
    }

    public class DatatableSearch {
        public string? value { get; set; } 
        public bool regex { get; set; }
    }
}