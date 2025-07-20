using Microsoft.AspNetCore.Http.HttpResults;
using mobibank_test.controller;
using mobibank_test.controller.dto;
using mobibank_test.model;
using mobibank_test.service;
using Moq;

namespace mobibank_test.mobi_test_test_project.test
{
    public class SessionControllerUnitTest
    {
        public SessionControllerUnitTest()
        {
            Environment.SetEnvironmentVariable("FIELD_SIZE", "3");
            Environment.SetEnvironmentVariable("WIN_CONDITION", "3");
        }

        [Fact]
        public async Task GetSessionByIdTest()
        {
            var session = new Session(1L, 1L, 2L);
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(1L).Result).Returns(new Session(session));

            Assert.Equal(((JsonHttpResult<Session>)Results.Json(session)).Value,
                         ((JsonHttpResult<Session>)await controller.GetSessionById(1)).Value);

            session.PlayerOId = 3L;

            Assert.NotEqual(((JsonHttpResult<Session>)Results.Json(session)).Value,
                         ((JsonHttpResult<Session>)await controller.GetSessionById(1)).Value);
        }

        [Fact]
        public async Task GetAllSessionsTest()
        {
            var session1 = new Session(1L, 1L, 2L);
            var session2 = new Session(2L, 2L, 5L);
            var sessions = new List<Session> { session1, session2 };
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindAll().Result).Returns(new List<Session> { new Session(session1), new Session(session2) });

            Assert.Equal(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value,
                ((JsonHttpResult<List<Session>>)await controller.GetAllSessions()).Value);

            session1.PlayerOId = 3L;

            Assert.NotEqual(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value,
                ((JsonHttpResult<List<Session>>)await controller.GetAllSessions()).Value);

            session1.PlayerOId = 2L;

            Assert.Equal(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value,
                ((JsonHttpResult<List<Session>>)await controller.GetAllSessions()).Value);

            sessions = new List<Session> { session1 };

