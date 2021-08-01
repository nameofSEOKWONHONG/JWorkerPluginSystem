﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailSenderPlugin
{
    internal class Client : IDisposable
    {
        public string Host { get; private set; }
        public string Userid { get; private set; }
        public string Passwd { get; private set; }
        public int Port { get; private set; }
        public string AttFileName { get; private set; }

        private SmtpClient objSmtpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        /// <param name="enableSsl">gmail : true</param>
        public Client(string host, string userid, string password, int port, bool enableSsl)
        {
            this.Host = host;
            this.Userid = userid;
            this.Passwd = password;
            this.Port = port;

            objSmtpClient = new SmtpClient(Host); // Server IP
            objSmtpClient.Port = Port;
            objSmtpClient.UseDefaultCredentials = false;
            objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            objSmtpClient.EnableSsl = enableSsl;

            System.Net.NetworkCredential objSMTPUserInfo = new System.Net.NetworkCredential(Userid, Passwd);            
            objSmtpClient.Credentials = objSMTPUserInfo;
        }

        public void Send(string name
                            , string strFrom
                            , string strTo
                            , string strSubject
                            , string strBody
                            , string[] attachmentFilenames)
        {
            MailMessage objMailMessage = new MailMessage();

            objMailMessage.From = new MailAddress(strFrom, name);
            objMailMessage.To.Add(new MailAddress(strTo));
            objMailMessage.BodyEncoding = Encoding.UTF8;//Encoding.GetEncoding("EUC-KR");                
            objMailMessage.IsBodyHtml = true;
            objMailMessage.Subject = strSubject;
            objMailMessage.Body = strBody;

            string filePath = string.Empty;
            if (attachmentFilenames != null)
            {
                foreach (string item in attachmentFilenames)
                {
                    filePath = "";
                    if (!string.IsNullOrEmpty(item))
                    {
                        filePath = AttFileName + item;

                        if (File.Exists(filePath))
                        {
                            System.Net.Mail.Attachment attachment;
                            attachment = new System.Net.Mail.Attachment(filePath);
                            objMailMessage.Attachments.Add(attachment);
                        }
                    }
                }
            }
            objSmtpClient.Send(objMailMessage);
            objMailMessage.Dispose();
        }

        public void Dispose()
        {
            objSmtpClient.Dispose();
        }

        public Task SendAsync(string name
                                    , string strFrom
                                    , string strTo
                                    , string strSubject
                                    , string strBody
                                    , string[] attachmentFilenames)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Guid sendGuid = Guid.NewGuid();
            
            MailMessage objMailMessage = new MailMessage();

            objMailMessage.From = new MailAddress(strFrom, name);
            objMailMessage.To.Add(new MailAddress(strTo));
            objMailMessage.BodyEncoding = Encoding.UTF8;//Encoding.GetEncoding("EUC-KR");                
            objMailMessage.IsBodyHtml = true;
            objMailMessage.Subject = strSubject;
            objMailMessage.Body = strBody;

            string filePath = string.Empty;
            if (attachmentFilenames != null)
            {
                foreach (string item in attachmentFilenames)
                {
                    filePath = "";
                    if (!string.IsNullOrEmpty(item))
                    {
                        filePath = AttFileName + item;

                        if (File.Exists(filePath))
                        {
                            System.Net.Mail.Attachment attachment;
                            attachment = new System.Net.Mail.Attachment(filePath);
                            objMailMessage.Attachments.Add(attachment);
                        }
                    }
                }
            }

            SendCompletedEventHandler handler = null;
            handler = (o, ea) =>
            {
                if (ea.UserState is Guid && ((Guid)ea.UserState) == sendGuid)
                {
                    objSmtpClient.SendCompleted -= handler;
                    if (ea.Cancelled)
                    {
                        tcs.SetCanceled();
                    }
                    else if (ea.Error != null)
                    {
                        tcs.SetException(ea.Error);
                    }
                    else
                    {
                        tcs.SetResult(null);
                    }
                    objMailMessage.Dispose();
                }
            };
            objSmtpClient.SendCompleted += handler;
            objSmtpClient.SendAsync(objMailMessage, sendGuid);

            return tcs.Task;
        }
    }
}