using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.events
{
   public class MyEventArgs:EventArgs
    {
        private object message;

        public MyEventArgs(object obj = null)

        {

            this.message = obj;

        }

        public object Message

        {

            get { return message; }

        }



        public delegate void MyHandler(object sender, MyEventArgs e);
    }
}
