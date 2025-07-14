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

        public Session? FindById(long id)
        {
            return SessionRepository.FindById(id);
        }

        public List<Session> FindAll()
        {
            return SessionRepository.FindAll();
        }

        public Session Add(Session entity)
        {
            Session session = new Session(0, entity);

            return SessionRepository.Save(session);
        }

        public Session Update(long id, Session entity)
        {
            Session session = new Session(id, entity);

            return SessionRepository.Save(session);
        }

        public bool DeleteById(long id)
        {
            return SessionRepository.DeleteById(id);
        }
    }
}