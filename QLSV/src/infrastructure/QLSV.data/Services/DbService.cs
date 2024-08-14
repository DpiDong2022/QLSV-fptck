

using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using QLSV.domain.entities;

namespace QLSV.data.Services
{
    public class DbService<T> : IDbService<T> where T : class
    {
        private readonly IDbConnection _db;

        public DbService()
        {
            _db = new SqlConnection("Server=(localdb)\\mssqllocaldb; Database=QLSV_FPTS_ChungKhoan_demo; Trusted_Connection=True;");
        }

        public async Task<bool> Delete(string command, object parms)
        {
            int numberOfRowsAffected = await _db.ExecuteAsync(sql: command,param: parms);
            return numberOfRowsAffected >= 1;
        }

        public async Task<List<T>> GetAll(string command, object parms)
        {
            List<T> list = new List<T>();
            list = (await _db.QueryAsync<T>(sql: command, commandType: CommandType.StoredProcedure)).ToList();
            return list;
        }

        public async Task<T> GetById(string command, object parms)
        {   
            var value = await _db.QueryFirstOrDefaultAsync<T>(sql: command, param: parms, commandType: CommandType.StoredProcedure);
            return value;
        }

        public async Task<int> Insert(string command, object parms)
        {
            var id = await _db.QuerySingleAsync<int>(sql: command, param: parms, commandType: CommandType.StoredProcedure);
            return id;
        }

        public async Task<bool> Update(string command, object parms)
        {
            int numberOfRowsAffected = await _db.ExecuteAsync(sql: command, param: parms, commandType: CommandType.StoredProcedure);
            return numberOfRowsAffected >= 1;
        }
    }
}