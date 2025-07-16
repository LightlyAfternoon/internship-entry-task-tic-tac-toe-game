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

        public Session MapToEntity()
        {
            Session session = new Session(FieldSize, PlayerXId, PlayerYId);
            session.WinnerId = WinnerId;
            session.IsEnded = IsEnded;
            
            return session;
        }
    }
}