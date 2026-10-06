using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IDatabaseBackupJob
    {
        Task BackupDatabaseAsync();
    }
}