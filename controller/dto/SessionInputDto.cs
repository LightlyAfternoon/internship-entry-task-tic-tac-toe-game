using mobibank_test.model;

namespace mobibank_test.controller.dto
{
    public class SessionInputDto
    {
        public int FieldSize { get; set; }
        public long PlayerXId { get; set; }
        public long PlayerYId { get; set; }
        public long? WinnerId { get; set; }
        public bool IsEnded { get; set; }

        public SessionInputDto(Session session)
        {
            FieldSize = session.FieldSize;
            PlayerXId = session.PlayerXId;
            PlayerYId = session.PlayerYId;
            WinnerId = session.WinnerId;
            IsEnded = session.IsEnded;
        }

        public static Session MapToEntity(SessionInputDto sessionInputDto)
        {
            if (sessionInputDto != null)
            {
                Session session = new Session(sessionInputDto.FieldSize, sessionInputDto.PlayerXId, sessionInputDto.PlayerYId);
                session.WinnerId = sessionInputDto.WinnerId;
                session.IsEnded = sessionInputDto.IsEnded;

                return session;
            }
            else
                return null;
        }
    }
}