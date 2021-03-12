using log4net.Core;
using log4net.Layout.Pattern;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace Log4NetCleansingLogger
{
    public class MessagePatternConvert : PatternLayoutConverter
    {

        public MessagePatternConvert()
        {

        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var listOfRegex = new List<string>();

            // DI not working within PatternlayoutConverter inheritance from PatternLayout inheritance 
            // therefore the _config must be built here.

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var _config = builder.Build();

            var children = _config.GetSection("REGEX").GetChildren();

            foreach (var item in children)
            {
                listOfRegex.Add(item.Value);
            }

            var logMessage = loggingEvent.GetLoggingEventData();
            if (logMessage.Message != null)
            {
                //inject regex as an array.and their own masking term found in appsettings.json
                foreach (var regex in listOfRegex)
                {
                    logMessage.Message = CleanMessage(logMessage.Message, regex);
                }
                writer.Write(logMessage.Message);
            }
        }

        private string CleanMessage(string str, string regex)
        {
            var _regex = regex;
            var strArray = str.Split(" ");
            foreach (var item in strArray)
            {
                if (Regex.IsMatch(item, _regex))
                {
                    str = str.Replace(item, "***Masked***");
                }
            }
            return str;
        }
    }
}
