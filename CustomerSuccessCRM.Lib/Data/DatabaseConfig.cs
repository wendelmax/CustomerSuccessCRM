using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CustomerSuccessCRM.Lib.Data
{
    public static class DatabaseConfig
    {
        public static string GetDatabasePath()
        {
            // Diretório base para o banco de dados
            string baseDir;

            // Em ambiente de desenvolvimento, usa o diretório da solução
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                baseDir = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..",
                    "Database"
                );
            }
            else
            {
                // Em produção, usa o diretório AppData do usuário
                baseDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CustomerSuccessCRM",
                    "Database"
                );
            }

            // Garante que o diretório existe
            Directory.CreateDirectory(baseDir);

            // Retorna o caminho completo do banco de dados
            return Path.Combine(baseDir, "crm.db");
        }

        public static void ConfigureDatabase(DbContextOptionsBuilder options)
        {
            var dbPath = GetDatabasePath();
            options.UseSqlite($"Data Source={dbPath}");
        }

        public static void EnsureDatabaseExists(CrmDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
} 