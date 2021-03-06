﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ironrubyinconsoleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Warm up, calling Ruby code through IronRuby...");
            FirstAttempt(args);
            Console.WriteLine("==");
            Console.WriteLine("Calling csscss from IronRuby...");
            RunCssFromNet();
            Console.WriteLine("==");
            Console.WriteLine("Calling csscss from command line...");
            RunCssFromCmdLine();
            Console.WriteLine("==");
            Console.WriteLine("Calling csscss from command line (with gems downloaded with Bundler)...");
            RunCssFromNetBundlerPowered();
            Console.WriteLine("==");
            Console.Write("Press Enter to exit...");
            Console.ReadLine();
        }

        static void RunCssFromCmdLine()
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.RedirectStandardOutput=true;
            //csscss under windows is a batch file. In order for it to correctly
            //find the Ruby files we should call a batch that calls the csscss batch
            startInfo.FileName="call_csscss.bat";
            startInfo.Arguments="  -n 1 -v sample.css";
            startInfo.UseShellExecute = false;

            var process=System.Diagnostics.Process.Start(startInfo);

            System.Console.WriteLine(process.StandardOutput.ReadToEnd());
        }


        static void RunCssFromNet()
        {
            var cmd = IronRuby.Ruby.CreateEngine();
            //Added search path as suggested by this question:
            //http://stackoverflow.com/questions/9333732/how-to-embed-a-ruby-gem-into-a-c-sharp-project-and-require-it-from-an-embedded-i
            //For strscan and stringio I've inserted ruby files like the ones found in 
            //https://github.com/xpaulbettsx/SassAndCoffee/tree/master/SassAndCoffee.Ruby/Sass/lib/ironruby
            //For date nothing is required
            //http://stackoverflow.com/questions/1478011/embedding-ironruby-in-c-sharp-and-datetime
            var searchPaths = cmd.GetSearchPaths().Concat(new[] 
{
    "C:\\Users\\ermiotto\\ruby-1.9.3-p448\\lib",
    "lib\\json",
    "lib\\parslet",
    "lib"
});
            cmd.SetSearchPaths(searchPaths.ToList());
            //The class of interest is under a module/namespace, to access it under
            //the scripting engine I made a helper class as suggested in
            //http://stackoverflow.com/questions/651381/accessing-the-return-value-from-an-ironruby-dsl
            cmd.ExecuteFile("lib/csscsshelper.rb");

            //Thanks to http://stackoverflow.com/questions/1684145/call-ruby-or-python-api-in-c-sharp-net
            //for this solution
            dynamic globals = cmd.Runtime.Globals;
            dynamic methodMissingDemo = globals.Helper.@new();

            using (var fs = new System.IO.StreamReader("sample.css"))
            {
                Console.WriteLine(methodMissingDemo.run(fs.ReadToEnd()));
            }
        }

        static void FirstAttempt(string[] args)
        {

            var cmd = IronRuby.Ruby.CreateEngine();
   
            cmd.Execute("puts 'Hello World'");
            cmd.ExecuteFile("hello.rb");
            //Thanks to http://stackoverflow.com/questions/1684145/call-ruby-or-python-api-in-c-sharp-net
            //for this solution
            dynamic globals = cmd.Runtime.Globals;

            dynamic methodMissingDemo = globals.MethodMissingDemo.@new();

            Console.WriteLine(methodMissingDemo.HelloDynamicWorld());

            methodMissingDemo.print_all(args);
        }

        static void RunCssFromNetBundlerPowered()
        {
            var cmd = IronRuby.Ruby.CreateEngine();
            //Added search path as suggested by this question:
            //http://stackoverflow.com/questions/9333732/how-to-embed-a-ruby-gem-into-a-c-sharp-project-and-require-it-from-an-embedded-i
            //For strscan and stringio I've inserted ruby files like the ones found in 
            //https://github.com/xpaulbettsx/SassAndCoffee/tree/master/SassAndCoffee.Ruby/Sass/lib/ironruby
            //For date nothing is required
            //http://stackoverflow.com/questions/1478011/embedding-ironruby-in-c-sharp-and-datetime
            //To find all gems install with bundler, I've drawn inspiration from this explanation
            //of the implementation of require by rubygems:
            //http://stackoverflow.com/questions/6181684/how-does-require-rubygems-help-find-rubygem-files
            var searchPaths = cmd.GetSearchPaths().Concat(new[] 
{
    "C:\\Users\\ermiotto\\ruby-1.9.3-p448\\lib",
    "minlib"
    
}).Concat(_GetLibPath("local"));
            cmd.SetSearchPaths(searchPaths.ToList());
            //The class of interest is under a module/namespace, to access it under
            //the scripting engine I made a helper class as suggested in
            //http://stackoverflow.com/questions/651381/accessing-the-return-value-from-an-ironruby-dsl
            cmd.ExecuteFile("lib/csscsshelper.rb");

            //Thanks to http://stackoverflow.com/questions/1684145/call-ruby-or-python-api-in-c-sharp-net
            //for this solution
            dynamic globals = cmd.Runtime.Globals;
            dynamic methodMissingDemo = globals.Helper.@new();

            using (var fs = new System.IO.StreamReader("sample.css"))
            {
                Console.WriteLine(methodMissingDemo.run(fs.ReadToEnd()));
            }
        }

        private static IEnumerable<string> _GetLibPath(string basePath)
        {
            var paths = new List<string>();

            foreach(var gemFolder in _GetGemSubFolders(basePath))
            {
                paths.Add(System.IO.Path.Combine(gemFolder, "lib"));
            }

            return paths;
        }

        private static IEnumerable<string> _GetGemSubFolders(string basePath)
        {
            var path = _GetGemFolder(basePath);

            return System.IO.Directory.EnumerateDirectories(path);
        }

        private static string _GetGemFolder(string basePath)
        {

            foreach(var folder in System.IO.Directory.EnumerateDirectories(basePath))
            {
                if (System.IO.Path.GetFileName(folder) == "gems")
                {
                    return folder;
                }
                else
                {
                    string path = _GetGemFolder(folder);
                    if(!String.IsNullOrWhiteSpace(path))
                    {
                        return path;
                    }

                }
            }

            return null;
        }

    }
}
