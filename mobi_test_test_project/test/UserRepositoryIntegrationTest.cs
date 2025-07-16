using mobibank_test.db;
using mobibank_test.model;
using mobibank_test.repository;
using mobibank_test.repository.impl;
using Testcontainers.PostgreSql;

namespace mobibank_test.mobi_test_test_project.test
{
    public class UserRepositoryIntegrationTest : IAsyncLifetime
    {
        private readonly PostgreSqlContainer Container = new PostgreSqlBuilder().WithImage("postgres:17.4").WithCleanUp(true).Build();

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

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = UserRepository.Save(user1);

            Assert.Equal(user1, UserRepository.FindById(user1.Id));

            User user2 = new User();
            user2.Name = "user 2";

            user2 = UserRepository.Save(user2);

            Assert.Equal(user2, UserRepository.FindById(user2.Id));
        }

        [Fact]
        public void FindAllTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = UserRepository.Save(user1);

            Assert.Contains(user1, UserRepository.FindAll());

            User user2 = new User();
            user2.Name = "user 2";

            user2 = UserRepository.Save(user2);

            Assert.Contains(user1, UserRepository.FindAll());
            Assert.Contains(user2, UserRepository.FindAll());

            UserRepository.DeleteById(user1.Id);

            Assert.Contains(user2, UserRepository.FindAll());
        }

        [Fact]
        public void SaveTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = UserRepository.Save(user1);

            Assert.Equal("user 1", user1.Name);

            User user2 = new User();
            user2.Name = "user 2";

            Assert.Equal("user 2", UserRepository.Save(user2).Name);

            User editedUser1 = new User(user1);
            editedUser1.Name = "user 3";

            Assert.Equal("user 3", UserRepository.Save(editedUser1).Name);
        }

        [Fact]
        public void DeleteByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = UserRepository.Save(user1);

            Assert.Equal(1, user1.Id);
            Assert.True(UserRepository.DeleteById(user1.Id));

            Assert.False(UserRepository.DeleteById(50L));
        }
    }
}