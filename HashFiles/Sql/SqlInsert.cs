using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HashFiles
{
    public class SqlInsert : ISqlInsert
    {
        private string _connectionString;

        public SqlInsert(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Вставка ошибок
        /// </summary>
        /// <param name="exception"></param>
        public async void InsertException(EntityException exception)
        {
            var query = $@"INSERT INTO ExceptionTable
                        VALUES ('{exception.Class}', '{exception.Method}', '{Normalize(exception.ExceptionMessage)}', '{exception.Date}' )";

            await InsertCommand(query);
        }

        /// <summary>
        /// Вставка сообщений
        /// </summary>
        /// <param name="message"></param>
        public async void InsertFileHash(EntityMessage message)
        {
            var query = $@"INSERT INTO FileHashTable
                        VALUES ('{Normalize(message.FileName)}', '{message.Hash}', '{message.Date}')";
            await InsertCommand(query);
        }

        private async Task InsertCommand(string query)
        {
            
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var sqlCommand = new SqlCommand(query, connection))
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Экранирование апострофов
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Normalize(string value)
        {
            return value.Replace("'", "''");
        }
    }
}
