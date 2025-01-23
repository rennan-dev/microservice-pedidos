# Microservice Pedidos

## Primeiro passo: atualizar os dois projetos
``` 
dotnet restore
```

```
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.2
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

```

## Segundo passo: atualizar database
terminal precisa estar em Estoque
```
dotnet ef migrations add UpdateProdutoSchema 
```
```
dotnet ef database update  
```
depois em Pedido
terminal precisa estar em Estoque
```
dotnet ef migrations add UpdatePedidoSchema 
```
```
dotnet ef database update  
```


## Terceiro passo: inicializar projetos
```
dotnet run
```

## Quarto passo: acessar links no navegador
Estoque
```
http://localhost:5222/swagger/index.html
```

Pedido
```
http://localhost:5047/swagger/index.html
```

# Visão Geral

Este projeto é composto por dois microsserviços que trabalham em conjunto para gerenciar pedidos e controlar o estoque de produtos. Os microsserviços são:

- **Microsserviço de Pedido**: Responsável pela gestão de pedidos e suas validações.
- **Microsserviço de Estoque**: Responsável pelo controle e atualização do estoque dos produtos.

## Regras de Negócio

### Microsserviço de Pedido

- **Estrutura do Pedido**: Um pedido pode conter vários itens, cada um associado a um produto.
- **Validação de Estoque**: Antes de confirmar um pedido, o sistema deve validar se há estoque disponível para todos os itens.

### Microsserviço de Estoque

- **Informações do Produto**: Cada produto no estoque deve ter as seguintes informações:
  - `ProductId`: Identificador único do produto.
  - `Nome`: Nome do produto.
  - `QuantidadeDisponivel`: Quantidade disponível em estoque.
  - `Preço`: Preço do produto.
- **Consulta de Disponibilidade**: O sistema deve permitir a consulta da disponibilidade dos produtos.
- **Atualização de Estoque**: Ao receber uma requisição de confirmação de pedido, o estoque deve reduzir a quantidade disponível do produto.
- **Validação de Reserva**: Se a quantidade de um item for insuficiente, o sistema não deve permitir a reserva.

## Tarefas Específicas

### Criação das Classes de Domínio

#### Microsserviço de Pedido:

- **Classe Pedido**: Representa o pedido, contendo os itens do pedido e o status do pedido.
- **Classe ItemPedido**: Representa cada item do pedido com informações sobre o produto e a quantidade.

#### Microsserviço de Estoque:

- **Classe Produto**: Representa cada produto disponível no estoque com suas respectivas informações.

### Validações e Relacionamentos

- **Definição de Relacionamentos**:
  - No microsserviço de Pedido, deve-se definir corretamente os relacionamentos entre as classes `Pedido` e `ItemPedido`.
  - No microsserviço de Estoque, o foco será no gerenciamento individual dos produtos.
