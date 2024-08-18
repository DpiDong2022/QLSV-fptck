using AutoMapper;
using QLSV.domain.entities;
using QLSV.domain.entities.DTO.response;

namespace QLSV.data.Helpers {

    public class MapperHelper {

        static public Mapper InitializeAutoMapperSinhVien (){

            var configMapper = new MapperConfiguration(config => {
                config.CreateMap<SinhVien, SinhVienDto>().ForMember(dest => dest.ngaysinh, act => act.MapFrom(src => src.ngaysinh.ToString("dd-MM-yyyy")));
            });

            return new Mapper(configMapper);
        }
    }
}