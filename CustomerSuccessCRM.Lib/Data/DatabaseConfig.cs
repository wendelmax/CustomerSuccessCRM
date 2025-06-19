using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CustomerSuccessCRM.Lib.Data
{
    public static class DatabaseConfig
    {
        public static string GetDatabasePath()
        {
            // Para simplificar, vamos usar sempre o diretório Database na raiz
            var solutionDir = FindSolutionDirectory(Directory.GetCurrentDirectory());
            var baseDir = solutionDir != null 
                ? Path.Combine(solutionDir, "Database")
                : Path.Combine(Directory.GetCurrentDirectory(), "Database");

            // Garante que o diretório existe
            Directory.CreateDirectory(baseDir);

            // Retorna o caminho completo do banco de dados
            return Path.Combine(baseDir, "crm.db");
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
            // Para teste, vamos usar uma string de conexão hardcoded
            var connectionString = "Data Source=\"crm.db\"";
            
            Console.WriteLine($"String de conexão: {connectionString}");
            Console.WriteLine($"Diretório atual: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"Bytes da string: {string.Join(", ", System.Text.Encoding.UTF8.GetBytes(connectionString))}");
            
            options.UseSqlite(connectionString);
        }

        public static void EnsureDatabaseExists(CrmDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                Console.WriteLine("Banco de dados criado com sucesso!");
            }
            catch (Exception ex)
            {
                // Log do erro para debug
                Console.WriteLine($"Erro ao criar banco de dados: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
} 