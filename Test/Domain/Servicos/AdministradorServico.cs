using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Test.Domain.Entidades;

[TestClass]
internal class AdministradorServicoTest
{
    private DbContexto CriarContextodeTeste()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var builder = new ConfigurationBuilder()
            .SetBasePath(path?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        var configuration = builder.Build();

        return new DbContexto(configuration);
    }

    [TestMethod]
    public void TestandoSalvarAdministrador()
    {
        // Arrange - todas as classes que vamos criar para fazer a validação
        var context = CriarContextodeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

        var adm = new Administrador();
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";

        var administradorServico = new AdministradorServico(context);

        // Act - Ação que vamos executar
        administradorServico.Incluir(adm);

        //Assert - Validação desses dados
        Assert.AreEqual(1, administradorServico.Todos(1).Count());
    }
}