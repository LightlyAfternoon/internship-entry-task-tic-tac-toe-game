using mobibank_test.model;

namespace mobibank_test.service
{
    public interface IFieldCellService : IService<FieldCell>
    {
        public List<FieldCell> FindAllBySessionId(long sessionId);

        public List<FieldCell> FindAllByOccupiedUserId(long userId);

        public List<FieldCell> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId);
    }
}