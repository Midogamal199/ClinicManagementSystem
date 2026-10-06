using System;
using System.IO;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSystem.Infrastructure.Jobs
{
    public class DatabaseBackupJob : IDatabaseBackupJob
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseBackupJob> _logger;

        public DatabaseBackupJob(
            ApplicationDbContext context,
            ILogger<DatabaseBackupJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task BackupDatabaseAsync()
        {
            try
            {
                var databaseName = _context.Database.GetDbConnection().Database;

                
                var defaultBackupPath = await GetDefaultBackupPathAsync();

                var backupFileName = $"{databaseName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.bak";
                var backupFilePath = Path.Combine(defaultBackupPath, backupFileName);

              
                var safeDatabaseName = databaseName.Replace("]", "]]");
                var sql = $"BACKUP DATABASE [{safeDatabaseName}] TO DISK = @backupPath WITH FORMAT, INIT";

                await _context.Database.ExecuteSqlRawAsync(
                    sql,
                    new SqlParameter("@backupPath", backupFilePath));

                _logger.LogInformation("Database backup completed successfully: {BackupFile}", backupFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database backup failed.");
            }
        }

        private async Task<string> GetDefaultBackupPathAsync()
        {
            var result = await _context.Database
                .SqlQueryRaw<string>(
                    "SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS NVARCHAR(500)) AS [Value]")
                .FirstOrDefaultAsync();

            return !string.IsNullOrWhiteSpace(result) ? result : @"C:\Backups";
        }
    }
}