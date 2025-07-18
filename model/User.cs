using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace mobibank_test.model
{
    [Table("user")]
    public class User
    {
        [Required]
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; private set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Session> SessionsX { get; set; }
        [JsonIgnore]
        public virtual ICollection<Session> SessionsY { get; set; }
        [JsonIgnore]
        public virtual ICollection<Session> SessionsWinners { get; set; }

        public User() {}

        public User(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public User(string name)
        {
            Name = name;
        }

        public User(long id, User user)
        {
            Id = id;
            Name = user.Name;
        }

        public User(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }

        public override bool Equals(object? obj)
        {
            var user = obj as User;

            if (user == null)
                return false;
            else
                return Id.Equals(user.Id) && Name.Equals(user.Name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + Name.GetHashCode();
        }
    }
}