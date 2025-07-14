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

        public User? FindById(long id)
        {
            return UserRepository.FindById(id);
        }

        public List<User> FindAll()
        {
            return UserRepository.FindAll();
        }

        public User Add(User entity)
        {
            User user = new User(0, entity);

            return UserRepository.Save(user);
        }

        public User Update(long id, User entity)
        {
            User user = new User(id, entity);
            
            return UserRepository.Save(user);
        }

        public bool DeleteById(long id)
        {
            return UserRepository.DeleteById(id);
        }
    }
}