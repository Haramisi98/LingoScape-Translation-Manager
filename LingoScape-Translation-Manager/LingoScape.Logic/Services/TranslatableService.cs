using LingoScape.DataAccessLayer.Models;
using LingoScape.Logic.Interfaces;
using LingoScape.Repository.Interfaces;

namespace LingoScape.Logic.Services
{
    public class TranslatableService(IRepositoryBase<TranslatableTextModel> _translatableRepository) : ITranslatableService
    {
        public TranslatableTextModel Create(TranslatableTextModel translatable) => _translatableRepository.Create(translatable);

        public void Delete(int id) => _translatableRepository.Delete(id);

        public IEnumerable<TranslatableTextModel> GetAll() => _translatableRepository.ReadAll();

        public TranslatableTextModel GetById(int id) => _translatableRepository.Read(id);

        public TranslatableTextModel Update(TranslatableTextModel translatable) => _translatableRepository.Update(translatable);
    }
}
