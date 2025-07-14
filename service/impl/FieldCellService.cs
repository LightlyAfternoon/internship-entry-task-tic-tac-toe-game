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

        public FieldCell? FindById(long id)
        {
            return FieldCellRepository.FindById(id);
        }

        public List<FieldCell> FindAll()
        {
            return FieldCellRepository.FindAll();
        }

        public List<FieldCell> FindAllBySessionId(long sessionId)
        {
            return FieldCellRepository.FindAllBySessionId(sessionId);
        }

        public List<FieldCell> FindAllByOccupiedUserId(long userId)
        {
            return FieldCellRepository.FindAllByOccupiedUserId(userId);
        }

        public List<FieldCell> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId)
        {
            return FieldCellRepository.FindAllBySessionIdAndOccupiedUserId(sessionId, userId);
        }

        public FieldCell Add(FieldCell entity)
        {
            FieldCell fieldcell = new FieldCell(0, entity);

            return FieldCellRepository.Save(fieldcell);
        }

        public FieldCell Update(long id, FieldCell entity)
        {
            FieldCell fieldcell = new FieldCell(id, entity);

            return FieldCellRepository.Save(fieldcell);
        }

        public bool DeleteById(long id)
        {
            return FieldCellRepository.DeleteById(id);
        }
    }
}