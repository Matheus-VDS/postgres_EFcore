using Microsoft.EntityFrameworkCore;

public class AgendaContext : DbContext
{
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Consulta> Consultas { get; set; }

    public AgendaContext(DbContextOptions<AgendaContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações de mapeamento para Paciente
        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(p => p.CPF);
            entity.Property(p => p.CPF)
                  .IsRequired()
                  .HasMaxLength(11);  // Tamanho máximo do CPF

            entity.Property(p => p.Nome)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.DataNascimento)
                  .IsRequired();

            entity.Property(p => p.Idade)
                  .IsRequired();
        });

        // Configurações de mapeamento para Consulta
        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(c => new { c.CPF, c.DataConsulta, c.HoraInicial });
            entity.Property(c => c.CPF).IsRequired();
            entity.Property(c => c.DataConsulta).IsRequired()
            .HasConversion(c => c.ToUniversalTime(), 
            c => DateTime.SpecifyKind(c, DateTimeKind.Utc))
            .HasColumnType("timestamp with time zone"); 
            entity.Property(c => c.HoraInicial).IsRequired();
            entity.Property(c => c.HoraFinal).IsRequired();

            // Relacionamento com Paciente
            entity.HasOne<Paciente>()
                  .WithMany()
                  .HasForeignKey(c => c.CPF);
        });
    }
}