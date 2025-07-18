using Microsoft.AspNetCore.Mvc;
using mobibank_test.controller.dto;
using mobibank_test.model;
using mobibank_test.service;
using System.Net;

namespace mobibank_test.controller
{
    [ApiController]
    [Route("games")]
    [Produces("application/json")]
    public class SessionController
    {
        private readonly ISessionService SessionService;
        private readonly IFieldCellService FieldCellService;

        public SessionController(ISessionService sessionService, IFieldCellService fieldCellService)
        {
            SessionService = sessionService;
            FieldCellService = fieldCellService;
        }

        /// <summary>
        /// Получение игры по id
        /// </summary>
        /// <param name="id">id игры</param>
        /// <returns>Игру с указанным id</returns>
        /// <response code="404">Если игра с данным id не найдена</response>
        [HttpGet("{id}")]
        public async Task<IResult> GetSessionById(long id)
        {
            Session? session = await SessionService.FindById(id);

            if (session != null)
                return Results.Json(session);
            else
                return Results.NotFound(StandardProblem.SessionNotFound(id));
        }

        /// <summary>
        /// Получение списка всех игр
        /// </summary>
        /// <returns>Все игры</returns>
        [HttpGet]
        public async Task<IResult> GetAllSessions()
        {
            List<Session> sessions = await SessionService.FindAll();

            return Results.Json(sessions);
        }

        /// <summary>
        /// Добавление новой игры в БД
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /games
        ///     {
        ///        "player_x_id": "2",
        ///        "player_y_id": "3",
        ///        "winner_id": 1,
        ///        "is_ended": "true"
        ///     }
        ///
        /// </remarks>
        /// <param name="session">Данные игры</param>
        /// <returns>Новую игру</returns>
        /// <response code="400">Если session=null</response>
        [HttpPost]
        public async Task<IResult> AddNewSession([FromBody] SessionInputDto sessionInputDto)
        {
            Session session = await SessionService.Add(SessionInputDto.MapToEntity(sessionInputDto));

            if (session != null)
                return Results.Json(session);
            else
                return Results.BadRequest(new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: "/games"));
        }

