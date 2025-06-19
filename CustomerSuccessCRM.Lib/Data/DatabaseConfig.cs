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
            if (IsDevelopmentEnvironment())
            {
                // Tenta encontrar o diretório Database na raiz da solução
                var currentDir = Directory.GetCurrentDirectory();
                var solutionDir = FindSolutionDirectory(currentDir);
                
                if (solutionDir != null)
                {
                    baseDir = Path.Combine(solutionDir, "Database");
                }
                else
                {
                    // Fallback: usa o diretório atual
                    baseDir = Path.Combine(currentDir, "Database");
                }
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

        private static bool IsDevelopmentEnvironment()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return env == "Development" || env == null || env == "";
        }

        private static string? FindSolutionDirectory(string startDirectory)
        {
            var current = startDirectory;
            
            while (current != null && Directory.Exists(current))
            {
                // Procura por arquivos .sln
                var slnFiles = Directory.GetFiles(current, "*.sln");
                if (slnFiles.Length > 0)
                {
                    return current;
                }

                // Sobe um nível
                var parent = Directory.GetParent(current);
                current = parent?.FullName;
            }

            return null;
        }

        public static void ConfigureDatabase(DbContextOptionsBuilder options)
        {
            var dbPath = GetDatabasePath();
            var connectionString = $"Data Source={dbPath}";
            options.UseSqlite(connectionString);
        }

        public static void EnsureDatabaseExists(CrmDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                // Log do erro para debug
                Console.WriteLine($"Erro ao criar banco de dados: {ex.Message}");
                throw;
            }
        }
    }
} 