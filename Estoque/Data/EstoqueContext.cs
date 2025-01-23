using Estoque.Models;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Data;

public class EstoqueContext : DbContext {
    public EstoqueContext(DbContextOptions<EstoqueContext> opts) : base(opts) {

    }

    public DbSet<Produto> Produtos { get; set; }
}