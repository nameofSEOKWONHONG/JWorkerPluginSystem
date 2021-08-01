using System;
using System.Collections.Generic;
using System.Linq;
using MailSenderPlugin.Data;

namespace MailSenderPlugin
{
    public class SmtpHelper
    {
        string _host;
        string _userId;
        string _password;
        int _port;
        bool _enableSsl;

        public List<TB_SEND_MAIL> SendList { get; private set; }
        
        public event Action<object, TB_SEND_MAIL> SendResultEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        /// <param name="enableSsl">gmail = true</param>
        public SmtpHelper(string host, string userid, string password, int port, bool enableSsl)
        {
            this._host = host;
            this._userId = userid;
            this._password = password;
            this._port = port;
            this._enableSsl = enableSsl;
        }

        public SmtpHelper AddMailList(IEnumerable<TB_SEND_MAIL> requestItems)
        {
            this.SendList.AddRange(requestItems);
            return this;
        }

        public SmtpHelper AddMail(TB_SEND_MAIL requestItem)
        {
            this.SendList.Add(requestItem);
            return this;
        }

        public void Send()
        {
            foreach (var item in this.SendList)
            {
                MailSend(item);
                if (SendResultEvent != null)
                {
                    SendResultEvent(this, item);
                }   
            }

            SendList.Clear();
        }

        private void MailSend(TB_SEND_MAIL requestDto)
        {
            var smtp = new Client(_host, _userId, _password, _port, _enableSsl);

            if (smtp != null)
            {
                smtp.Send(
                    requestDto.NAME, requestDto.FROM, requestDto.TO,
                    requestDto.SUBJECT, requestDto.BODY,
                    requestDto.ATT_FILES == null ? null : requestDto.ATT_FILES.ToArray());

                IDisposable dispose = smtp as IDisposable;

                if (dispose != null) dispose.Dispose();
            }
        }
    }
}