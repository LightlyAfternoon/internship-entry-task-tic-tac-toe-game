using mobibank_test.model;

namespace mobibank_test.controller.dto
{
    public class FieldCellInputDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public long SessionId { get; set; }
        public long OccupiedByUserId { get; set; }

        public FieldCellInputDto(FieldCell fieldCell)
        {
            fieldCell.X = X;
            fieldCell.Y = Y;
            fieldCell.SessionId = SessionId;
            fieldCell.OccupiedByUserId = OccupiedByUserId;
        }

        public FieldCell MapToEntity()
        {
            return new FieldCell(X, Y, SessionId, OccupiedByUserId);
        }
    }
}