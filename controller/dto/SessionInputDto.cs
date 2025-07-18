using mobibank_test.model;

namespace mobibank_test.controller.dto
{
    public class SessionInputDto
    {
        public long PlayerXId { get; set; }
        public long PlayerYId { get; set; }
        public long? WinnerId { get; set; }
        public bool IsEnded { get; set; }

        public SessionInputDto(Session session)
        {
            PlayerXId = session.PlayerXId;
            PlayerYId = session.PlayerYId;
            WinnerId = session.WinnerId;
            IsEnded = session.IsEnded;
        }

        public static Session? MapToEntity(SessionInputDto? sessionInputDto)
        {
            if (sessionInputDto != null)
            {
                int fieldSize = int.Parse(Environment.GetEnvironmentVariable("FIELD_SIZE"));
                Session session = new Session(fieldSize, sessionInputDto.PlayerXId, sessionInputDto.PlayerYId);
                session.WinnerId = sessionInputDto.WinnerId;
                session.IsEnded = sessionInputDto.IsEnded;

                return session;
            }
            else
                return null;
        }
    }
}