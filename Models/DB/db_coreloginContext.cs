using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using static SistemaCadastro.Pages.PageModelExtensions;

namespace SistemaCadastro.Models.DB
{
    public partial class db_coreloginContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;
        private AddItemInSessionJson _addItemInSessionJson;

        public db_coreloginContext()
        {

        }

        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Item> Item { get; set; }

        public db_coreloginContext(DbContextOptions<db_coreloginContext> options, AddItemInSessionJson addItemInSessionJson)
            : base(options)
        {
            _addItemInSessionJson = addItemInSessionJson;
        }

        ////protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        ////{
        ////    if (!optionsBuilder.IsConfigured)
        ////    {
        ////        optionsBuilder.UseSqlServer("Server=SQL SERVER;Database=DATABASE;User id=SQL USERNAME;Password=SQL PASSWORD;Trusted_Connection=True;");
        ////    }
        ////}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////modelBuilder.Entity<Login>(entity =>
            ////{
            ////    entity.Property(e => e.Id).HasColumnName("id");

            ////    entity.Property(e => e.Password)
            ////        .IsRequired()
            ////        .HasColumnName("password")
            ////        .HasMaxLength(50)
            ////        .IsUnicode(false);

            ////    entity.Property(e => e.Username)
            ////        .IsRequired()
            ////        .HasColumnName("username")
            ////        .HasMaxLength(50)
            ////        .IsUnicode(false);
            ////});

            // [Asma Khalid]: Query for store procedure.
            modelBuilder.Query<LoginByUsernamePassword>();
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #region Login by username and password store procedure method.

        /// <summary>
        /// Login by username and password store procedure method.
        /// </summary>
        /// <param name="usernameVal">Username value parameter</param>
        /// <param name="passwordVal">Password value parameter</param>
        /// <returns>Returns - List of logins by username and password</returns>
        public async Task<List<LoginByUsernamePassword>> LoginByUsernamePasswordMethodAsync(string usernameVal, string passwordVal)
        {
            // Initialization.
            List<LoginByUsernamePassword> lst = new List<LoginByUsernamePassword>();

            try
            {
                // Settings.
                SqlParameter usernameParam = new SqlParameter("@username", usernameVal ?? (object)DBNull.Value);
                SqlParameter passwordParam = new SqlParameter("@password", passwordVal ?? (object)DBNull.Value);

                // Processing.
                string sqlQuery = "SELECT * FROM Login WHERE Username = @username AND Password = @password";

                lst = await this.Query<LoginByUsernamePassword>().FromSql(sqlQuery, usernameParam, passwordParam).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Guardar item na session 
            _addItemInSessionJson.InsertItemInSessionJson(lst[0], "UsuarioLogin");


            // Info.
            return lst;
        }

        /// <summary>
        /// Login by username and password store procedure method.
        /// </summary>
        /// <param name="usernameVal">Username value parameter</param>
        /// <returns>Returns - List of logins by username and password</returns>
        public async Task<LoginByUsernamePassword> LoginByUsernameMethodAsync(string usernameVal)
        {
            // Initialization.
            List<LoginByUsernamePassword> lst = new List<LoginByUsernamePassword>();

            try
            {
                // Settings.
                SqlParameter usernameParam = new SqlParameter("@username", usernameVal ?? (object)DBNull.Value);

                // Processing.
                string sqlQuery = "SELECT * FROM Login WHERE Username = @username";

                lst = await this.Query<LoginByUsernamePassword>().FromSql(sqlQuery, usernameParam).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.
            return lst[0];
        }
        #endregion
    }
}
