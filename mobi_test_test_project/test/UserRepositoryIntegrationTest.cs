using mobibank_test.db;
using mobibank_test.model;
using mobibank_test.repository;
using mobibank_test.repository.impl;
using System.Threading.Tasks;
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
        public async Task FindByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = await UserRepository.Save(user1);

            Assert.Equal(user1, await UserRepository.FindById(user1.Id));

            User user2 = new User();
            user2.Name = "user 2";

            user2 = await UserRepository.Save(user2);

            Assert.Equal(user2, await UserRepository.FindById(user2.Id));
        }

        [Fact]
        public async Task FindAllTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = await UserRepository.Save(user1);

            Assert.Contains(user1, await UserRepository.FindAll());

            User user2 = new User();
            user2.Name = "user 2";

            user2 = await UserRepository.Save(user2);

            Assert.Contains(user1, await UserRepository.FindAll());
            Assert.Contains(user2, await UserRepository.FindAll());

            await UserRepository.DeleteById(user1.Id);

            Assert.Contains(user2, await UserRepository.FindAll());
        }

        [Fact]
        public async Task SaveTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = await UserRepository.Save(user1);

            Assert.Equal("user 1", user1.Name);

            User user2 = new User();
            user2.Name = "user 2";

            Assert.Equal("user 2", (await UserRepository.Save(user2)).Name);

            User editedUser1 = new User(user1);
            editedUser1.Name = "user 3";

            Assert.Equal("user 3", (await UserRepository.Save(editedUser1)).Name);
        }

        [Fact]
        public async Task DeleteByIdTest()
        {
            ApplicationContext = new ApplicationContext(Container.GetConnectionString());

            UserRepository = new UserRepository(ApplicationContext);

            User user1 = new User();
            user1.Name = "user 1";

            user1 = await UserRepository.Save(user1);

            Assert.Equal(1, user1.Id);
            Assert.True(await UserRepository.DeleteById(user1.Id));

            Assert.False(await UserRepository.DeleteById(50L));
        }
    }
}