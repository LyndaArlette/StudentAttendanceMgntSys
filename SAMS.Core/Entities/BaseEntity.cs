using System;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
