namespace QLSV.data.Services
{
    public interface IDbService<T> where T : class {
        Task<List<T>> GetAll(string command, object parms);
        Task<T> GetById(string command, object parms);
        Task<int> Insert(string command, object parms);
        Task<bool> Update(string command, object parms);
        Task<bool> Delete(string command, object parms);
    }
}