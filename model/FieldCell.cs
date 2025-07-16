using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace mobibank_test.model
{
    [Table("field_cell")]
    public class FieldCell
    {
        [Required]
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; private set; }
        [Required]
        [Column("x")]
        public int X { get; set; }
        [Required]
        [Column("y")]
        public int Y { get; set; }
        [Required]
        [Column("session_id"), ForeignKey("Session")]
        public long SessionId { get; set; }
        [Column("ocupied_by_user_id"), ForeignKey("OccupiedByUser")]
        public long OccupiedByUserId { get; set; }

        [JsonIgnore]
        public virtual User OccupiedByUser { get; set; }
        [JsonIgnore]
        public virtual Session Session { get; set; }

        public FieldCell() {}

        public FieldCell(long id, int x, int y, long sessionId)
        {
            Id = id;
            X = x;
            Y = y;
            SessionId = sessionId;
        }

        public FieldCell(int x, int y, long sessionId)
        {
            X = x;
            Y = y;
            SessionId = sessionId;
        }

        public FieldCell(long id, int x, int y, long sessionId, long occupiedByUserId)
        {
            Id = id;
            X = x;
            Y = y;
            SessionId = sessionId;
            OccupiedByUserId = occupiedByUserId;
        }

        public FieldCell(int x, int y, long sessionId, long occupiedByUserId)
        {
            X = x;
            Y = y;
            SessionId = sessionId;
            OccupiedByUserId = occupiedByUserId;
        }

        public FieldCell(long id, FieldCell cell)
        {
            Id = id;
            X = cell.X;
            Y = cell.Y;
            SessionId = cell.SessionId;
            OccupiedByUserId = cell.OccupiedByUserId;
        }

        public FieldCell(FieldCell cell)
        {
            Id = cell.Id;
            X = cell.X;
            Y = cell.Y;
            SessionId = cell.SessionId;
            OccupiedByUserId = cell.OccupiedByUserId;
        }

        public override bool Equals(object obj)
        {
            var fieldCell = obj as FieldCell;

            if (fieldCell == null)
                return false;
            else
                return Id.Equals(fieldCell.Id) && X.Equals(fieldCell.X)
                && Y.Equals(fieldCell.Y) && SessionId.Equals(fieldCell.SessionId)
                && OccupiedByUserId.Equals(fieldCell.OccupiedByUserId);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + X.GetHashCode() + Y.GetHashCode()
            + SessionId.GetHashCode() + OccupiedByUserId.GetHashCode();
        }
    }
}