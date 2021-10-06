using FilmesAPI.Models;
using FilmesAPI.Data.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Endereco>()
                .HasOne(endereco => endereco.Cinema)
                .WithOne(cinema => cinema.Endereco)
                .HasForeignKey<Cinema>(cinema => cinema.EnderecoId);

            builder.Entity<Cinema>()
                .HasOne(cinema => cinema.Gerente)
                .WithMany(gerente => gerente.Cinemas)
                .HasForeignKey(cinema => cinema.GerenteId);
            //.OnDelete(DeleteBehavior.Restrict); Caso for deletar, fica restrica caso os dados vinculado a outra classe
            //.IsRequired(false); Caso não seja obrigatorio colocar o GerenteId como obrigatorio.
            // No modo cascata, se tentarmos deletar um recurso que é dependência de outro, todos os outros recursos que dependem desse serão excluídos também.
            // No modo restrito não conseguiremos efetuar a deleção.
        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Gerente> Gerentes { get; set; }
    }
}
