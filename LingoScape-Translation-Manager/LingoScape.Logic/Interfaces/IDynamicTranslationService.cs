using LingoScape.DataAccessLayer.Models;

namespace LingoScape.Logic.Interfaces
{
    public interface IDynamicTranslationService
    {
        IEnumerable<DynamicTranslationModel> GetAll();
        DynamicTranslationModel GetById(int id);
        DynamicTranslationModel Create(DynamicTranslationModel translatable);
        DynamicTranslationModel Update(DynamicTranslationModel translatable);
        void Delete(int id);
    }
}
