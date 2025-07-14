using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace mobibank_test.model
{
    [Table("session")]
    public class Session
    {
        [Required]
        [Key]
        [Column("id")]
        public long Id { get; private set; }
        [Required]
        [Column("field_size")]
        public int FieldSize { get; set; }
        // ходит первым
        [Required]
        [Column("player_x_id"), ForeignKey("PlayerX")]
        public long PlayerXId { get; set; }
        [Required]
        [Column("player_y_id"), ForeignKey("PlayerY")]
        public long PlayerYId { get; set; }
        [Column("winner_id"), ForeignKey("Winner")]
        public long WinnerId { get; set; }
        [Required]
        [Column("is_ended")]
        public bool IsEnded { get; set; }

        [JsonIgnore]
        [InverseProperty("SessionsX")]
        public virtual User PlayerX { get; set; }
        [JsonIgnore]
        [InverseProperty("SessionsY")]
        public virtual User PlayerY { get; set; }
        [JsonIgnore]
        [InverseProperty("SessionsWinners")]
        public virtual User? Winner { get; set; }
        // текущего игрока можно определять исходя из кол-ва сделанных ходов
        public virtual ICollection<FieldCell> Cells { get; set; }

        public Session() {}

        public Session(long id, long playerXId, long playerYId)
        {
            Id = id;
            FieldSize = 3;
            PlayerXId = playerXId;
            PlayerYId = playerYId;
        }

        public Session(long playerXId, long playerYId)
        {
            FieldSize = 3;
            PlayerXId = playerXId;
            PlayerYId = playerYId;
        }

        public Session(long id, int fieldSize, long playerXId, long playerYId)
        {
            Id = id;
            FieldSize = fieldSize;
            PlayerXId = playerXId;
            PlayerYId = playerYId;
        }

        public Session(int fieldSize, long playerXId, long playerYId)
        {
            FieldSize = fieldSize;
            PlayerXId = playerXId;
            PlayerYId = playerYId;
        }

        public Session(long id, Session session)
        {
            Id = id;
            FieldSize = session.FieldSize;
            PlayerXId = session.PlayerXId;
            PlayerYId = session.PlayerYId;
            WinnerId = session.WinnerId;
            IsEnded = session.IsEnded;
        }

        public Session(Session session)
        {
            Id = session.Id;
            FieldSize = session.FieldSize;
            PlayerXId = session.PlayerXId;
            PlayerYId = session.PlayerYId;
            WinnerId = session.WinnerId;
            IsEnded = session.IsEnded;
        }

        public User GetCurrentTurnPlayer()
        {
            if (Cells.Count % 2 == 0)
                return PlayerX;
            else
                return PlayerY;
        }

        public override bool Equals(object obj)
        {
            var session = obj as Session;

            if (session == null)
                return false;
            else
                return Id.Equals(session.Id) && FieldSize.Equals(session.FieldSize)
                && PlayerXId.Equals(session.PlayerXId) && PlayerYId.Equals(session.PlayerYId)
                && WinnerId.Equals(session.WinnerId) && IsEnded.Equals(session.IsEnded);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + FieldSize.GetHashCode() + PlayerXId.GetHashCode()
            + PlayerYId.GetHashCode() + WinnerId.GetHashCode() + IsEnded.GetHashCode();
        }
    }
}