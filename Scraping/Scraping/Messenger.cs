using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping
{
    /// <summary>
    /// これが何か分からない
    /// </summary>
    class Messenger : EventAggregator
    {
        private static Messenger _instance;

        public static Messenger Instance
        {
            get { return _instance ?? (_instance = new Messenger()); }
        }
    }
}
