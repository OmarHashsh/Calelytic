using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Calelytic.Core.Enums;

namespace Calelytic.Core.Models
{
    public class Reminder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public TimeSpan TimeBeforeEvent { get; set; }

        [Required]
        public ReminderMethod Method { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }

        [Required]
        public Event? Event { get; set; }
    }
}
