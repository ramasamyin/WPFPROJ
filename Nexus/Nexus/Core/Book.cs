using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Core {
   public class Book: IEntity{
      
      public int ID { get; set; }
      
      [Required]
      public string BookName { get; set; }
      
      [Required]
      public string Author { get; set; }
      
   }
}
