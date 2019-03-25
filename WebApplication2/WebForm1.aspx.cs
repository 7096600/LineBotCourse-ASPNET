using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using isRock.LineBot;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string channelAccessToken = WebConfigurationManager.AppSettings["channelAccessToken"];
        string AdminUserId = WebConfigurationManager.AppSettings["AdminUserId"];

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //ButtonsTemplate
        protected void Button1_Click(object sender, EventArgs e)
        {
            var bot = new isRock.LineBot.Bot(channelAccessToken);

            var ButtonTmpMsg = new isRock.LineBot.ButtonsTemplate()
            {
                text = "文字",
                title = "標題",
                altText = "替代文字",
                thumbnailImageUrl = new Uri("https://66.media.tumblr.com/2e37eafc9b6b715ed99b31fb6f72e6a5/tumblr_inline_pjjzfnFy7a1u06gc8_640.jpg")
            };
            //add actions
            var action1 = new isRock.LineBot.MessageAction()
            { label = "顯示的標題", text = "呈現的文字" };
            ButtonTmpMsg.actions.Add(action1);

            var action2 = new isRock.LineBot.UriAction()
            { label = "顯示的標題", uri = new Uri("http://www.google.com") };
            ButtonTmpMsg.actions.Add(action2);

            bot.PushMessage(AdminUserId, ButtonTmpMsg);
        }
        //ConfirmTemplate
        protected void Button2_Click(object sender, EventArgs e)
        {
            var bot = new isRock.LineBot.Bot(channelAccessToken);

            var CarouselTmpMsg = new isRock.LineBot.ConfirmTemplate()
            { 
                text = "文字",
                altText = "替代文字",
            };
            //add actions
            var action1 = new isRock.LineBot.MessageAction()
            { label = "OK", text = "呈現的文字" };
            CarouselTmpMsg.actions.Add(action1);

            var action2 = new isRock.LineBot.UriAction()
            { label = "NO", uri = new Uri("http://www.google.com") };
            CarouselTmpMsg.actions.Add(action2);

            bot.PushMessage(AdminUserId, CarouselTmpMsg);
        }
        //QuickReplyMessageAction
        protected void Button3_Click(object sender, EventArgs e)
        {
            //icon位置
            const string IconUrl = "https://pic.pimg.tw/chico386/1414112596-3072196168_n.png";

            //建立一個TextMessage物件
            isRock.LineBot.TextMessage m =
                new isRock.LineBot.TextMessage("請在底下選擇一個選項");

            //在TextMessage物件的quickReply屬性中加入items
            m.quickReply.items.Add(
                    new isRock.LineBot.QuickReplyMessageAction(
                        $"一般標籤", "點選後顯示的text文字"));

            m.quickReply.items.Add(
                new isRock.LineBot.QuickReplyMessageAction(
                    $"有圖示的標籤", "點選後顯示的text文字", new Uri(IconUrl)));
            //加入QuickReplyDatetimePickerAction
            m.quickReply.items.Add(
                new isRock.LineBot.QuickReplyDatetimePickerAction(
                    "選時間", "選時間", isRock.LineBot.DatetimePickerModes.datetime,
                    new Uri(IconUrl)));
            //加入QuickReplyLocationAction
            m.quickReply.items.Add(
                new isRock.LineBot.QuickReplyLocationAction(
                    "選地點", new Uri(IconUrl)));
            //加入QuickReplyCameraAction
            m.quickReply.items.Add(
                new isRock.LineBot.QuickReplyCameraAction(
                "Show Camera", new Uri(IconUrl)));
            //加入QuickReplyCamerarollAction
            m.quickReply.items.Add(
                new isRock.LineBot.QuickReplyCamerarollAction(
                "Show Cameraroll", new Uri(IconUrl)));
            //建立bot instance
            isRock.LineBot.Bot bot = new isRock.LineBot.Bot(channelAccessToken);
            //透過Push發送訊息
            bot.PushMessage(AdminUserId, m);
        }
        //一次發送多則訊息
        protected void Button4_Click(object sender, EventArgs e)
        {
            Bot bot = new Bot(channelAccessToken);

            var msg = new List<MessageBase>();
            msg.Add(new TextMessage("這是個文字"));
            msg.Add(new StickerMessage(1,11));

            bot.PushMessage(AdminUserId, msg);
        }
    }
}