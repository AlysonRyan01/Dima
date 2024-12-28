Projeto de site completo

1- Criando o projeto:
    a. dotnet new sln // Conectar todos os projetos relacionados em uma única solução.
    b. dotnet new classlib -o Dima.Core //  Nao é um executavel, cria apenas uma dll. Serve para compartilhar informacões entre o backend e o frontend.
    c. dotnet sln add ./Dima.Core/ // Para adicionar o Dima.Core ao sln da aplicacão.

2- Criando a pasta Models no Dima.Core:
    a. Category
    b. Transaction

3- Criando uma pasta de Emums no Dima.Core:
    a. Adicionando a classe ETransactionType para armazenar os valores Withdraw e Deposit.

4- Criando o Dima.Api:
    a. dotnet new web -o Dima.Api. // Projeto web mais simples do .NET, para criarmos a API.
    b. dotnet sln add ./Dima.Api/. // Adicinando a API no sln do projeto.