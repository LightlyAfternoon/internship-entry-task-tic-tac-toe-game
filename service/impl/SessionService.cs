using mobibank_test.model;
using mobibank_test.repository;

namespace mobibank_test.service.impl
{
    public class SessionService : ISessionService
    {
        private ISessionRepository SessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            SessionRepository = sessionRepository;
        }

        public async Task<Session?> FindById(long id)
        {
            return await SessionRepository.FindById(id);
        }

        public async Task<List<Session>> FindAll()
        {
            return await SessionRepository.FindAll();
        }

        public async Task<Session> Add(Session entity)
        {
            Session session = new Session(0, entity);

            return await SessionRepository.Save(session);
        }

        public async Task<Session> Update(long id, Session entity)
        {
            Session session = new Session(id, entity);

            return await SessionRepository.Save(session);
        }

        public async Task<bool> DeleteById(long id)
        {
            return await SessionRepository.DeleteById(id);
        }
    }
}