using System;
using System.Collections.Generic;
using System.Data;
using DnsClient.Internal;
using eXtensionSharp;
using JPlugin;
using JWLibrary.Database;
using MailSenderPlugin.Data;
using Microsoft.Data.SqlClient;
using RepoDb;

namespace MailSenderPlugin
{
    public class MailSenderExecutor : IPlugin
    {
        private IEnumerable<TB_SEND_MAIL> _sendMailItems;
        private readonly ILogger _logger;

        public MailSenderExecutor(ILogger logger)
        {
            this._logger = logger;
        }

        public void SetRequest<TRequest>(TRequest request)
        {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecute((db, tran) =>
                {
                    this._sendMailItems = db.Query<TB_SEND_MAIL>("SELECT TOP 100 * FROM TB_SEND_MAIL WHERE ISNULL(SEND_YN, 'N') = 'N'");
                });
        }

        public bool Validate()
        {
            if (_sendMailItems.xIsEmpty())
            {
                Console.WriteLine($"mail send list is empty");
                return false;
            }
            return true;
        }

        public bool PreExecute()
        {
            Console.WriteLine("Mail Sender : PreExecute");
            return true;
        }

        public void Execute()
        {
            var helper = new SmtpHelper("[mail host]", "[userid]", "[pwd]", 443, true);

            helper.AddMailList(this._sendMailItems);
            helper.SendResultEvent += (s, e) =>
            {
                e.SEND_YN = "Y";
                Console.WriteLine($"send result : {e.xToJson()}");
                JDatabaseResolver.Resolve<SqlConnection>()
                    .DbExecute((db, tran) =>
                    {
                        db.ExecuteNonQuery($"UPDATE TB_SEND_MAIL SET SEND_YN = 'Y' WHERE ID = {e.ID}");
                    });
            };
            helper.Send();
            
            Console.WriteLine("Mail Sender : Execute");
        }

        public bool AfterExecute()
        {
            return true;
        }
    }
}