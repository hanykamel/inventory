using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExaminationCommittee.Commands
{
  public  class EditExaminationCommandVM
    {

        public Guid Id { get; set; }
        public bool isEditSuccess { get; set; }
        public List<ValidationMassage> massage { get; set; }
    }

    public class ValidationMassage
    {

        public bool isSuccess { get; set; }
        public long Id { get; set; }
        public string BaseItemName { get; set; }
        public List<string>  message { get; set; }

        public bool Isbudget { get; set; }

    }
}
