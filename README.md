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
    <br>

<strong>2- Criando a pasta Models no Dima.Core:</strong>
    <br>
    a. Category
    <br>
    b. Transaction
    <br>
    <br>

<strong>3- Criando uma pasta de Emums no Dima.Core:</strong>
    <br>
    a. Adicionando a classe ETransactionType para armazenar os valores Withdraw e Deposit.
    <br>
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
                            x.CustomSchemaIds(n => n.FullName); 
                        }); // Usado para nao confundir o Swagger caso tenha mais de    classe ou funcao com o mesmo nome.
    <br>
    h. app.UseSwagger(); app.UseSwaggerUI(); // Vão gerar a interface web.
    <br>
    i. Fizemos uma referencia do Dima.Api no Dima.Core.
    <br>
    j. Criando a Pasta Data e adicionando a classe AppDbContext nele;
    <br>
    k. dotnet add package Microsoft.EntityFrameWorkCore.SqlServer // Adicionando o pacote do SqlServer.
    <br>
    l. dotnet add package Microsoft.EntityFrameWorkCore.Design // Serve para criar migracoes.
    <br>
    m. public class AppDbContext : DbContext // Adicionando a heranca do DbContext no nosso AppDbContext, o DbContext representa o nosso banco de dados em memoria.
    <br>
    n. public DbSet<Category> Categories {get;set;} = null!;, public DbSet<Transaction> Transactions {get;set;} = null!; // Adicionando o DbSet de Category e Transaction no AppDbContext, cada DbSet representa uma tabela no banco de dados em memoria.
    <br>
    o. protected override onmodelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    } // Esse metodo vai ser acionado quando a gente estiver criando o banco de dados pela primeira vez, o modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly) serve para aplicar os Mappings que fizemos, como CategoryMap e TransactionMap. Ele vai varrer o Projeto todo e configurar os Mappings que estao herdando o IEntityTypeConfiguration<>.
    <br>
    p. Criando a pasta Mappings e adicionando o mapeamento de CategoryMap e TransactionMap com o FluentMap. Todos os Mappings como CategoryMap vao precisar herdar da interface IEntityTypeConfiguration<Category> e implementar o metodo public void Configure(EntityTypeBuilder<Category> builder), dentro desse metodo ficam as configuracoes da tabela.
    <br>
    q. Precisamos adicionar uma ConnectionStrings no appsettings.json e recuperar ela no Program.cs com o var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");.
    <br>
    r. builder.Services.AddDbContext<AppDbContext>(x =>
    {
        x.UseSqlServer(ConnectionString);
    }); // Configurando a nossa API para usar SQL SERVER e passando a connection string;
    <br>
    s. dotnet user-secrets init // Vai armazenar informacoes importantes que nao podem ser visualizados por pessoas, como a connection string. dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;" // Vai adicionar a nossa connection string no user-secrets.
    <br>
    t. dotnet tool install --global dotnet-ef // Vai instalar a ferramente necessaria para criarmos as migrations.
    <br>
    u. Agora so falta aplicar as migracoes com dotnet ef migration add v1, e dotnet ef database update.
    <br>
    <br>

<strong>5- Padronizando as requests e as responses no Dima.Core:</strong>
    <br>
    a. Criando as pastas Requests e Responses;
    <br>
    b. Criamos as classes abstratas chamada BaseRequest e a PagedRequest, para servir como base para as outras classes.
    <br>
    c. Criamos uma classe chamada Response na pasta Responses.
    <br>
    d. Criamos as classes abstratas chamada BaseResponse e a PagedResponse, para servir como base para as outras classes.
    <br>
    e. Criamos a classe estatica Configuration na raiz do Core para armazenar as configuracoes padroes.
    <br>
    f. Criamos uma pasta Handlers para definir os contratos com interfaces dos modelos, como ICategoryHandler.
    <br>
    <br>

<strong>6- Criando os handlers no Dima.Api utilizando os contratos das interfaces do Core:</strong>
    <br>
    a. Criamos o CateogoryHandler que herda de ICategoryHandler para cumprir todos os contratos, fazemos isso para todos os modelos, como TransactionHandler, etc.
    <br>
    <br>

<strong>7- Criando as endpoints utilizando os handlers:</strong>
    <br>
    a. Criamos uma pasta chamada Endpoints. // Essa pasta vai armazenar todas as configuracoes das endpoints.
    <br>
    b. Criamos uma interface IEndpoint, essa interface vai conter um metodo void estatico abstrato chamado Map, que precisa como parametro um app do tipo IEndpointRouteBuilder. // Esse IEndpointRouteBuilder que vai permitir mapear as rotas como app.MapGet(), app.MapPost(), app.MapPut() e app.MapDelete().
    <br>
    c. Criamos uma classe estatica chamada Endpoint, essa classe vai conter a funcao void estatica MapEndpoints. Essa funcao vai ser uma extensao de WebApplication (this WebApplication app). Apos criar a classe precisamos adicionar app.MapEndpoints() no Program.cs. // Essa funcao vai ser a responsável por mapear os grupos de endpoints.
    <br>
    d. Para cada modelo que precisa de Endpoints, criamos uma pasta, como Categories e Transactions.
    <br>
    e. Dentro de cada pasta de algum modelo específico, criamos os Endpoints para mapear as rotas utilizando algum Handler, como CreateCategoryEndpoint, DeleteCategoryEndpoint, etc.
    <br>
    f. Essas classes herdam da interface IEndpoint, entao obviamente precisamos adicionar a funcao Map(IendpointRouteBuilder app). Dentro dessa funcao fazemos o mapeamento, como app.MapPost("/", HandlerAsync).
    <br>
    g. O HandleAsync também é criado dentro dessa funcao, ele é uma funcao estatica async que retorna uma Task<IResult> e possui 2 parametros, a interface de algum handler e um request.


