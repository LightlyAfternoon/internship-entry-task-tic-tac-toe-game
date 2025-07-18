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
            X = fieldCell.X;
            Y = fieldCell.Y;
            SessionId = fieldCell.SessionId;
            OccupiedByUserId = fieldCell.OccupiedByUserId;
        }

        public static FieldCell? MapToEntity(FieldCellInputDto? fieldCellInputDto)
        {
            if (fieldCellInputDto != null)
            {
                return new FieldCell(fieldCellInputDto.X, fieldCellInputDto.Y, fieldCellInputDto.SessionId, fieldCellInputDto.OccupiedByUserId);
            }
            else
            {
                return null;
            }
        }
    }
}