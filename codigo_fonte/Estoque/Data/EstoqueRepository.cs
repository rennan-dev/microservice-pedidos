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


    //parte assíncrona - ItemPedido e Pedido
    public IEnumerable<ItemPedido> GetAllItensPedido() {
        return _context.ItensPedido.ToList();
    }

    public ItemPedido GetItemPedidoById(int id) => _context.ItensPedido.FirstOrDefault(c => c.ItemPedidoKey == id);

    public IEnumerable<PedidoCliente> GetAllPedidos() {
        return _context.Pedidos.ToList();
    }

    public PedidoCliente GetPedidoById(int id) => _context.Pedidos.FirstOrDefault(c => c.PedidoKey == id);

    public void CreatePedido(PedidoCliente pedido) {
        if(pedido==null) {
            throw new ArgumentNullException(nameof(pedido));
        }
        _context.Pedidos.Add(pedido);
    }
}