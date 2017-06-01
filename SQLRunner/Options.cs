using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace SQLRunner
{
    class Options
    {
        [Option('s', "Server", Required = true,
        HelpText = "SQL Server to connect to.")]
        public string Server { get; set; }

        [Option('d', "Database", Required = true,
          HelpText = "Database to insert data into.")]
        public string Database { get; set; }

        [Option('f', "Folder", Required = true,
          HelpText = "Folder which contains static data script.")]
        public string Folder { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
