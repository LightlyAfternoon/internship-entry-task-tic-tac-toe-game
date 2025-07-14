using Microsoft.AspNetCore.Mvc;
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
        public IResult GetUserById(long id)
        {
            User? user = UserService.FindById(id);

            if (user != null)
                return Results.Json(user);
            else
                return Results.NotFound($"Пользователь с id={id} не найден");
        }

        /// <summary>
        /// Получение списка всех игроков
        /// </summary>
        /// <returns>Всех игроков</returns>
        [HttpGet]
        public IResult GetAllUsers()
        {
            List<User> users = UserService.FindAll();

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
        public IResult AddNewUser([FromBody] User user)
        {
            user = UserService.Add(user);

            if (user != null)
                return Results.Json(user);
            else
                return Results.BadRequest("Не получилось добавить пользователя");
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
        public IResult UpdateUser(long id, [FromBody] User user)
        {
            if (UserService.FindById(id) == null)
                return Results.NotFound($"Пользователь с id={id} не найден" );

            user = UserService.Update(id, user);

            if (user != null)
                return Results.Json(user);
            else
                return Results.BadRequest("Не получилось изменить данные пользователя");
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
        public IResult DeleteUserById(long id)
        {
            bool isDeleted = UserService.DeleteById(id);

            if (isDeleted)
                return Results.Ok($"Пользователь с id={id} удалён");
            else
                return Results.NotFound($"Пользователь с id={id} не найден");
        }
    }
}