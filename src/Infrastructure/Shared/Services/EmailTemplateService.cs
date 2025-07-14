﻿using Application.Common.Mailing;
using RazorEngineCore;
using System.Text;

namespace Shared.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
        {
            string template = GetTemplate(templateName);

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

            return modifiedTemplate.Run(mailTemplateModel);
        }

        public static string GetTemplate(string templateName)
        {
            string baseDirectory = "";/*AppDomain.CurrentDomain.BaseDirectory;*/
            string tmplFolder = Path.Combine(baseDirectory, "EmailTemplates");
            string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //using var sr = new StreamReader(fs, Encoding.Default);
            using var sr = new StreamReader(fs, Encoding.UTF8);//coloco esta linea para que envié los caracteres con tilde correctamente

            string mailText = sr.ReadToEnd();
            sr.Close();

            return mailText;
        }
    }
}
