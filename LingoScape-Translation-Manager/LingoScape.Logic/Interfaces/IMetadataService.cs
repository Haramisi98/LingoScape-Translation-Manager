using LingoScape.DataAccessLayer.Models;

namespace LingoScape.Logic.Interfaces
{
    public interface IMetadataService
    {
        IEnumerable<MetadataModel> GetAll();
        MetadataModel GetById(int id);
        MetadataModel Create(MetadataModel translatable);
        MetadataModel Update(MetadataModel translatable);
        void Delete(int id);
    }
}
