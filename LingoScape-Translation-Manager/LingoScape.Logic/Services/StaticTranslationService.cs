using LingoScape.DataAccessLayer.Models;
using LingoScape.Logic.Interfaces;
using LingoScape.Repository.Interfaces;

namespace LingoScape.Logic.Services
{
    public class StaticTranslationService(IRepositoryBase<StaticTranslationModel> _staticTranslationRepository) : IStaticTranslationService
    {
        public StaticTranslationModel Create(StaticTranslationModel staticTranslation) => _staticTranslationRepository.Create(staticTranslation);

        public void Delete(int id) => _staticTranslationRepository.Delete(id);

        public IEnumerable<StaticTranslationModel> GetAll() => _staticTranslationRepository.ReadAll();

        public StaticTranslationModel GetById(int id) => _staticTranslationRepository.Read(id);

        public StaticTranslationModel Update(StaticTranslationModel staticTranslation) => _staticTranslationRepository.Update(staticTranslation);
    }
}
