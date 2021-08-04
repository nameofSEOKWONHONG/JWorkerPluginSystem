using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JWorkerPluginSystemTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var template_object = new KakaoTemplateObjectBase();
            template_object.object_type = "feed";
            template_object.content = new TamplateContent()
            {

            };
            template_object.social = new TemplateSocial()
            {

            };
            template_object.buttons = new List<TemplateButton>()
            {

            };

            var s = JsonConvert.SerializeObject(template_object);
            Console.WriteLine(s);
        }
    }
    
    public interface IKakaoRequestObject { }

    public class KakaoRequestObject
    {
        public static IKakaoRequestObject CreateTempldateObject(string object_type)
        {
            if (object_type == "feed")
            {
                return new KakaoTemplateObjectBase();
            }
            else if(object_type == "list")
            {
                
            }

            return null;
        }
    }
    
    public class KakaoTemplateObjectBase : IKakaoRequestObject
    {
        public string object_type { get; set; }
        public TamplateContent content { get; set; }
        public TemplateSocial social { get; set; }
        public IEnumerable<TemplateButton> buttons { get; set; }
    }

    public class TamplateContent
    {
        public string title { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
        public TamplateContentLink link { get; set; }
    }

    public class TamplateContentLink
    {
        public string webUrl { get; set; }
        public string mobileWebUrl { get; set; }
        public string android_execution_params { get; set; }
        public string ios_execution_params { get; set; }
    }

    public class TemplateSocial
    {
        public int likeCount { get; set; }
        public int commentCount { get; set; }
        public int sharedCount { get; set; }
    }

    public class TemplateButton
    {
        public string title { get; set; }
        public TamplateContentLink link { get; set; }
    }
}