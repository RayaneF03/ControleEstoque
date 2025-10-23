using ControleEstoque.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //incluir os dbsets para as models do sistemas que irao se tornar tabelas no banco de dados
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Movimentacao> Movimentacao { get; set; }

        // sobrescrever o metodo onmodelcreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Configurações adicionais de mapeamento podem ser feitas aqui
        }
    }
}