            Assert.NotEqual(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value,
                ((JsonHttpResult<List<Session>>)await controller.GetAllSessions()).Value);
        }

        [Fact]
        public async Task AddNewSessionTest()
        {
            var session = new SessionInputDto(new Session(1L, 2L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.Add(SessionInputDto.MapToEntity(session)).Result).Returns(new Session(1L, SessionInputDto.MapToEntity(session)));

            Assert.Equal(((JsonHttpResult<Session>)Results.Json(new Session(1L, SessionInputDto.MapToEntity(session)))).Value,
                         ((JsonHttpResult<Session>)await controller.AddNewSession(session)).Value);

            session = null;
            mockSessionService.Setup(x => x.Add(SessionInputDto.MapToEntity(session)).Result).Returns(SessionInputDto.MapToEntity(session));

            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.SessionBadRequest())).StatusCode,
                         ((BadRequest<StandardProblem>)await controller.AddNewSession(session)).StatusCode);
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.SessionBadRequest())).Value,
                         ((BadRequest<StandardProblem>)await controller.AddNewSession(session)).Value);
        }

        [Fact]
        public async Task UpdateSessionTest()
        {
            var session = new SessionInputDto(new Session(1, 1L, 2L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(2L).Result).Returns(new Session(2L, SessionInputDto.MapToEntity(session)));
            session.PlayerOId = 3L;
            mockSessionService.Setup(x => x.Update(2L, SessionInputDto.MapToEntity(session)).Result).Returns(new Session(2L, SessionInputDto.MapToEntity(session)));

            Assert.Equal(((JsonHttpResult<Session>)Results.Json(new Session(2L, SessionInputDto.MapToEntity(session)))).Value,
                         ((JsonHttpResult<Session>)await controller.UpdateSession(2L, session)).Value);

            session = null;
            mockSessionService.Setup(x => x.FindById(1L).Result).Returns(SessionInputDto.MapToEntity(session));
            mockSessionService.Setup(x => x.Update(1L, SessionInputDto.MapToEntity(session)).Result).Returns(SessionInputDto.MapToEntity(session));
            
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(1L))).StatusCode,
                            ((NotFound<StandardProblem>)await controller.UpdateSession(1L, session)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(1L))).Value,
                            ((NotFound<StandardProblem>)await controller.UpdateSession(1L, session)).Value);
        }
        
        [Fact]
        public async Task DeleteSessionByIdTest()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.DeleteById(1L).Result).Returns(true);

            Assert.Equal(((Ok<string>)Results.Ok($"Игра с id=1 удалена")).StatusCode,
                         ((Ok<string>)await controller.DeleteSessionById(1L)).StatusCode);
            Assert.Equal(((Ok<string>)Results.Ok($"Игра с id=1 удалена")).Value,
                         ((Ok<string>)await controller.DeleteSessionById(1L)).Value);

            mockSessionService.Setup(x => x.DeleteById(2L).Result).Returns(false);

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(2L))).StatusCode,
                         ((NotFound<StandardProblem>)await controller.DeleteSessionById(2L)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(2L))).Value,
                         ((NotFound<StandardProblem>)await controller.DeleteSessionById(2L)).Value);
        }

        [Fact]
        public async Task GetAllSessionMovesTest()
        {
            var session = new Session(1L, 1L, 2L);
            var move1 = new FieldCell(1L, 0, 1, 1L, session.Id);
            var move2 = new FieldCell(2L, 1, 1, 1L, session.Id);
            var moves = new List<FieldCell> { move1, move2 };
            session.Cells = moves;
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockFieldCellService.Setup(x => x.FindAllBySessionId(1L).Result).Returns(new List<FieldCell> { new FieldCell(move1), new FieldCell(move2) });

            Assert.Equal(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                         ((JsonHttpResult<List<FieldCell>>)await controller.GetAllSessionMoves(1L)).Value);

            move1.Y = 2;

            Assert.NotEqual(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                            ((JsonHttpResult<List<FieldCell>>)await controller.GetAllSessionMoves(1L)).Value);

            move1.Y = 1;

            Assert.Equal(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                         ((JsonHttpResult<List<FieldCell>>)await controller.GetAllSessionMoves(1L)).Value);

            moves = new List<FieldCell> { move1 };

            Assert.NotEqual(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                            ((JsonHttpResult<List<FieldCell>>)await controller.GetAllSessionMoves(1L)).Value);
        }

        [Fact]
        public async Task AddNewSessionMoveTest()
        {
            var session = new Session(1L, 1L, 2L);
            var move = new FieldCellInputDto(new FieldCell(0, 1, session.Id, 1L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(session.Id).Result).Returns(session);
            mockFieldCellService.Setup(x => x.Add(FieldCellInputDto.MapToEntity(move)).Result).Returns(new FieldCell(1L, FieldCellInputDto.MapToEntity(move)));

            Assert.Equal(((JsonHttpResult<FieldCell>)Results.Json(new FieldCell(1L, FieldCellInputDto.MapToEntity(move)))).Value,
                         ((JsonHttpResult<FieldCell>)await controller.AddNewSessionMove(1L, move)).Value);

            session.Cells.Add(FieldCellInputDto.MapToEntity(move));

            move = null;
            mockFieldCellService.Setup(x => x.Add(FieldCellInputDto.MapToEntity(move)).Result).Returns(FieldCellInputDto.MapToEntity(move));
            
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.FieldBadRequest(1L, 0L))).StatusCode,
                         ((BadRequest<StandardProblem>)await controller.AddNewSessionMove(1L, move)).StatusCode);
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.FieldBadRequest(1L, 0L))).Value,
                         ((BadRequest<StandardProblem>)await controller.AddNewSessionMove(1L, move)).Value);

            var existedMove = new FieldCellInputDto(new FieldCell(0, 1, session.Id, 2L));

            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(1L))).StatusCode,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, existedMove)).StatusCode);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(1L))).ProblemDetails.Title,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, existedMove)).ProblemDetails.Title);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(1L))).ProblemDetails.Detail,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, existedMove)).ProblemDetails.Detail);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(1L))).ProblemDetails.Instance,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, existedMove)).ProblemDetails.Instance);

            var notCurrentPlayerMove = new FieldCellInputDto(new FieldCell(2, 1, session.Id, 1L));

            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotCurrentTurnPlayer(1L))).StatusCode,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, notCurrentPlayerMove)).StatusCode);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotCurrentTurnPlayer(1L))).ProblemDetails.Title,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, notCurrentPlayerMove)).ProblemDetails.Title);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotCurrentTurnPlayer(1L))).ProblemDetails.Detail,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, notCurrentPlayerMove)).ProblemDetails.Detail);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotCurrentTurnPlayer(1L))).ProblemDetails.Instance,
                         ((ProblemHttpResult)await controller.AddNewSessionMove(1L, notCurrentPlayerMove)).ProblemDetails.Instance);

            var secondMove = new FieldCellInputDto(new FieldCell(1, 1, session.Id, 2L));
            var thirdMove = new FieldCellInputDto(new FieldCell(0, 0, session.Id, 1L));
            var fourthMove = new FieldCellInputDto(new FieldCell(2, 2, session.Id, 2L));
            var fifthMove = new FieldCellInputDto(new FieldCell(0, 2, session.Id, 1L));
            session.Cells.Add(FieldCellInputDto.MapToEntity(secondMove));
            session.Cells.Add(FieldCellInputDto.MapToEntity(thirdMove));
            session.Cells.Add(FieldCellInputDto.MapToEntity(fourthMove));

            var fifthFieldCell = new FieldCell(5L, FieldCellInputDto.MapToEntity(fifthMove));
            mockFieldCellService.Setup(x => x.Add(FieldCellInputDto.MapToEntity(fifthMove)).Result).Returns(fifthFieldCell);

            fifthFieldCell.Session = session;

            Assert.True(((JsonHttpResult<FieldCell>)await controller.AddNewSessionMove(1L, fifthMove)).Value.Session.IsEnded);
        }

        [Fact]
        public async Task UpdateSessionMoveTest()
        {
            var session = new Session(1L, 1L, 2L);
            var move = new FieldCellInputDto(new FieldCell(0, 1, session.Id, 1L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(session.Id).Result).Returns(session);
            mockFieldCellService.Setup(x => x.FindById(2L).Result).Returns(new FieldCell(2L, FieldCellInputDto.MapToEntity(move)));
            move.Y = 2;
            mockFieldCellService.Setup(x => x.Update(2L, FieldCellInputDto.MapToEntity(move)).Result).Returns(new FieldCell(2L, FieldCellInputDto.MapToEntity(move)));

            Assert.Equal(((JsonHttpResult<FieldCell>)Results.Json(new FieldCell(2L, FieldCellInputDto.MapToEntity(move)))).Value,
                         ((JsonHttpResult<FieldCell>)await controller.UpdateSessionMove(session.Id, 2L, move)).Value);

            session.Cells.Add(new FieldCell(2L, FieldCellInputDto.MapToEntity(move)));

            move = null;
            mockFieldCellService.Setup(x => x.FindById(1L).Result).Returns(FieldCellInputDto.MapToEntity(move));

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(session.Id, 1L))).StatusCode,
                            ((NotFound<StandardProblem>)await controller.UpdateSessionMove(session.Id, 1L, move)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(session.Id, 1L))).Value,
                            ((NotFound<StandardProblem>)await controller.UpdateSessionMove(session.Id, 1L, move)).Value);

            var existedMove = new FieldCellInputDto(new FieldCell(0, 2, session.Id, 2L));

            mockFieldCellService.Setup(x => x.FindById(3L).Result).Returns(new FieldCell(3L, FieldCellInputDto.MapToEntity(existedMove)));

            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(session.Id, 3L))).StatusCode,
                         ((ProblemHttpResult)await controller.UpdateSessionMove(session.Id, 3L, existedMove)).StatusCode);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(1L, 3L))).ProblemDetails.Title,
                         ((ProblemHttpResult)await controller.UpdateSessionMove(session.Id, 3L, existedMove)).ProblemDetails.Title);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(1L, 3L))).ProblemDetails.Detail,
                         ((ProblemHttpResult)await controller.UpdateSessionMove(session.Id, 3L, existedMove)).ProblemDetails.Detail);
            Assert.Equal(((ProblemHttpResult)Results.Problem(StandardProblem.FieldIsNotEmpty(1L, 3L))).ProblemDetails.Instance,
                         ((ProblemHttpResult)await controller.UpdateSessionMove(session.Id, 3L, existedMove)).ProblemDetails.Instance);
        }
        
        [Fact]
        public async Task DeleteMoveByIdTest()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(2L).Result).Returns(new Session(2L, 2L, 1L));
            mockFieldCellService.Setup(x => x.DeleteById(1L).Result).Returns(true);

            Assert.Equal(((Ok<string>)Results.Ok($"Ход с id=1 удалён")).StatusCode,
                         ((Ok<string>)await controller.DeleteMoveByMoveId(2L, 1L)).StatusCode);
            Assert.Equal(((Ok<string>)Results.Ok($"Ход с id=1 удалён")).Value,
                         ((Ok<string>)await controller.DeleteMoveByMoveId(2L, 1L)).Value);

            mockFieldCellService.Setup(x => x.DeleteById(2L).Result).Returns(false);

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(2L, 2L))).StatusCode,
                         ((NotFound<StandardProblem>)await controller.DeleteMoveByMoveId(2L, 2L)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(2L, 2L))).Value,
                         ((NotFound<StandardProblem>)await controller.DeleteMoveByMoveId(2L, 2L)).Value);
        }
    }
}