        /// <summary>
        /// Изменение данных игры с указанным id (если такой есть в БД)
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /games
        ///     {
        ///        "player_x_id": "2",
        ///        "player_y_id": "3",
        ///        "winner_id": 1,
        ///        "is_ended": "true"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">id игры</param>
        /// <param name="session">Данные игры</param>
        /// <returns>Игру с изменёнными данными</returns>
        /// <response code="400">Если session=null</response>
        /// <response code="404">Если игра с данным id не найдена</response>
        [HttpPut("{id}")]
        public async Task<IResult> UpdateSession(long id, [FromBody] SessionInputDto sessionInputDto)
        {
            if (await SessionService.FindById(id) == null)
                return Results.NotFound(StandardProblem.SessionNotFound(id));

            Session session = await SessionService.Update(id, SessionInputDto.MapToEntity(sessionInputDto));

            if (session != null)
                return Results.Json(session);
            else
                return Results.BadRequest(new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: $"/games/{id}"));
        }

        /// <summary>
        /// Удаление игры по id
        /// </summary>
        /// <param name="id">id игры</param>
        /// <returns>Статус удаления игры</returns>
        /// <response code="200">Если игра с данным id успешно удалена</response>
        /// <response code="404">Если игра с данным id не найдена</response>
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteSessionById(long id)
        {
            bool isDeleted = await SessionService.DeleteById(id);

            if (isDeleted)
                return Results.Ok($"Игра с id={id} удалена");
            else
                return Results.NotFound(StandardProblem.SessionNotFound(id));
        }

        /// <summary>
        /// Получение списка всех ходов игры с указанным id
        /// </summary>
        /// <param name="id">id игры</param>
        /// <returns>Все ходы, сделанные за игру</returns>
        [HttpGet("{id}/moves")]
        public async Task<IResult> GetAllSessionMoves(long id)
        {
            List<FieldCell> fieldCells = await FieldCellService.FindAllBySessionId(id);

            return Results.Json(fieldCells);
        }

        /// <summary>
        /// Добавление новой игры в БД
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /1/moves
        ///     {
        ///        "x": 0,
        ///        "y": 2,
        ///        "session_id": 1,
        ///        "ocupied_by_user_id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="id">id игры</param>
        /// <param name="fieldCell">Данные клетки поля</param>
        /// <returns></returns>
        [HttpPost("{id}/moves")]
        public async Task<IResult> AddNewSessionMove(long id, [FromBody] FieldCellInputDto fieldCellInputDto)
        {
            FieldCell fieldCell = FieldCellInputDto.MapToEntity(fieldCellInputDto);

            if (fieldCell != null)
            {
                fieldCell.SessionId = id;
                Session session = await SessionService.FindById(id);

                if (session.IsEnded)
                {
                    return Results.Problem(StandardProblem.SessionAlreadyEnded(id, 0L));
                }

                if (session == null)
                {
                    return Results.NotFound(StandardProblem.SessionNotFound(id, 0L));
                }

                foreach (FieldCell existedFieldCell in session.Cells)
                {
                    if (existedFieldCell.SessionId == fieldCell.SessionId
                        && existedFieldCell.X == fieldCell.X && existedFieldCell.Y == fieldCell.Y)
                    {
                        return Results.Problem(StandardProblem.FieldIsNotEmpty(id));
                    }
                }

                if (fieldCell.OccupiedByUserId != session.GetCurrentTurnPlayerId())
                {
                    return Results.Problem(StandardProblem.FieldIsNotCurrentTurnPlayer(id));
                }

                if (session.Cells.Count > 0 && session.Cells.Count % 3 == 0)
                {
                    double chancePercentage = 10 / 100;
                    double randomNumber = new Random().NextDouble();

                    if (randomNumber <= chancePercentage)
                    {
                        fieldCell = ReverseFieldCellOwner(session, fieldCell);
                    }
                }

                fieldCell = await FieldCellService.Add(fieldCell);

                if (session.IsWinCondition(fieldCell))
                {
                    session.WinnerId = fieldCell.OccupiedByUserId;
                    session.IsEnded = true;
                }

                if (session.AllFieldIsFull())
                {
                    session.IsEnded = true;

                    session = await SessionService.Update(session.Id, session);
                }
            }

            if (fieldCell != null)
                return Results.Json(fieldCell);
            else
                return Results.BadRequest(StandardProblem.FieldBadRequest(id, 0L));
        }

        /// <summary>
        /// Изменение данных хода с указанным moveId и принадлежащий игре с id (если такой есть в БД)
        /// </summary>
        /// <param name="id">id игры</param>
        /// <param name="moveId">id хода</param>
        /// <param name="fieldCell">Данные клетки поля</param>
        /// <returns></returns>
        [HttpPut("{id}/moves/{moveId}")]
        public async Task<IResult> UpdateSessionMove(long id, long moveId, [FromBody] FieldCellInputDto fieldCellInputDto)
        {
            FieldCell fieldCell = FieldCellInputDto.MapToEntity(fieldCellInputDto);

            Session session = await SessionService.FindById(id);
            if (session == null)
                return Results.NotFound(StandardProblem.SessionNotFound(id, moveId));
            else if (await FieldCellService.FindById(moveId) == null)
                return Results.NotFound(StandardProblem.FieldNotFound(id, moveId));
            else if (await FieldCellService.FindById(moveId) != null && (await FieldCellService.FindById(moveId)).SessionId != id)
                return Results.Problem(StandardProblem.FieldForbidden(id, moveId));

            foreach (FieldCell existedFieldCell in session.Cells)
            {
                if (existedFieldCell.SessionId == id && existedFieldCell.Id != moveId
                    && existedFieldCell.X == fieldCell.X && existedFieldCell.Y == fieldCell.Y)
                {
                    return Results.Problem(StandardProblem.FieldIsNotEmpty(id, moveId));
                }
            }

            fieldCell = await FieldCellService.Update(moveId, FieldCellInputDto.MapToEntity(fieldCellInputDto));

            if (session.AllFieldIsFull())
            {
                session.IsEnded = true;

                session = await SessionService.Update(session.Id, session);
            }

            if (fieldCell != null)
                return Results.Json(fieldCell);
            else
                return Results.BadRequest(StandardProblem.FieldBadRequest(id, moveId));
        }

        /// <summary>
        /// Удаление хода по moveId
        /// </summary>
        /// <param name="id">id игры</param>
        /// <param name="moveId">id хода</param>
        /// <returns></returns>
        [HttpDelete("{id}/moves/{moveId}")]
        public async Task<IResult> DeleteMoveByMoveId(long id, long moveId)
        {
            if (await SessionService.FindById(id) == null)
                return Results.NotFound(StandardProblem.SessionNotFound(id, moveId));
            else if (await FieldCellService.FindById(moveId) != null && (await FieldCellService.FindById(moveId)).SessionId != id)
                return Results.Problem(StandardProblem.FieldForbidden(id, moveId));

            bool isDeleted = await FieldCellService.DeleteById(moveId);

            if (isDeleted)
                return Results.Ok($"Ход с id={moveId} удалён");
            else
                return Results.NotFound(StandardProblem.FieldNotFound(id, moveId));
        }

        private FieldCell ReverseFieldCellOwner(Session session, FieldCell cell)
        {
            if (session.GetCurrentTurnPlayerId() == session.PlayerXId)
                cell.OccupiedByUserId = session.PlayerYId;
            else
                cell.OccupiedByUserId = session.PlayerXId;

            return cell;
        }
    }
}