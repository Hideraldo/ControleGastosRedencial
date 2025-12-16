using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace ControleGastosRedencial.Server.Models
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Executa uma consulta SQL arbitrária e mapeia resultados para o tipo informado.
        /// </summary>
        public static async Task<List<T>?> SqlQueryAsync<T>(this DbContext db, string sql, object[]? parameters = null, CancellationToken cancellationToken = default) where T : class
        {
            if (parameters is null)
            {
                parameters = Array.Empty<object>();
            }

            if (typeof(T).GetProperties().Any())
            {
                await using (var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection(), db.Database.CurrentTransaction))
                {
                    db2.Database.SetCommandTimeout(db.Database.GetCommandTimeout());
                    return await db2.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
                }
            }
            else
            {
                await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
                return default;
            }
        }

        private class ContextForQueryType<T> : DbContext where T : class
        {
            private readonly DbConnection _connection;
            private readonly IDbContextTransaction? _transaction;

            /// <summary>
            /// Constrói um contexto temporário para execução de consultas sem chave.
            /// </summary>
            public ContextForQueryType(DbConnection connection, IDbContextTransaction? transaction)
            {
                _connection = connection;
                _transaction = transaction;

                if (transaction != null)
                {
                    this.Database.UseTransaction((transaction as IInfrastructure<DbTransaction>)?.Instance);
                }
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(_connection);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasNoKey().ToView(null);
            }
        }
    }

    public class OutputParameter<TValue>
    {
        private bool _valueSet = false;
        private TValue? _value;

        /// <summary>
        /// Valor do parâmetro de saída (lança exceção se não definido).
        /// </summary>
        public TValue? Value
        {
            get
            {
                if (!_valueSet)
                    throw new InvalidOperationException("Value not set.");

                return _value;
            }
        }

        /// <summary>
        /// Define o valor a partir de um objeto retornado.
        /// </summary>
        internal void SetValue(object? value)
        {
            _valueSet = true;
            _value = value == null || Convert.IsDBNull(value) ? default : (TValue?)value;
        }
    }
}
