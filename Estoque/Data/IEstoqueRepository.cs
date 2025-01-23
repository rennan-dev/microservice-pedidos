using Estoque.Models;

namespace Estoque.Data;

public interface IEstoqueRepository {
    void SaveChanges();
    IEnumerable<Produto> GetAllProduto();
    Produto GetProdutoById(int id);
    void CreateProduto(Produto produto);
}