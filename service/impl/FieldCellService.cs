using mobibank_test.model;
using mobibank_test.repository;

namespace mobibank_test.service.impl
{
    public class FieldCellService : IFieldCellService
    {
        private IFieldCellRepository FieldCellRepository;

        public FieldCellService(IFieldCellRepository fieldCellRepository)
        {
            FieldCellRepository = fieldCellRepository;
        }

        public async Task<FieldCell?> FindById(long id)
        {
            return await FieldCellRepository.FindById(id);
        }

        public async Task<List<FieldCell>> FindAll()
        {
            return await FieldCellRepository.FindAll();
        }

        public async Task<List<FieldCell>> FindAllBySessionId(long sessionId)
        {
            return await FieldCellRepository.FindAllBySessionId(sessionId);
        }

        public async Task<List<FieldCell>> FindAllByOccupiedUserId(long userId)
        {
            return await FieldCellRepository.FindAllByOccupiedUserId(userId);
        }

        public async Task<List<FieldCell>> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId)
        {
            return await FieldCellRepository.FindAllBySessionIdAndOccupiedUserId(sessionId, userId);
        }

        public async Task<FieldCell> Add(FieldCell entity)
        {
            FieldCell fieldcell = new FieldCell(0, entity);

            return await FieldCellRepository.Save(fieldcell);
        }

        public async Task<FieldCell> Update(long id, FieldCell entity)
        {
            FieldCell fieldcell = new FieldCell(id, entity);

            return await FieldCellRepository.Save(fieldcell);
        }

        public async Task<bool> DeleteById(long id)
        {
            return await FieldCellRepository.DeleteById(id);
        }
    }
}