using isRock.LineBot;
using System;
using System.Web.Configuration;

namespace WebApplication2
{
    public partial class _default : System.Web.UI.Page
    {
        string channelAccessToken = WebConfigurationManager.AppSettings["channelAccessToken"];
        string AdminUserId= WebConfigurationManager.AppSettings["AdminUserId"];      

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, $"測試 {DateTime.Now.ToString()} ! ");
            bot.PushMessage(AdminUserId, 1, 11);
            bot.PushMessage(AdminUserId, new Uri("https://pic.pimg.tw/chico386/1414112596-3072196168_n.png"));
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, 1,2);
        }
    }
}