using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FullCalendarMvc.Models;

namespace FullCalendarMvc.DAL
{
    public class FullCalendarMvcContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }

        public void Salvar()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}