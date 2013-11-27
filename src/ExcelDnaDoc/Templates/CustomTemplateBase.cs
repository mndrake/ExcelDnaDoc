﻿namespace ExcelDnaDoc.Templates
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Reflection;
    using RazorEngine;
    using RazorEngine.Templating;

    public abstract class CustomTemplateBase<T> : TemplateBase<T>
    {
        public CustomTemplateBase()
        {
        }

        public new T Model { get; set; }

        public abstract string PageName { get; }

        public abstract byte[] Template { get; }

        public string ViewFilePath
        {
            get
            {
                return Path.Combine(HtmlHelp.HelpContentFolderPath, this.GetType().Name + ".cshtml");
            }
        }

        public void Publish(string folder)
        {
            string template;

            // destination file path
            string path = Path.Combine(folder, this.PageName);

            // look for razorengine template otherwise use embedded one
            if (File.Exists(this.ViewFilePath))
            {
                Console.WriteLine("using local template : " + Path.GetFileName(this.ViewFilePath));
                template = File.ReadAllText(this.ViewFilePath);
            }
            else
            {
                template = (new UTF8Encoding()).GetString(this.Template);
            }

            // unicode result
            string content = Razor.Parse(template, this.Model);

            // strip non ANSI characters from result
            content = Regex.Replace(content, @"[^\u0000-\u007F]", string.Empty);
            content = content.TrimStart(new char[] { '\r', '\n' });

            File.WriteAllText(path, content);
        }
    }
}