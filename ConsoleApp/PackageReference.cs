using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp
{
    internal class PackageReference
    {
        static string rootPath = "C:\\o365\\SB\\src";

        public static void Package()
        {
            Dictionary<string, XElement> map = new Dictionary<string, XElement>(StringComparer.OrdinalIgnoreCase);

            string file = "C:\\Users\\v-alightwang\\OneDrive - Microsoft\\Work\\WorkItem\\package.md";
            string[] paths = File.ReadAllLines(file);

            foreach (var path in paths)
            {
                string fpath = Path.Combine(rootPath, path);

                XDocument xDocument = XDocument.Load(fpath);

                var eles = xDocument.Descendants("PackageReference");
                foreach (XElement ele in eles)
                {
                    string value = ele.Attribute("Include")?.Value;
                    if (value != null)
                    {
                        map[value] = ele;
                    }
                }
            }

            string desc = "C:\\O365\\SB\\src\\sources\\dev\\PopImap\\nupkg\\AutodV2\\Microsoft.Exchange.AutodV2.csproj";

            XDocument descDoc = XDocument.Load(desc);

            var ig = descDoc.Descendants("ItemGroup").First();

            var pcs = ig.Elements();

            foreach (var item in map)
            {
                var find = pcs.FirstOrDefault(a => a.Attribute("Include").Value.Equals(item.Key, StringComparison.OrdinalIgnoreCase));

                if (find == null)
                {
                    ig.Add(item.Value);

                }

            }


            var ig1 = descDoc.Descendants("ItemGroup").ElementAt(1);
            var ig2 = descDoc.Descendants("ItemGroup").ElementAt(2);


            foreach (var item in paths)
            {
                string fpath = Path.Combine(rootPath, item);

                string assemblyName = XDocument.Load(fpath).Descendants("AssemblyName").First().Value + ".dll";

                var e2 = new XElement("QCustomInput");
                e2.SetAttributeValue("Visible", "false");
                string d1 = item.Split('/')[2];
                string d2 = Path.GetFileNameWithoutExtension(item);
                e2.SetAttributeValue("Include", $@"$(TargetPathDir)dev\{d1}\{d2}\$(FlavorPlatformDir)\{assemblyName}");
                ig2.Add(e2);


                var e1 = new XElement("None");
                e1.SetAttributeValue("Pack", "true");
                e1.SetAttributeValue("PackagePath", @"lib\net6.0");
                e1.SetAttributeValue("Include", $@"$(TargetPathDir)dev\{d1}\{d2}\bin\$(Configuration)\net6.0\{assemblyName}");
                ig1.Add(e1);

            }




            descDoc.Save(desc);




        }
    }
}
