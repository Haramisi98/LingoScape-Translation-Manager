using LingoScape.DataAccessLayer.Models;
using LingoScape.Logic.Interfaces;
using LingoScape.Repository.Interfaces;

namespace LingoScape.Logic.Services
{
    public class MetadataService(IRepositoryBase<MetadataModel> _metadataRepository) : IMetadataService
    {
        public MetadataModel Create(MetadataModel metadata) => _metadataRepository.Create(metadata);

        public void Delete(int id) => _metadataRepository.Delete(id);

        public IEnumerable<MetadataModel> GetAll() => _metadataRepository.ReadAll();

        public MetadataModel GetById(int id) => _metadataRepository.Read(id);

        public MetadataModel Update(MetadataModel metadata) => _metadataRepository.Update(metadata);
    }
}
