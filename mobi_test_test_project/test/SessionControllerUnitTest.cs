using System.Net;
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
        [Fact]
        public void GetSessionByIdTest()
        {
            var session = new Session(1L, 1L, 2L);
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(1L)).Returns(new Session(session));

            Assert.Equal(((JsonHttpResult<Session>)Results.Json(session)).Value,
                         ((JsonHttpResult<Session>)controller.GetSessionById(1)).Value);

            session.PlayerYId = 3L;

            Assert.NotEqual(((JsonHttpResult<Session>)Results.Json(session)).Value,
                         ((JsonHttpResult<Session>)controller.GetSessionById(1)).Value);
        }

        [Fact]
        public void GetAllSessionsTest()
        {
            var session1 = new Session(1L, 1L, 2L);
            var session2 = new Session(2L, 2L, 5L);
            var sessions = new List<Session> { session1, session2 };
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindAll()).Returns(new List<Session> { new Session(session1), new Session(session2) });

            Assert.Equal(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value, ((JsonHttpResult<List<Session>>)controller.GetAllSessions()).Value);

            session1.PlayerYId = 3L;

            Assert.NotEqual(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value, ((JsonHttpResult<List<Session>>)controller.GetAllSessions()).Value);

            session1.PlayerYId = 2L;

            Assert.Equal(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value, ((JsonHttpResult<List<Session>>)controller.GetAllSessions()).Value);

            sessions = new List<Session> { session1 };

            Assert.NotEqual(((JsonHttpResult<List<Session>>)Results.Json(sessions)).Value, ((JsonHttpResult<List<Session>>)controller.GetAllSessions()).Value);
        }

        [Fact]
        public void AddNewSessionTest()
        {
            var session = new SessionInputDto(new Session(1L, 2L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.Add(SessionInputDto.MapToEntity(session))).Returns(new Session(1L, SessionInputDto.MapToEntity(session)));

            Assert.Equal(((JsonHttpResult<Session>)Results.Json(new Session(1L, SessionInputDto.MapToEntity(session)))).Value,
                         ((JsonHttpResult<Session>)controller.AddNewSession(session)).Value);

            session = null;
            mockSessionService.Setup(x => x.Add(SessionInputDto.MapToEntity(session))).Returns(SessionInputDto.MapToEntity(session));

            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.SessionBadRequest())).StatusCode,
                         ((BadRequest<StandardProblem>)controller.AddNewSession(session)).StatusCode);
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.SessionBadRequest())).Value,
                         ((BadRequest<StandardProblem>)controller.AddNewSession(session)).Value);
        }

        [Fact]
        public void UpdateSessionTest()
        {
            var session = new SessionInputDto(new Session(1, 1L, 2L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(2L)).Returns(new Session(2L, SessionInputDto.MapToEntity(session)));
            session.PlayerYId = 3L;
            mockSessionService.Setup(x => x.Update(2L, SessionInputDto.MapToEntity(session))).Returns(new Session(2L, SessionInputDto.MapToEntity(session)));

            Assert.Equal(((JsonHttpResult<Session>)Results.Json(new Session(2L, SessionInputDto.MapToEntity(session)))).Value,
                         ((JsonHttpResult<Session>)controller.UpdateSession(2L, session)).Value);

            session = null;
            mockSessionService.Setup(x => x.FindById(1L)).Returns(SessionInputDto.MapToEntity(session));
            mockSessionService.Setup(x => x.Update(1L, SessionInputDto.MapToEntity(session))).Returns(SessionInputDto.MapToEntity(session));
            
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(1L))).StatusCode,
                            ((NotFound<StandardProblem>)controller.UpdateSession(1L, session)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(1L))).Value,
                            ((NotFound<StandardProblem>)controller.UpdateSession(1L, session)).Value);
        }
        
        [Fact]
        public void DeleteSessionByIdTest()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.DeleteById(1L)).Returns(true);

            Assert.Equal(((Ok<string>)Results.Ok($"Игра с id=1 удалена")).StatusCode,
                         ((Ok<string>)controller.DeleteSessionById(1L)).StatusCode);
            Assert.Equal(((Ok<string>)Results.Ok($"Игра с id=1 удалена")).Value,
                         ((Ok<string>)controller.DeleteSessionById(1L)).Value);

            mockSessionService.Setup(x => x.DeleteById(2L)).Returns(false);

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(2L))).StatusCode,
                         ((NotFound<StandardProblem>)controller.DeleteSessionById(2L)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.SessionNotFound(2L))).Value,
                         ((NotFound<StandardProblem>)controller.DeleteSessionById(2L)).Value);
        }

        [Fact]
        public void GetAllSessionMovesTest()
        {
            var session = new Session(1L, 1L, 2L);
            var move1 = new FieldCell(1L, 0, 1, 1L, session.Id);
            var move2 = new FieldCell(2L, 1, 1, 1L, session.Id);
            var moves = new List<FieldCell> { move1, move2 };
            session.Cells = moves;
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockFieldCellService.Setup(x => x.FindAllBySessionId(1L)).Returns(new List<FieldCell> { new FieldCell(move1), new FieldCell(move2) });

            Assert.Equal(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                         ((JsonHttpResult<List<FieldCell>>)controller.GetAllSessionMoves(1L)).Value);

            move1.Y = 2;

            Assert.NotEqual(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                            ((JsonHttpResult<List<FieldCell>>)controller.GetAllSessionMoves(1L)).Value);

            move1.Y = 1;

            Assert.Equal(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                         ((JsonHttpResult<List<FieldCell>>)controller.GetAllSessionMoves(1L)).Value);

            moves = new List<FieldCell> { move1 };

            Assert.NotEqual(((JsonHttpResult<List<FieldCell>>)Results.Json(moves)).Value,
                            ((JsonHttpResult<List<FieldCell>>)controller.GetAllSessionMoves(1L)).Value);
        }

        [Fact]
        public void AddNewSessionMoveTest()
        {
            var session = new Session(1L, 1L, 2L);
            var move = new FieldCellInputDto(new FieldCell(0, 1, session.Id, 1L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(session.Id)).Returns(session);
            mockFieldCellService.Setup(x => x.Add(FieldCellInputDto.MapToEntity(move))).Returns(new FieldCell(1L, FieldCellInputDto.MapToEntity(move)));

            Assert.Equal(((JsonHttpResult<FieldCell>)Results.Json(new FieldCell(1L, FieldCellInputDto.MapToEntity(move)))).Value,
                         ((JsonHttpResult<FieldCell>)controller.AddNewSessionMove(1L, move)).Value);

            move = null;
            mockFieldCellService.Setup(x => x.Add(FieldCellInputDto.MapToEntity(move))).Returns(FieldCellInputDto.MapToEntity(move));
            
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.FieldBadRequest(1L, 0L))).StatusCode,
                         ((BadRequest<StandardProblem>)controller.AddNewSessionMove(1L, move)).StatusCode);
            Assert.Equal(((BadRequest<StandardProblem>)Results.BadRequest(StandardProblem.FieldBadRequest(1L, 0L))).Value,
                         ((BadRequest<StandardProblem>)controller.AddNewSessionMove(1L, move)).Value);
        }

        [Fact]
        public void UpdateSessionMoveTest()
        {
            var session = new Session(1L, 1L, 2L);
            var move = new FieldCellInputDto(new FieldCell(0, 1, session.Id, 1L));
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(session.Id)).Returns(session);
            mockFieldCellService.Setup(x => x.FindById(2L)).Returns(new FieldCell(2L, FieldCellInputDto.MapToEntity(move)));
            move.Y = 2;
            mockFieldCellService.Setup(x => x.Update(2L, FieldCellInputDto.MapToEntity(move))).Returns(new FieldCell(2L, FieldCellInputDto.MapToEntity(move)));

            Assert.Equal(((JsonHttpResult<FieldCell>)Results.Json(new FieldCell(2L, FieldCellInputDto.MapToEntity(move)))).Value,
                         ((JsonHttpResult<FieldCell>)controller.UpdateSessionMove(session.Id, 2L, move)).Value);

            move = null;
            mockFieldCellService.Setup(x => x.FindById(1L)).Returns(FieldCellInputDto.MapToEntity(move));

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(session.Id, 1L))).StatusCode,
                            ((NotFound<StandardProblem>)controller.UpdateSessionMove(session.Id, 1L, move)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(session.Id, 1L))).Value,
                            ((NotFound<StandardProblem>)controller.UpdateSessionMove(session.Id, 1L, move)).Value);
        }
        
        [Fact]
        public void DeleteMoveByIdTest()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockFieldCellService = new Mock<IFieldCellService>();
            var controller = new SessionController(mockSessionService.Object, mockFieldCellService.Object);

            mockSessionService.Setup(x => x.FindById(2L)).Returns(new Session(2L, 2L, 1L));
            mockFieldCellService.Setup(x => x.DeleteById(1L)).Returns(true);

            Assert.Equal(((Ok<string>)Results.Ok($"Ход с id=1 удалён")).StatusCode,
                         ((Ok<string>)controller.DeleteMoveByMoveId(2L, 1L)).StatusCode);
            Assert.Equal(((Ok<string>)Results.Ok($"Ход с id=1 удалён")).Value,
                         ((Ok<string>)controller.DeleteMoveByMoveId(2L, 1L)).Value);

            mockFieldCellService.Setup(x => x.DeleteById(2L)).Returns(false);

            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(2L, 2L))).StatusCode,
                         ((NotFound<StandardProblem>)controller.DeleteMoveByMoveId(2L, 2L)).StatusCode);
            Assert.Equal(((NotFound<StandardProblem>)Results.NotFound(StandardProblem.FieldNotFound(2L, 2L))).Value,
                         ((NotFound<StandardProblem>)controller.DeleteMoveByMoveId(2L, 2L)).Value);
        }
    }
}