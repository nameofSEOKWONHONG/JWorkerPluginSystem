using System.Collections.Generic;

namespace MailSenderPlugin.Data
{
    public class TB_SEND_MAIL
    {
        public long ID { get; set; }
        public string NAME { get; set; }
        public string FROM { get; set; }
        public string TO { get; set; }
        public string SUBJECT { get; set; }
        public string BODY { get; set; }
        public List<string> ATT_FILES { get; set; }
        public string SEND_YN { get; set; }
    }
}