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
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
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
        [Column("player_o_id"), ForeignKey("PlayerY")]
        public long PlayerOId { get; set; }
        [Column("winner_id"), ForeignKey("Winner")]
        public long? WinnerId { get; set; }
        [Required]
        [Column("is_ended")]
        public bool IsEnded { get; set; }

        [JsonIgnore]
        [InverseProperty("SessionsX")]
        public virtual User PlayerX { get; set; }
        [JsonIgnore]
        [InverseProperty("SessionsO")]
        public virtual User PlayerO { get; set; }
        [JsonIgnore]
        [InverseProperty("SessionsWinners")]
        public virtual User? Winner { get; set; }
        // текущего игрока можно определять исходя из кол-ва сделанных ходов
        public virtual ICollection<FieldCell> Cells { get; set; }

        public Session()
        {
            FieldSize = 3;
            Cells = new List<FieldCell>();
        }

        public Session(long id, long playerXId, long playerOId)
        {
            Id = id;
            FieldSize = 3;
            PlayerXId = playerXId;
            PlayerOId = playerOId;
            Cells = new List<FieldCell>();
        }

        public Session(long playerXId, long playerOId)
        {
            FieldSize = 3;
            PlayerXId = playerXId;
            PlayerOId = playerOId;
            Cells = new List<FieldCell>();
        }

        public Session(long id, int fieldSize, long playerXId, long playerOId)
        {
            Id = id;
            FieldSize = fieldSize;
            PlayerXId = playerXId;
            PlayerOId = playerOId;
            Cells = new List<FieldCell>();
        }

        public Session(int fieldSize, long playerXId, long playerOId)
        {
            FieldSize = fieldSize;
            PlayerXId = playerXId;
            PlayerOId = playerOId;
            Cells = new List<FieldCell>();
        }

        public Session(long id, Session session)
        {
            Id = id;
            FieldSize = session.FieldSize;
            PlayerXId = session.PlayerXId;
            PlayerOId = session.PlayerOId;
            WinnerId = session.WinnerId;
            IsEnded = session.IsEnded;
            Cells = new List<FieldCell>();
        }

        public Session(Session session)
        {
            Id = session.Id;
            FieldSize = session.FieldSize;
            PlayerXId = session.PlayerXId;
            PlayerOId = session.PlayerOId;
            WinnerId = session.WinnerId;
            IsEnded = session.IsEnded;
            Cells = new List<FieldCell>();
        }

        public long GetCurrentTurnPlayerId()
        {
            if (Cells.Count % 2 == 0)
                return PlayerXId;
            else
                return PlayerOId;
        }

        public bool IsWinCondition(FieldCell cell)
        {
            int winCondition = int.Parse(Environment.GetEnvironmentVariable("WIN_CONDITION"));
            int winCountPlayer = 1;
            FieldCell[,] cells = new FieldCell[FieldSize, FieldSize];

            for (int x = 0; x < FieldSize; x++)
            {
                for (int y = 0; y < FieldSize; y++)
                {
                    cells[x, y] = Cells.FirstOrDefault(c => c.X == x && c.Y == y);
                }
            }

            int yI = cell.Y;

            while (yI < FieldSize)
            {
                if (cells[cell.X, yI] != null && cells[cell.X, yI].OccupiedByUserId == cell.OccupiedByUserId)
                {
                    winCountPlayer++;

                    if (winCountPlayer >= winCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }

                yI++;
            }

            if (cell.Y > 0)
            {
                yI = cell.Y - 1;

                while (yI > -1)
                {
                    if (cells[cell.X, yI] != null && cells[cell.X, yI].OccupiedByUserId == cell.OccupiedByUserId)
                    {
                        winCountPlayer++;

                        if (winCountPlayer >= winCondition)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }

                    yI--;
                }
            }

            winCountPlayer = 0;

            int xI = cell.X;

            while (xI < FieldSize)
            {
                if (cells[xI, cell.Y] != null && cells[xI, cell.Y].OccupiedByUserId == cell.OccupiedByUserId)
                {
                    winCountPlayer++;

                    if (winCountPlayer >= winCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }

                xI++;
            }

            if (cell.X > 0)
            {
                xI = cell.X - 1;

                while (xI > -1)
                {
                    if (cells[xI, cell.Y] != null && cells[xI, cell.Y].OccupiedByUserId == cell.OccupiedByUserId)
                    {
                        winCountPlayer++;

                        if (winCountPlayer >= winCondition)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }

                    xI--;
                }
            }

            winCountPlayer = 0;

            xI = cell.X + 1;
            yI = cell.Y + 1;

            while (xI < FieldSize && yI < FieldSize)
            {
                if (cells[xI, yI] != null && cells[xI, yI].OccupiedByUserId == cell.OccupiedByUserId)
                {
                    winCountPlayer++;

                    if (winCountPlayer >= winCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }

                xI++;
                yI++;
            }


            if (cell.X > 0 && cell.Y > 0)
            {
                xI = cell.X - 1;
                yI = cell.Y - 1;

                while (xI > -1 && yI > -1)
                {
                    if (cells[xI, yI] != null && cells[xI, yI].OccupiedByUserId == cell.OccupiedByUserId)
                    {
                        winCountPlayer++;

                        if (winCountPlayer >= winCondition)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }

                    xI--;
                    yI--;
                }
            }

            winCountPlayer = 0;

            xI = cell.X - 1;
            yI = cell.Y + 1;

            while (xI > -1 && yI < FieldSize)
            {
                if (cells[xI, yI] != null && cells[xI, yI].OccupiedByUserId == cell.OccupiedByUserId)
                {
                    winCountPlayer++;

                    if (winCountPlayer >= winCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }

                xI--;
                yI++;
            }

            if (cell.X > 0 && cell.Y < FieldSize)
            {
                xI = cell.X + 1;
                yI = cell.Y - 1;

                while (xI < FieldSize && yI > -1)
                {
                    if (cells[xI, yI] != null && cells[xI, yI].OccupiedByUserId == cell.OccupiedByUserId)
                    {
                        winCountPlayer++;

                        if (winCountPlayer >= winCondition)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }

                    xI++;
                    yI--;
                }
            }

            return false;
        }

        public bool AllFieldIsFull()
        {
            FieldCell[,] cells = new FieldCell[FieldSize, FieldSize];

            for (int x = 0; x < FieldSize; x++)
            {
                for (int y = 0; y < FieldSize; y++)
                {
                    cells[x, y] = Cells.FirstOrDefault(c => c.X == x && c.Y == y);
                }
            }

            for (int x = 0; x < FieldSize; x++)
            {
                for (int y = 0; y < FieldSize; y++)
                {
                    if (cells[x, y] == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            var session = obj as Session;

            if (session == null)
                return false;
            else
                return Id.Equals(session.Id) && FieldSize.Equals(session.FieldSize)
                && PlayerXId.Equals(session.PlayerXId) && PlayerOId.Equals(session.PlayerOId)
                && WinnerId.Equals(session.WinnerId) && IsEnded.Equals(session.IsEnded);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + FieldSize.GetHashCode() + PlayerXId.GetHashCode()
            + PlayerOId.GetHashCode() + WinnerId.GetHashCode() + IsEnded.GetHashCode();
        }
    }
}