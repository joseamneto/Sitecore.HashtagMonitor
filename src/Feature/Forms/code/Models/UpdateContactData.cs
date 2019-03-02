using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoHorse.Feature.Forms.Models
{
    public class UpdateContactData
    {
        public Guid FirstNameFieldId { get; set; }
        public Guid LastNameFieldId { get; set; }
        public Guid EmailFieldId { get; set; }
        public Guid TwitterAccountFieldId { get; set; }
    }
}