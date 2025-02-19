# Microservice Pedidos

Para rodar esses serviços, é necessário ter o Docker instalado em seu computador e garantir que ele esteja em execução enquanto você roda os comandos abaixo.

Eu disponibilizei duas formas de rodar o programa, sendo a primeira a mais simples, utilizando o Docker Compose para executar todos os serviços com um único comando. No entanto, a escolha fica a seu critério.

## Passos no docker-compose

No terminal, navegue até o diretório onde está localizado o arquivo docker-compose.yml deste projeto.

Em seguida, utilize o seguinte comando e aguarde até que os serviços Estoque e Pedido iniciem, pois eles são dependentes do **MySQL** e do **RabbitMQ**:

```
docker-compose up
```
## Para a visualização, cole os seguintes links no seu navegador
Estoque
```
http://localhost:5222/swagger/index.html
```
Pedido
```
http://localhost:5047/swagger/index.html
```
Para verificar se todos os quatro serviços estão em execução, utilize o comando abaixo. Caso algum ainda não tenha iniciado, aguarde um pouco mais e execute novamente:
```
docker ps
```
Para verificar se eles executaram mas pararam por motivos de erros, utilize o comando abaixo.
```
docker ps -a
```
Para cancelar a execução:
```
docker-compose down
```


## Passos no docker:

### Baixando dotnet
```
docker pull mcr.microsoft.com/dotnet/sdk:8.0
```

### Executar dentro da raiz do projeto em Estoque
```
docker build -t estoqueservice:1.1 .
```

### Executar dentro da raiz do projeto em Pedido
```
docker build -t pedidoservice:1.1 .
```

### Criando bridge para todos os contêineres ficarem agrupados
```
docker network create microsservice-bridge
```

### Executando o mensageiro RabbitMQ

```
docker run --name rabbitmq-service -d -p 15672:15672 -p 5672:5672 --network microsservice-bridge rabbitmq:3-management
```

### Executando o bando de dados 
```
docker run --name=mysql -e MYSQL_ROOT_PASSWORD=root -d --network microsservice-bridge mysql:5.6
```

### Executando os serviços 'Estoque' e 'Pedido'
```
docker run --name estoque -d -p 5222:8080 --network microsservice-bridge estoqueservice:1.1
```
```
docker run --name pedido -d -p 5047:8080 --network microsservice-bridge pedidoservice:1.1
```

## Verificar database(opcional)
```
docker exec -it mysql bash
mysql -u root -p
```

## Acessar links no navegador
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
