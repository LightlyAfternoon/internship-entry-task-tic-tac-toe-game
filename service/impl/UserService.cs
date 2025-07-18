using mobibank_test.model;
using mobibank_test.repository;

namespace mobibank_test.service.impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository UserRepository;

        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public async Task<User?> FindById(long id)
        {
            return await UserRepository.FindById(id);
        }

        public async Task<List<User>> FindAll()
        {
            return await UserRepository.FindAll();
        }

        public async Task<User> Add(User entity)
        {
            User user = new User(0, entity);

            return await UserRepository.Save(user);
        }

        public async Task<User> Update(long id, User entity)
        {
            User user = new User(id, entity);
            
            return await UserRepository.Save(user);
        }

        public async Task<bool> DeleteById(long id)
        {
            return await UserRepository.DeleteById(id);
        }
    }
}