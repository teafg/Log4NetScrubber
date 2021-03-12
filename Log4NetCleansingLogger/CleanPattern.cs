using log4net.Layout;
using log4net.Util;

namespace Log4NetCleansingLogger
{
    class CleanPattern: PatternLayout
    {
        public CleanPattern()
        {
            AddConverter(new ConverterInfo
            {
                Name = "cleaned_message",
                Type = typeof(MessagePatternConvert)
            }
        );

        }
    }
}
