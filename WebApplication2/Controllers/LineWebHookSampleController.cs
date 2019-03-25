using isRock.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class LineBotWebHookController : LineWebHookControllerBase
    {
        string channelAccessToken = WebConfigurationManager.AppSettings["channelAccessToken"];
        string AdminUserId = WebConfigurationManager.AppSettings["AdminUserId"];

        [Route("api/LineWebHookSample")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();

                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    isRock.LineBot.Bot bot = new isRock.LineBot.Bot(ChannelAccessToken);
                    var userinfo = bot.GetUserInfo(LineEvent.source.userId);
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        switch (LineEvent.message.text)
                        {
                            case "/上一頁":
                                SwitchMenuTo("快捷選單1", LineEvent);
                                break;
                            case "/下一頁":
                                SwitchMenuTo("快捷選單2", LineEvent);
                                break;
                            default:
                                this.ReplyMessage(LineEvent.replyToken, "你說了:" + LineEvent.message.text);
                                break;
                        }
                        this.ReplyMessage(LineEvent.replyToken, userinfo.displayName + "你說了:" + LineEvent.message.text
                          + " 你的用戶ID是" + LineEvent.source.userId);
                    }
                    if (LineEvent.message.type == "sticker") //收到貼圖
                    { this.ReplyMessage(LineEvent.replyToken, 1, 2); }
                }
                //回覆訊息
                if (LineEvent.type == "image")
                {
                    string path = System.Web.HttpContext.Current.Request.MapPath("/temp/");
                    string filename = Guid.NewGuid().ToString() + ".png";
                    var filebody = this.GetUserUploadedContent(LineEvent.message.id);
                    System.IO.File.WriteAllBytes(path + filename, filebody);
                }
                //檢查用戶如果當前沒有任何選單，則嘗試綁定快捷選單1
                var currentMenu = isRock.LineBot.Utility.GetRichMenuIdOfUser(LineEvent.source.userId, channelAccessToken);
                if (currentMenu == null || string.IsNullOrEmpty(currentMenu.richMenuId))
                {
                    SwitchMenuTo("快捷選單1", LineEvent);
                }
                if (isRock.LineBot.Utility.GetRichMenu(currentMenu.richMenuId, channelAccessToken) == null)
                {
                    SwitchMenuTo("快捷選單1", LineEvent);
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
        private bool SwitchMenuTo(string MenuName, Event LineEvent)
        {
            //抓取所有選單
            var menus = isRock.LineBot.Utility.GetRichMenuList(channelAccessToken);
            //列舉每一個
            foreach (var item in menus.richmenus)
            {
                //如果選單名稱為 MenuName
                if (item.name == MenuName)
                {
                    isRock.LineBot.Utility.LinkRichMenuToUser(item.richMenuId, LineEvent.source.userId, channelAccessToken);
                    return true;
                }
            }
            return false;
        }
    }
}
