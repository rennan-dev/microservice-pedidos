using Estoque.Models;

namespace Estoque.Data;

public interface IEstoqueRepository {
    void SaveChanges();
    IEnumerable<Produto> GetAllProduto();
    Produto GetProdutoById(int id);
    void CreateProduto(Produto produto);



    //parte assíncrona - itemPedido e pedido
    IEnumerable<ItemPedido> GetAllItensPedido();
    ItemPedido GetItemPedidoById(int id);

    IEnumerable<PedidoCliente> GetAllPedidos();
    PedidoCliente GetPedidoById(int id);
    void CreatePedido(PedidoCliente pedido);
}