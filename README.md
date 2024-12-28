<strong>Documentando o projeto</strong>
<br>
<br>
<strong>1- Criando o projeto:</strong>
    <br>
    a. dotnet new sln // Conectar todos os projetos relacionados em uma única solução.
    <br>
    b. dotnet new classlib -o Dima.Core //  Nao é um executavel, cria apenas uma dll. Serve para compartilhar informacões entre o backend e o frontend.
    <br>
    c. dotnet sln add ./Dima.Core/ // Para adicionar o Dima.Core ao sln da aplicacão.
    <br>

<strong>2- Criando a pasta Models no Dima.Core:</strong>
    <br>
    a. Category
    <br>
    b. Transaction
    <br>

<strong>3- Criando uma pasta de Emums no Dima.Core:</strong>
    <br>
    a. Adicionando a classe ETransactionType para armazenar os valores Withdraw e Deposit.
    <br>

<strong>4- Criando o Dima.Api:</strong>
    <br>
    a. dotnet new web -o Dima.Api. // Projeto web mais simples do .NET para criarmos a API.
    <br>
    b. dotnet sln add ./Dima.Api/. // Adicinando a API no sln do projeto.
    <br>
    c. dotnet add package Microsoft.AspNetCore.OpenApi // Suporte para criar APIs padronizadas.
    <br>
    d. dotnet add package Swashbuckle.AspNetCore // Utilizado para integrar o suporte ao Swagger e OpenAPI.
    <br>
    e. builder.Services.AddEndpointsApiExplorer(); // Adicionando o suporte para o OpenApi no Program.cs.
    <br>
    f. builder.Services.AddSwaggerGen(); // vai adicionar o suporte para uma aplicacao web para testar a nossa API.
    <br>
    g. Adicionamos ao builder.Services.AddSwaggerGen(x =>
                        {
                            x.CustomSchemaIds(n => n.FullName); // Usado para nao confundir o Swagger caso tenha mais de    classe ou funcao com o mesmo nome.
                        }); 
    <br>
    h. app.UseSwagger(); app.UseSwaggerUI(); // Vão gerar a interface web.