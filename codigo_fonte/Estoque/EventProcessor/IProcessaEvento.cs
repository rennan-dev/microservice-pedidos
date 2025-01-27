namespace Estoque.EventProcessor;

public interface IProcessaEvento {
    public void Processa(string mensagem);
}