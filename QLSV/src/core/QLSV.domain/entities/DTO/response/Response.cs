using System.Collections.Generic;

namespace QLSV.domain.entities.DTO.response
{
    public class ResBase {
        public string status { get; set; } = "OK";
    }

    public class ResData<T> : ResBase {
        public  T data { get; set; }
    }
    
    public class ResError : ResBase {
        public ResError() : base() { status = "ERROR"; }
        public List<Error> errors { get; set; }
    }

    public class ResDatatable<T> : ResBase {
        public IEnumerable<T> data { get; set;}
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

    }

    public class Error {
        public Error(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string key { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;

        
    }
}