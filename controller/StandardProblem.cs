using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace mobibank_test.controller
{
    public class StandardProblem
    {
        public string Type { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }

        public StandardProblem(string type, HttpStatusCode status, string title, string detail, string instance)
        {
            Type = type;
            Status = status;
            Title = title;
            Detail = detail;
            Instance = instance;
        }

        public override bool Equals(object obj)
        {
            var problem = obj as StandardProblem;

            if (problem == null)
                return false;
            else
                return Type.Equals(problem.Type) && Status.Equals(problem.Status) && Title.Equals(problem.Title)
                       && Detail.Equals(problem.Detail) && Instance.Equals(problem.Instance);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() + Status.GetHashCode() + Title.GetHashCode() + Detail.GetHashCode() + Instance.GetHashCode();
        }

        public static StandardProblem UserNotFound(long id)
        {
            return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.NotFound,
                        title: "Пользователь не найден",
                        detail: $"Пользователь с id={id} не существует",
                        instance: $"/users/{id}");
        }

        public static StandardProblem UserBadRequest()
        {
            return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: "/users");
        }

        public static StandardProblem UserBadRequest(long id)
        {
            if (id > 0L)
            {
                return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: $"/users/{id}");
            }
            else
            {
                return UserBadRequest();
            }
        }

        public static StandardProblem SessionNotFound(long id)
        {
            return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.NotFound,
                        title: "Игра не найдена",
                        detail: $"Игра с id={id} не существует",
                        instance: $"/games/{id}");
        }

        public static StandardProblem SessionBadRequest()
        {
            return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: "/games");
        }

        public static StandardProblem SessionBadRequest(long id)
        {
            if (id > 0L)
            {
                return new StandardProblem(
                            type: "about:blank",
                            status: HttpStatusCode.BadRequest,
                            title: "Некорректные данные",
                            detail: "Отсутствуют необходимые поля",
                            instance: $"/games/{id}");
            }
            else
            {
                return SessionBadRequest();
            }
        }

        public static StandardProblem SessionBadRequest(long id, long moveId)
        {
            if (moveId > 0L)
            {
                return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: $"/games/{id}/moves/{moveId}");
            }
            else
            {
                return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: $"/games/{id}/moves");
            }
        }

        public static StandardProblem SessionNotFound(long id, long moveId)
        {
            if (moveId > 0L)
            {
                return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.NotFound,
                        title: "Игра не найдена",
                        detail: $"Игра с id={id} не существует",
                        instance: $"/games/{id}/moves/{moveId}");
            }
            else
            {
                return new StandardProblem(
                                type: "about:blank",
                                status: HttpStatusCode.NotFound,
                                title: "Игра не найдена",
                                detail: $"Игра с id={id} не существует",
                                instance: $"/games/{id}/moves");
            }
        }

        public static StandardProblem FieldNotFound(long id, long moveId)
        {
            return new StandardProblem(
                            type: "about:blank",
                            status: HttpStatusCode.NotFound,
                            title: "Ход не найден",
                            detail: $"Ход с id={moveId} не существует",
                            instance: $"/games/{id}/moves/{moveId}");
        }

        public static StandardProblem FieldBadRequest(long id)
        {
                return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: $"/{id}/moves");
        }

        public static StandardProblem FieldBadRequest(long id, long moveId)
        {
            if (moveId > 0L)
            {
                return new StandardProblem(
                        type: "about:blank",
                        status: HttpStatusCode.BadRequest,
                        title: "Некорректные данные",
                        detail: "Отсутствуют необходимые поля",
                        instance: $"/{id}/moves/{moveId}");
            }
            else
            {
                return FieldBadRequest(id);
            }
        }

        public static ProblemDetails FieldForbidden(long id, long moveId)
        {
            return new ProblemDetails
            {
                Type = "about:blank",
                Status = (int)HttpStatusCode.Forbidden,
                Title = "Некорректные данные",
                Detail = $"Ход с id={moveId} не принадлежит данной игре",
                Instance = $"/{id}/moves/{moveId}"
            };
        }
    }
}