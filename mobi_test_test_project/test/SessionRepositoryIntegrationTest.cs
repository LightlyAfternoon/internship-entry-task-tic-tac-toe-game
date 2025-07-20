using mobibank_test.db;
using mobibank_test.model;
using mobibank_test.repository;
using mobibank_test.repository.impl;
using Testcontainers.PostgreSql;

namespace mobibank_test.mobi_test_test_project.test
{
    public class SessionRepositoryIntegrationTest : IAsyncLifetime
    {
        private readonly PostgreSqlContainer Container = new PostgreSqlBuilder()
            .WithImage("postgres:17.4").WithCleanUp(true).WithEnvironment("FIELD_SIZE", "3").Build();

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
        public async Task FindByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = await UserRepository.Save(new User("user 1"));
            User user2 = await UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerOId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = await SessionRepository.Save(session1);

            Assert.Equal(session1, await SessionRepository.FindById(session1.Id));

            Session session2 = new Session();
            session2.PlayerXId = user2.Id;
            session2.PlayerOId = user1.Id;

            session2 = await SessionRepository.Save(session2);

            Assert.Equal(session2, await SessionRepository.FindById(session2.Id));
        }

        [Fact]
        public async Task FindAllTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = await UserRepository.Save(new User("user 1"));
            User user2 = await UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerOId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = await SessionRepository.Save(session1);

            Assert.Contains(session1, await SessionRepository.FindAll());

            Session session2 = new Session();
            session2.PlayerXId = user2.Id;
            session2.PlayerOId = user1.Id;

            session2 = await SessionRepository.Save(session2);
            
            Assert.Contains(session1, await SessionRepository.FindAll());
            Assert.Contains(session2, await SessionRepository.FindAll());

            await SessionRepository.DeleteById(session1.Id);

            Assert.Contains(session2, await SessionRepository.FindAll());
        }

        [Fact]
        public async Task SaveTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = await UserRepository.Save(new User("user 1"));
            User user2 = await UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerOId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = await SessionRepository.Save(session1);

            Assert.Equal(user1.Id, session1.PlayerXId);
            Assert.Equal(user2.Id, session1.PlayerOId);
            Assert.Equal(user2.Id, session1.WinnerId);
            Assert.True(session1.IsEnded);

            Session session2 = new Session();
            session2.PlayerXId = user2.Id;
            session2.PlayerOId = user1.Id;

            session2 = await SessionRepository.Save(session2);

            Assert.Equal(user2.Id, session2.PlayerXId);
            Assert.Equal(user1.Id, session2.PlayerOId);
            Assert.Null(session2.WinnerId);
            Assert.False(session2.IsEnded);

            Session editedSession1 = new Session(session1);
            editedSession1.WinnerId = user1.Id;

            editedSession1 = await SessionRepository.Save(editedSession1);

            Assert.Equal(session1.Id, editedSession1.Id);
            Assert.Equal(user1.Id, editedSession1.PlayerXId);
            Assert.Equal(user2.Id, editedSession1.PlayerOId);
            Assert.Equal(user1.Id, editedSession1.WinnerId);
            Assert.True(editedSession1.IsEnded);
        }

        [Fact]
        public async Task DeleteByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            Session session1 = new Session();
            User user1 = await UserRepository.Save(new User("user 1"));
            User user2 = await UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerOId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;

            session1 = await SessionRepository.Save(session1);

            Assert.True(await SessionRepository.DeleteById(session1.Id));

            Assert.False(await SessionRepository.DeleteById(50L));
        }
    }
}