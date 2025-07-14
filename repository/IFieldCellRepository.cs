using mobibank_test.model;

namespace mobibank_test.repository;

public interface IFieldCellRepository : IRepository<FieldCell>
{
    public List<FieldCell> FindAllBySessionId(long sessionId);

    public List<FieldCell> FindAllByOccupiedUserId(long userId);

    public List<FieldCell> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId);
}