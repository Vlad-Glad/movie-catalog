namespace CatalogInfrastructure.Services
{
    public interface IReportService
    {
        Task<MemoryStream> GenerateSiteStatisticsReportAsync(CancellationToken cancellationToken = default);
    }
}

