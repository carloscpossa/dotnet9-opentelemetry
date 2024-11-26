using Dapper;
using Microsoft.Data.SqlClient;

namespace CidadesApi.Repositorios;

public class CidadeRepositorio : ICidadeRepositorio
{
    private readonly string _connectionString;

    public CidadeRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BancoCidades") ?? string.Empty;
    }
    public async Task<IEnumerable<Cidade>> ObterCidadesAsync(string uf)
    {
        var sqlCidades = @"SELECT CodigoIBGE, NomeCidade, UF 
                           FROM Cidades
                           WHERE Uf = @Uf";
        
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Cidade>(sqlCidades, new { Uf = uf });
    }
}