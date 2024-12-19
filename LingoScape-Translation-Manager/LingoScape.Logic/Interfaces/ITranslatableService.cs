using LingoScape.DataAccessLayer.Models;

namespace LingoScape.Logic.Interfaces
{
    public interface ITranslatableService
    {
        IEnumerable<TranslatableTextModel> GetAll();
        TranslatableTextModel GetById(int id);
        TranslatableTextModel Create(TranslatableTextModel translatable);
        TranslatableTextModel Update(TranslatableTextModel translatable);
        void Delete(int id);
    }
}
