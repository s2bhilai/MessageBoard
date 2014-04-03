using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp_MessageBoard.Models
{
    public class ContactModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string comment { get; set; }

    }
}
