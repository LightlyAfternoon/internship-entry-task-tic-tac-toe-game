using mobibank_test.model;
using mobibank_test.repository;

namespace mobibank_test.service.impl
{
    public class FieldCellService : IFieldCellService
    {
        private readonly IFieldCellRepository FieldCellRepository;
        private readonly ISessionService SessionService;

        public FieldCellService(IFieldCellRepository fieldCellRepository, ISessionService sessionService)
        {
            FieldCellRepository = fieldCellRepository;
            SessionService = sessionService;
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
            Session session = await SessionService.FindById(entity.SessionId);

            if (session.Cells.Count > 0 && session.Cells.Count % 3 == 0)
            {
                double chancePercentage = 10 / 100;
                double randomNumber = new Random().NextDouble();

                if (randomNumber <= chancePercentage)
                {
                    entity = ReverseFieldCellOwner(session, entity);
                }
            }

            if (session.IsWinCondition(entity))
            {
                session.WinnerId = entity.OccupiedByUserId;
                session.IsEnded = true;

                session = await SessionService.Update(session.Id, session);
            }

            if (session.AllFieldIsFull())
            {
                session.IsEnded = true;

                session = await SessionService.Update(session.Id, session);
            }

            return await FieldCellRepository.Save(fieldcell);
        }

        public async Task<FieldCell> Update(long id, FieldCell entity)
        {
            FieldCell fieldcell = new FieldCell(id, entity);
            Session session = await SessionService.FindById(id);

            if (session.AllFieldIsFull())
            {
                session.IsEnded = true;

                session = await SessionService.Update(session.Id, session);
            }

            return await FieldCellRepository.Save(fieldcell);
        }

        public async Task<bool> DeleteById(long id)
        {
            return await FieldCellRepository.DeleteById(id);
        }

        private FieldCell ReverseFieldCellOwner(Session session, FieldCell cell)
        {
            if (session.GetCurrentTurnPlayerId() == session.PlayerXId)
                cell.OccupiedByUserId = session.PlayerOId;
            else
                cell.OccupiedByUserId = session.PlayerXId;

            return cell;
        }
    }
}