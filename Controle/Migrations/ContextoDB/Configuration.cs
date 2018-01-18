namespace Controle.Migrations.ContextoDB
{
    using Controle.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Controle.Models.ContextoDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ContextoDB";
        }

        protected override void Seed(Controle.Models.ContextoDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Usuarios.AddOrUpdate(
              p => p.Id,
              new UsuarioTarefasModel {Apelido = "Mateus",NomeConpleto = "Mateus dos santos", email= "mateus@gmail.com"},
              new UsuarioTarefasModel {Apelido = "Valeria",NomeConpleto = "Valeria cristina", email= "lela.cristina@gmail.com"},
              new UsuarioTarefasModel {Apelido = "B.A",NomeConpleto ="Brasilino Almeida costa", email= "BrasilinoaCosta@gmail.com"}
            );
            //
        }
    }
}
