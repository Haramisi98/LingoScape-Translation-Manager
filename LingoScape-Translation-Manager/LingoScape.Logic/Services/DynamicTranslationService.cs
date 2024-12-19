using LingoScape.DataAccessLayer.Models;
using LingoScape.Logic.Interfaces;
using LingoScape.Repository.Interfaces;

namespace LingoScape.Logic.Services
{
    public class DynamicTranslationService(IRepositoryBase<DynamicTranslationModel> _dynamicTranslationRepository) : IDynamicTranslationService
    {
        public DynamicTranslationModel Create(DynamicTranslationModel dynamicTranslation) => _dynamicTranslationRepository.Create(dynamicTranslation);

        public void Delete(int id) => _dynamicTranslationRepository.Delete(id);

        public IEnumerable<DynamicTranslationModel> GetAll() => _dynamicTranslationRepository.ReadAll();

        public DynamicTranslationModel GetById(int id) => _dynamicTranslationRepository.Read(id);

        public DynamicTranslationModel Update(DynamicTranslationModel DynamicTranslation) => _dynamicTranslationRepository.Update(DynamicTranslation);
    }
}
