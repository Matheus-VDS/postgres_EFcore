using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class AgendaContextFactory : IDesignTimeDbContextFactory<AgendaContext>
{
    public AgendaContext CreateDbContext(string[] args)
    {
        // Obter a configuração do arquivo appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Configurar as opções do banco de dados
        var builder = new DbContextOptionsBuilder<AgendaContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(connectionString);

        // Criar e retornar o contexto
        return new AgendaContext(builder.Options);
    }
}