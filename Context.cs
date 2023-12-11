using AsientosContrablesApi.Models;
using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
  public Context(DbContextOptions<Context> options) : base(options)
  {

  }
  public DbSet<JournalEntryLines> JournalEntryLines { get; set; }
  // public DbSet<Account> Accounts { get; set; }
  public DbSet<AsientoContable> AsientosContables { get; set; }
  public DbSet<Registro> Registros { get; set; }
  public DbSet<Proceso> Procesos { get; set; }
}