namespace QLSV.data.Services
{
    public interface IDbService<T> where T : class {
        Task<List<T>> GetAll(string command, object parms);
        Task<List<T>> GetListDatatable(string command, object parms);
        Task<int> CountDatatableRecordsFiltered(string command, object parms);
        Task<T> GetById(string command, object parms);
        Task<int> Insert(string command, object parms);
        Task<bool> Update(string command, object parms);
        Task<bool> Delete(string command, object parms);
        Task<int> Count(string command);
        Task<T1> QuerySingleOrDefault<T1>(string command, object parms);
    }
}