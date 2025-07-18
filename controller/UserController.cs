using Microsoft.AspNetCore.Mvc;
using mobibank_test.controller.dto;
using mobibank_test.model;
using mobibank_test.service;

namespace mobibank_test.controller
{
    [ApiController]
    [Route("users")]
    [Produces("application/json")]
    public class UserController
    {
        private readonly IUserService UserService;

        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// Получение игрока по id
        /// </summary>
        /// <param name="id">id игрока</param>
        /// <returns>Игрока с указанным id</returns>
        /// <response code="404">Если игрок с данным id не найден</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IResult> GetUserById(long id)
        {
            User? user = await UserService.FindById(id);

            if (user != null)
                return Results.Json(user);
            else
                return Results.NotFound(StandardProblem.UserNotFound(id));
        }

        /// <summary>
        /// Получение списка всех игроков
        /// </summary>
        /// <returns>Всех игроков</returns>
        [HttpGet]
        public async Task<IResult> GetAllUsers()
        {
            List<User> users = await UserService.FindAll();

            return Results.Json(users);
        }

        /// <summary>
        /// Добавление нового игрока в БД
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /users
        ///     {
        ///        "name": "New Player"
        ///     }
        ///
        /// </remarks>
        /// <param name="user">Данные игрока</param>
        /// <returns>Нового игрока</returns>
        /// <response code="400">Если user=null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> AddNewUser([FromBody] UserInputDto userDto)
        {
            User user = await UserService.Add(UserInputDto.MapToEntity(userDto));

            if (user != null)
                return Results.Json(user);
            else
                return Results.BadRequest(StandardProblem.UserBadRequest());
        }

        /// <summary>
        /// Изменение данных игрока с указанным id (если такой есть в БД)
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /users
        ///     {
        ///        "name": "Edited Player"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">id игрока</param>
        /// <param name="user">Данные игрока</param>
        /// <returns>Игрока с изменёнными данными</returns>
        /// <response code="400">Если user=null</response>
        /// <response code="404">Если игрок с данным id не найден</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IResult> UpdateUser(long id, [FromBody] UserInputDto userDto)
        {
            if (await UserService.FindById(id) == null)
                return Results.NotFound(StandardProblem.UserNotFound(id));

            User user = await UserService.Update(id, UserInputDto.MapToEntity(userDto));

            if (user != null)
                return Results.Json(user);
            else
                return Results.BadRequest(StandardProblem.UserBadRequest(id));
        }

        /// <summary>
        /// Удаление игрока по id
        /// </summary>
        /// <param name="id">id игрока</param>
        /// <returns>Статус удаления игрока</returns>
        /// <response code="200">Если игрок с данным id успешно удалён</response>
        /// <response code="404">Если игрок с данным id не найден</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IResult> DeleteUserById(long id)
        {
            bool isDeleted = await UserService.DeleteById(id);

            if (isDeleted)
                return Results.Ok($"Пользователь с id={id} удалён");
            else
                return Results.NotFound(StandardProblem.UserNotFound(id));
        }
    }
}