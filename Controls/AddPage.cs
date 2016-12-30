using System;
using System.Threading;
using System.Windows.Forms;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class AddPage : UserControl
    {
        private int current;
        private int interval;
        private string[] links = {};

        public AddPage()
        {
            InitializeComponent();
        }

        public void DoAdd(string[] url)
        {
            var t = new Thread(() => xDoAdd(url));
            t.IsBackground = true;
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }

        private void xDoAdd(string[] url)
        {
            Thread.Sleep(5000 + interval);
            links = url;
            current = 0;
            interval = 0;
            if (links.Length > 0)
                dolink(links[0]);
        }

        private void dolink(string link)
        {
            if (webBrowser1.InvokeRequired)
                webBrowser1.Invoke(new MethodInvoker(() => webBrowser1.Navigate(link)));
            else
                webBrowser1.Navigate(link);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var t = new Thread(InvokeClick);
            t.IsBackground = true;
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }

        private void InvokeClick()
        {
            try
            {
                HtmlElementCollection collection = null;
                if (webBrowser1.InvokeRequired)
                    webBrowser1.Invoke(new MethodInvoker(() => collection = webBrowser1.Document.All));
                else
                    collection = webBrowser1.Document.All;
                foreach (HtmlElement hem in collection)
                {
                    if (hem.GetAttribute("id").Contains("skip_button"))
                    {
                        hem.InvokeMember("Click");
                        break;
                    }
                }
                Thread.Sleep(1000);
                if (webBrowser1.InvokeRequired)
                    webBrowser1.Invoke(new Action(() => webBrowser1.Stop()));
                else
                    webBrowser1.Stop();
                Thread.Sleep(6000 + interval);
                current++;
                interval += 100 + current;
                if (current < links.Length)
                    dolink(links[current]);
            }
            catch
            {
            }
        }
    }
}