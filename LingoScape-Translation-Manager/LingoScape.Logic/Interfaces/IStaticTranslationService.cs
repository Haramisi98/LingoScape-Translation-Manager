using LingoScape.DataAccessLayer.Models;

namespace LingoScape.Logic.Interfaces
{
    public interface IStaticTranslationService
    {
        IEnumerable<StaticTranslationModel> GetAll();
        StaticTranslationModel GetById(int id);
        StaticTranslationModel Create(StaticTranslationModel translatable);
        StaticTranslationModel Update(StaticTranslationModel translatable);
        void Delete(int id);
    }
}
