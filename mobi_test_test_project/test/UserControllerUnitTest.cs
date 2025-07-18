using Microsoft.AspNetCore.Http.HttpResults;
using mobibank_test.controller;
using mobibank_test.controller.dto;
using mobibank_test.model;
using mobibank_test.service;
using Moq;

namespace mobibank_test.mobi_test_test_project.test
{
    public class UserControllerUnitTest
    {
        [Fact]
        public async Task GetUserByIdTest()
        {
            var user = new User(1L, "test user");
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            mockUserService.Setup(x => x.FindById(1L).Result).Returns(new User(user));

            Assert.Equal(((JsonHttpResult<User>)Results.Json(user)).Value,
                         ((JsonHttpResult<User>)await controller.GetUserById(1L)).Value);

            user.Name = "new name of user";

            Assert.NotEqual(((JsonHttpResult<User>)Results.Json(user)).Value,
                         ((JsonHttpResult<User>)await controller.GetUserById(1L)).Value);
        }

        [Fact]
        public async Task GetAllUsersTest()
        {
            var user1 = new User(1L, "test user");
            var user2 = new User(2L, "test user2");
            var users = new List<User> { user1, user2 };
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            mockUserService.Setup(x => x.FindAll().Result).Returns(new List<User> { new User(user1), new User(user2) });

            Assert.Equal(((JsonHttpResult<List<User>>)Results.Json(users)).Value,
                ((JsonHttpResult<List<User>>)await controller.GetAllUsers()).Value);

            user1.Name = "new name of user";

            Assert.NotEqual(((JsonHttpResult<List<User>>)Results.Json(users)).Value,
                ((JsonHttpResult<List<User>>)await controller.GetAllUsers()).Value);

            user1.Name = "test user";

            Assert.Equal(((JsonHttpResult<List<User>>)Results.Json(users)).Value,
                ((JsonHttpResult<List<User>>)await controller.GetAllUsers()).Value);

            users = new List<User> { user1 };

            Assert.NotEqual(((JsonHttpResult<List<User>>)Results.Json(users)).Value,
                ((JsonHttpResult<List<User>>)await controller.GetAllUsers()).Value);
        }

        [Fact]
        public async Task AddNewUserTest()
        {
            var user = new UserInputDto(new User("test user"));
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            mockUserService.Setup(x => x.Add(UserInputDto.MapToEntity(user)).Result).Returns(new User(1L, UserInputDto.MapToEntity(user)));

            Assert.Equal(((JsonHttpResult<User>)Results.Json(new User(1L, UserInputDto.MapToEntity(user)))).Value,
                         ((JsonHttpResult<User>)await controller.AddNewUser(user)).Value);

            user = null;
            mockUserService.Setup(x => x.Add(UserInputDto.MapToEntity(user)).Result).Returns(UserInputDto.MapToEntity(user));
            
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.UserBadRequest())).StatusCode,
                         ((BadRequest<StandardProblem>)await controller.AddNewUser(user)).StatusCode);
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.UserBadRequest())).Value,
                         ((BadRequest<StandardProblem>)await controller.AddNewUser(user)).Value);
        }

        [Fact]
        public async Task UpdateUserTest()
        {
            var user = new UserInputDto(new User(1, "test user"));
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            mockUserService.Setup(x => x.FindById(2L).Result).Returns(new User(2L, UserInputDto.MapToEntity(user)));
            user.Name = "new name user";
            mockUserService.Setup(x => x.Update(2L, UserInputDto.MapToEntity(user)).Result).Returns(new User(2L, UserInputDto.MapToEntity(user)));

            Assert.Equal(((JsonHttpResult<User>)Results.Json(new User(2, UserInputDto.MapToEntity(user)))).Value,
                         ((JsonHttpResult<User>)await controller.UpdateUser(2, user)).Value);

            user = null;
            mockUserService.Setup(x => x.FindById(1L).Result).Returns(UserInputDto.MapToEntity(user));
            mockUserService.Setup(x => x.Update(1L, UserInputDto.MapToEntity(user)).Result).Returns(UserInputDto.MapToEntity(user));

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.UserNotFound(1L))).StatusCode,
                            ((NotFound<StandardProblem>)await controller.UpdateUser(1L, user)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.UserNotFound(1L))).Value,
                            ((NotFound<StandardProblem>)await controller.UpdateUser(1L, user)).Value);
        }
        
        [Fact]
        public async Task DeleteUserByIdTest()
        {
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            mockUserService.Setup(x => x.DeleteById(1L).Result).Returns(true);

            Assert.Equal(((Ok<string>)Results.Ok($"Пользователь с id=1 удалён")).StatusCode,
                         ((Ok<string>)await controller.DeleteUserById(1L)).StatusCode);
            Assert.Equal(((Ok<string>)Results.Ok($"Пользователь с id=1 удалён")).Value,
                         ((Ok<string>)await controller.DeleteUserById(1L)).Value);

            mockUserService.Setup(x => x.DeleteById(2L).Result).Returns(false);

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.UserNotFound(2L))).StatusCode,
                         ((NotFound<StandardProblem>)await controller.DeleteUserById(2L)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.UserNotFound(2L))).Value,
                         ((NotFound<StandardProblem>)await controller.DeleteUserById(2L)).Value);
        }
    }
}