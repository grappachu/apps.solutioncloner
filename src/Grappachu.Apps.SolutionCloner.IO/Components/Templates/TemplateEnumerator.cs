using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Apps.SolutionCloner.Engine.Model.Templates;
using Grappachu.Core.Preview.IO;
using Newtonsoft.Json;

namespace Grappachu.Apps.SolutionCloner.Engine.Components.Templates
{
    public class TemplateEnumerator : ITemplateEnumerator
    {
        private readonly string _templateRoot;

        public TemplateEnumerator(string templateRoot)
        {
            _templateRoot = templateRoot;
        }

        public IReadOnlyCollection<TemplateInfo> GetTemplates()
        {
            var searchPath = new PathSearchInfo(_templateRoot, "*.template", SearchOption.AllDirectories);
            IFileEnumerator fe = new FileEnumerator(new List<PathSearchInfo> { searchPath });
            var templatefiles = fe.ToArray();
            var res = new List<TemplateInfo>();
            foreach (var templatefile in templatefiles)
            {
                try
                {
                    var templateData = File.ReadAllText(templatefile.FullName);
                    var template = JsonConvert.DeserializeObject<TemplateInfo>(templateData);
                    template.TemplatePath = templatefile.FullName;
                    res.Add(template);
                }
                catch (JsonException jsex)
                {
                    Console.WriteLine("Invalid Template: " + templatefile + " - " + jsex.Message);
                }
            }

            return res;
        }
    }
}