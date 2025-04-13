using CatalogDomain.Model;

namespace CatalogInfrastructure.Services
{
    public class MovieDataPortServiceFactory : IDataPortServiceFactory<Movie>
    {
        private readonly DbCatalogContext _dbCatalogContext;

        public MovieDataPortServiceFactory(DbCatalogContext dbCatalogContext)
        {
            _dbCatalogContext = dbCatalogContext;
        }

        public IImportService<Movie> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new MovieImportService(_dbCatalogContext);
            }

            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");

        }

        public IExportService<Movie> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new MovieExportService(_dbCatalogContext);
            }

            throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");

        }
    }
}
