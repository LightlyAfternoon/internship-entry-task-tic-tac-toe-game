using mobibank_test.db;
using mobibank_test.model;
using mobibank_test.repository;
using mobibank_test.repository.impl;
using Testcontainers.PostgreSql;

namespace mobibank_test.mobi_test_test_project.test
{
    public class SessionRepositoryIntegrationTest : IAsyncLifetime
    {
        private readonly PostgreSqlContainer Container = new PostgreSqlBuilder().WithImage("postgres:17.4").WithCleanUp(true).Build();

        private ISessionRepository SessionRepository;
        private IUserRepository UserRepository;
        private ApplicationContext ApplicationContext;

        public Task InitializeAsync()
        {
            return Container.StartAsync();
        }

        public Task DisposeAsync()
        {
            return Container.DisposeAsync().AsTask();
        }

        [Fact]
        public void FindByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = SessionRepository.Save(session1);

            Assert.Equal(session1, SessionRepository.FindById(session1.Id));

            Session session2 = new Session();
            session2.PlayerXId = user2.Id;
            session2.PlayerYId = user1.Id;

            session2 = SessionRepository.Save(session2);

            Assert.Equal(session2, SessionRepository.FindById(session2.Id));
        }

        [Fact]
        public void FindAllTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = SessionRepository.Save(session1);

            Assert.Contains(session1, SessionRepository.FindAll());

            Session session2 = new Session();
            session2.PlayerXId = user2.Id;
            session2.PlayerYId = user1.Id;

            session2 = SessionRepository.Save(session2);
            
            Assert.Contains(session1, SessionRepository.FindAll());
            Assert.Contains(session2, SessionRepository.FindAll());

            SessionRepository.DeleteById(session1.Id);

            Assert.Contains(session2, SessionRepository.FindAll());
        }

        [Fact]
        public void SaveTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = SessionRepository.Save(session1);

            Assert.Equal(user1.Id, session1.PlayerXId);
            Assert.Equal(user2.Id, session1.PlayerYId);
            Assert.Equal(user2.Id, session1.WinnerId);
            Assert.True(session1.IsEnded);

            Session session2 = new Session();
            session2.PlayerXId = user2.Id;
            session2.PlayerYId = user1.Id;

            session2 = SessionRepository.Save(session2);

            Assert.Equal(user2.Id, session2.PlayerXId);
            Assert.Equal(user1.Id, session2.PlayerYId);
            Assert.Null(session2.WinnerId);
            Assert.False(session2.IsEnded);

            Session editedSession1 = new Session(session1);
            editedSession1.WinnerId = user1.Id;

            editedSession1 = SessionRepository.Save(editedSession1);

            Assert.Equal(session1.Id, editedSession1.Id);
            Assert.Equal(user1.Id, editedSession1.PlayerXId);
            Assert.Equal(user2.Id, editedSession1.PlayerYId);
            Assert.Equal(user1.Id, editedSession1.WinnerId);
            Assert.True(editedSession1.IsEnded);
        }

        [Fact]
        public void DeleteByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = SessionRepository.Save(session1);

            Assert.True(SessionRepository.DeleteById(session1.Id));

            Assert.False(SessionRepository.DeleteById(50L));
        }
    }
}