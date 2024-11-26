namespace CidadesApi.Repositorios;

public interface ICidadeRepositorio
{
    Task<IEnumerable<Cidade>> ObterCidadesAsync(string uf);
}