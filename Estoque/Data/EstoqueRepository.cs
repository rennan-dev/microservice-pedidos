using Estoque.Models;

namespace Estoque.Data;

public class EstoqueRepository : IEstoqueRepository {

    private readonly EstoqueContext _context;
    public EstoqueRepository(EstoqueContext context) {
        _context = context;
    }

    public void SaveChanges() {
        _context.SaveChanges();
    }

    public void CreateProduto(Produto produto) {
        if(produto==null) {
            throw new ArgumentNullException(nameof(produto));
        }
        _context.Produtos.Add(produto);
    }

    public IEnumerable<Produto> GetAllProduto() {
        return _context.Produtos.ToList();
    }
    public Produto GetProdutoById(int id) => _context.Produtos.FirstOrDefault(c => c.ProductId == id);
}