using mobibank_test.model;

namespace mobibank_test.repository;

public interface IFieldCellRepository : IRepository<FieldCell>
{
    public Task<List<FieldCell>> FindAllBySessionId(long sessionId);

    public Task<List<FieldCell>> FindAllByOccupiedUserId(long userId);

    public Task<List<FieldCell>> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId);
}