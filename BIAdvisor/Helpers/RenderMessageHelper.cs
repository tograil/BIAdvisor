﻿using BIAdvisor.Web.Infrastructure.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BIAdvisor.Web.Helpers
{
    public static class RenderMessageHelper
    {
        /// <summary>
        /// Render all messages that have been set during execution of the controller action.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static HtmlString RenderMessages(this HtmlHelper htmlHelper)
        {
            var messages = String.Empty;
            foreach (var messageType in Enum.GetNames(typeof(MessageType)))
            {
                var message = htmlHelper.ViewContext.ViewData.ContainsKey(messageType)
                                ? htmlHelper.ViewContext.ViewData[messageType]
                                : htmlHelper.ViewContext.TempData.ContainsKey(messageType)
                                    ? htmlHelper.ViewContext.TempData[messageType]
                                    : null;
                if (message != null)
                {
                    var messageBoxBuilder = new TagBuilder("div");
                    messageBoxBuilder.AddCssClass(String.Format("messagebox {0}", messageType.ToLowerInvariant()));
                    messageBoxBuilder.SetInnerText(message.ToString());
                    messages += messageBoxBuilder.ToString();
                }
            }
            return MvcHtmlString.Create(messages);
        }
    }
}