using mobibank_test.db;
using mobibank_test.model;
using mobibank_test.repository;
using mobibank_test.repository.impl;
using Testcontainers.PostgreSql;

namespace mobibank_test.mobi_test_test_project.test
{
    public class FieldSessionRepositoryIntegrationTest : IAsyncLifetime
    {
        private readonly PostgreSqlContainer Container = new PostgreSqlBuilder().WithImage("postgres:17.4").WithCleanUp(true).Build();

        private IFieldCellRepository FieldCellRepository;
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

            FieldCellRepository = new FieldCellRepository(ApplicationContext);
            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            FieldCell fieldCell1 = new FieldCell();
            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;
            session1 = SessionRepository.Save(session1);
            fieldCell1.X = 0;
            fieldCell1.Y = 0;
            fieldCell1.SessionId = session1.Id;
            fieldCell1.OccupiedByUserId = user1.Id;

            fieldCell1 = FieldCellRepository.Save(fieldCell1);

            Assert.Equal(fieldCell1, FieldCellRepository.FindById(fieldCell1.Id));

            FieldCell fieldCell2 = new FieldCell();
            fieldCell2.X = 1;
            fieldCell2.Y = 0;
            fieldCell2.SessionId = session1.Id;
            fieldCell2.OccupiedByUserId = user2.Id;

            fieldCell2 = FieldCellRepository.Save(fieldCell2);

            Assert.Equal(fieldCell2, FieldCellRepository.FindById(fieldCell2.Id));
        }

        [Fact]
        public void FindAllTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            FieldCellRepository = new FieldCellRepository(ApplicationContext);
            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            FieldCell fieldCell1 = new FieldCell();
            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;
            session1 = SessionRepository.Save(session1);
            fieldCell1.X = 0;
            fieldCell1.Y = 0;
            fieldCell1.SessionId = session1.Id;
            fieldCell1.OccupiedByUserId = user1.Id;

            fieldCell1 = FieldCellRepository.Save(fieldCell1);

            Assert.Contains(fieldCell1, FieldCellRepository.FindAll());

            FieldCell fieldCell2 = new FieldCell();
            fieldCell2.X = 1;
            fieldCell2.Y = 0;
            fieldCell2.SessionId = session1.Id;
            fieldCell2.OccupiedByUserId = user2.Id;

            fieldCell2 = FieldCellRepository.Save(fieldCell2);

            Assert.Contains(fieldCell1, FieldCellRepository.FindAll());
            Assert.Contains(fieldCell2, FieldCellRepository.FindAll());

            FieldCellRepository.DeleteById(fieldCell1.Id);

            Assert.Contains(fieldCell2, FieldCellRepository.FindAll());
        }

        [Fact]
        public void SaveTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            FieldCellRepository = new FieldCellRepository(ApplicationContext);
            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            FieldCell fieldCell1 = new FieldCell();
            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;
            session1 = SessionRepository.Save(session1);
            fieldCell1.X = 0;
            fieldCell1.Y = 0;
            fieldCell1.SessionId = session1.Id;
            fieldCell1.OccupiedByUserId = user1.Id;

            fieldCell1 = FieldCellRepository.Save(fieldCell1);

            Assert.Equal(0, fieldCell1.X);
            Assert.Equal(0, fieldCell1.Y);
            Assert.Equal(session1.Id, fieldCell1.SessionId);
            Assert.Equal(user1.Id, fieldCell1.OccupiedByUserId);

            FieldCell fieldCell2 = new FieldCell();
            fieldCell2.X = 1;
            fieldCell2.Y = 0;
            fieldCell2.SessionId = session1.Id;
            fieldCell2.OccupiedByUserId = user2.Id;

            fieldCell2 = FieldCellRepository.Save(fieldCell2);

            Assert.Equal(1, fieldCell2.X);
            Assert.Equal(0, fieldCell2.Y);
            Assert.Equal(session1.Id, fieldCell2.SessionId);
            Assert.Equal(user2.Id, fieldCell2.OccupiedByUserId);

            FieldCell editedFieldCell1 = new FieldCell(fieldCell1);
            editedFieldCell1.X = 2;

            editedFieldCell1 = FieldCellRepository.Save(editedFieldCell1);

            Assert.Equal(fieldCell1.Id, editedFieldCell1.Id);
            Assert.Equal(2, editedFieldCell1.X);
            Assert.Equal(fieldCell1.Y, editedFieldCell1.Y);
            Assert.Equal(fieldCell1.SessionId, editedFieldCell1.SessionId);
            Assert.Equal(fieldCell1.OccupiedByUserId, editedFieldCell1.OccupiedByUserId);
        }

        [Fact]
        public void DeleteByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            FieldCellRepository = new FieldCellRepository(ApplicationContext);
            SessionRepository = new SessionRepository(ApplicationContext);
            UserRepository = new UserRepository(ApplicationContext);

            FieldCell fieldCell1 = new FieldCell();
            Session session1 = new Session();
            User user1 = UserRepository.Save(new User("user 1"));
            User user2 = UserRepository.Save(new User("user 2"));
            session1.PlayerXId = user1.Id;
            session1.PlayerYId = user2.Id;
            session1.WinnerId = user2.Id;
            session1.IsEnded = true;
            session1 = SessionRepository.Save(session1);
            fieldCell1.X = 0;
            fieldCell1.Y = 0;
            fieldCell1.SessionId = session1.Id;
            fieldCell1.OccupiedByUserId = user1.Id;

            fieldCell1 = FieldCellRepository.Save(fieldCell1);

            Assert.True(FieldCellRepository.DeleteById(fieldCell1.Id));

            Assert.False(FieldCellRepository.DeleteById(50L));
        }
    }
}