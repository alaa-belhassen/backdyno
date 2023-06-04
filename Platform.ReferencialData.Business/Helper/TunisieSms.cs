using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferencialData.Business.Helper
{
    public class TunisieSmsService
    {
       public object sendmessage(string mobile,string text)
        {
            TunisieSMS.MyStudents.Sms _sms = new TunisieSMS.MyStudents.Sms();
            _sms.API_KEY = "14PcPKE1bvoMFf6AiLfU3G!1NpBPRS0WHRAinQlPLandHuo0GUVLmWOAMbcp4dMO5Y7u!uJMV70o2qqLMMi9u7LHlz7rdVFk4Of5J!54";
            _sms.SENDER = "TunSMS Test";
            _sms.MESSAGES.Add(new TunisieSMS.MyStudents.Entity.Message(mobile,text));
            object sendRequest = _sms.Send();
            return sendRequest;
        }

    }
}
