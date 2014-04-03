﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MvcApp_MessageBoard.Services
{
    public class MockMailService:IMailService
    {
        public bool SendMail(string from, string to, string subject, string body)
        {
            Debug.WriteLine(string.Concat("SendMail: ",subject));
            return true;
        }
    }
}