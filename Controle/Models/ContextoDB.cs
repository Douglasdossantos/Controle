using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Controle.Models
{
    public class ContextoDB :DbContext
    {
        public ContextoDB() : base("DefaultConnection")
        {
        }

        public DbSet<UsuarioTarefasModel> Usuarios { get; set; }

        public DbSet<TimeModel> Times { get; set; }

        public DbSet<TarefaMode> Tarefas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<UsuarioTarefasModel>()
                .HasMany(h => h.Times)
                .WithMany(w => w.Usuarios)
                .Map(m =>
                {
                    m.MapLeftKey("UsuariosTarefasModelId");
                    m.MapRightKey("TimeModelId");
                    m.ToTable("UsuariosTimesModel");
                });

            base.OnModelCreating(modelBuilder);
        }

    }
}