using mobibank_test.model;

namespace mobibank_test.controller.dto
{
    public class UserInputDto
    {
        public string Name { get; set; }

        public UserInputDto() { }

        public UserInputDto(User user)
        {
            Name = user.Name;
        }

        public static User MapToEntity(UserInputDto userInputDto)
        {
            if (userInputDto != null)
                return new User(userInputDto.Name);
            else
                return null;
        }
    }
}