using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entidades.Model
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Gross { get; set; }

        public double Rating { get; set; }

        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual ICollection<ActorMovie> Characters { get; set; }
    }

}
