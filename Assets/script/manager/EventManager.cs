using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.manager
{
    class EventManager
    {
        static private EventManager _instance;
        static public EventManager getIntance()
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }


       //static public event events.MyEventArgs.MyHandler CreateRoleEnd;
    }
   

}
