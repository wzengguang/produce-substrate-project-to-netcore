
namespace SubstrateUtils.ProduceAssembly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using SubstrateUtils.Extensions;

    public class ProjectReference
    {
        string path;
        string rootPath;
        HashSet<string> Props;

        public ProjectReference(string rootPath, string path)
        {
            this.path = path;
            this.rootPath = rootPath;
            Props = GetPackageProps();
        }

        public string Produce()
        {
            if (path == null)
            {
                return "path" + "not found.";
            }

            string physicPath = Path.Combine(rootPath, path);

            if (!File.Exists(physicPath))
            {
                return "physicPath" + "not found.";
            }

            XDocument doc = XDocument.Load(physicPath);
            XElement root = doc.Root;
            ToSDK(root);
            FindNet472ProjectReference(root);
            ConvertReferenceToPackageReference(root);
            OrderPackageReference(root);
            ClearEmptyItemGroup(root);
            RemoveNamespace(root);
            root.Save(physicPath);

            string[] strs = File.ReadAllLines(physicPath);
            if (strs[0].Contains("<?xml"))
            {
                File.WriteAllLines(physicPath, strs.Skip(1));
            }

            return File.ReadAllText(physicPath);
        }

        private static void RemoveNamespace(XElement element)
        {
            (from node in element.DescendantsAndSelf()
             where node is XElement
             from attr in node.Attributes()
             where attr.IsNamespaceDeclaration && attr.Name.LocalName == "xmlns"
             select attr).All(attr => { attr.Remove(); return true; });
        }

        public void ToSDK(XElement root)
        {
            if (root.CheckAttribute(XmlConst.Sdk) == null)
            {
                root.RemoveAttributes();
                root.SetAttributeValue(XmlConst.Sdk, XmlConst.SdkValue);
            }

            XElement import = root.GetFirstChildByAttribute("Import", "Project", true, "$(EnvironmentConfig)");
            if (import != null)
            {
                import.Remove();
            }

            XElement projectGuid = root.GetFirstDescendent("ProjectGuid");
            if (projectGuid != null)
            {
                projectGuid.Remove();
            }

            XElement projectTypeGuids = root.GetFirstDescendent("ProjectTypeGuids");
            if (projectTypeGuids != null)
            {
                projectTypeGuids.Remove();
            }

            XElement CSharp = root.GetFirstChildByAttribute("Import", "Project", true, @"$(ExtendedTargetsPath)\Microsoft.CSharp.targets");
            if (CSharp != null)
            {
                CSharp.Remove();
            }

            XElement packageReferenceItemgroup = ItemGroupOfTargetChild(root, XmlConst.PackageReference);
            XElement mSTest = root.GetFirstChildByAttribute("Import", "Project", true, @"$(BranchTargetsPath)\Test\MSTest.targets");
            if (mSTest != null)
            {
                mSTest.Remove();
                AddNewPackageReference(root, packageReferenceItemgroup, "MSTest.TestAdapter");
                AddNewPackageReference(root, packageReferenceItemgroup, "MSTest.TestFramework");
            }

            XElement nSubstitute = root.GetFirstChildByAttribute("Import", "Project", true, @"$(BranchTargetsPath)\Test\NSubstitute.targets");
            if (nSubstitute != null)
            {
                nSubstitute.Remove();
                AddNewPackageReference(root, packageReferenceItemgroup, "NSubstitute");
            }
        }

        public void ConvertReferenceToPackageReference(XElement root)
        {
            XElement parentEle = ItemGroupOfTargetChild(root, XmlConst.PackageReference);
            var references = root.GetDescendents(XmlConst.Reference).ToList();
            foreach (var reference in references)
            {
                string include = reference.GetAttribute(XmlConst.Include);
                if (string.IsNullOrEmpty(include) || !include.ContainIgnoreCase(XmlConst.PkgPrefix))
                {
                    include = reference.GetFirstChild(XmlConst.HintPath)?.Value;
                }

                if (!include.ContainIgnoreCase(XmlConst.PkgPrefix))
                {
                    continue;
                }

                string pattern = @"\$\((.*?)\)";
                Match match = Regex.Match(include, pattern);
                if (!match.Success)
                {
                    continue;
                }

                string pkg = match.Groups[1].Value;
                string pkgName = pkg.Trim().ReplaceIgnoreCase("Pkg", "").Replace("_", ".");
                string dllName = include.Split('\\').Last().ReplaceIgnoreCase(".dll", "");
                if (!pkgName.EqualsIgnoreCase(dllName))
                {
                    continue;
                }

                AddNewPackageReference(root, parentEle, pkgName);
                reference.Remove();
            }
        }

        public void AddNewPackageReference(XElement root, XElement parent, string pkgName)
        {
            if (root.GetFirstDescendentByAttribute(XmlConst.PackageReference, pkgName) != null || !Props.Contains(pkgName))
            {
                return;
            }

            var newpkg = new XElement(XmlConst.PackageReference);
            newpkg.SetAttributeValue(XmlConst.Include, pkgName);
            parent.Add(newpkg);
        }

        public void OrderPackageReference(XElement root)
        {
            var packagereferences = root.GetDescendents("PackageReference").OrderBy(a => a.GetAttribute("Include")).ToList();

            XElement igAll = null;
            List<XElement> forall = new List<XElement>();
            foreach (var packageref in packagereferences)
            {
                if (packageref.Parent.CheckAttribute("Condition") == null)
                {
                    forall.Add(packageref);
                    if (igAll == null)
                    {
                        igAll = packageref.Parent;
                    }
                    packageref.Remove();
                }
            }
            igAll.Add(forall);
        }

        public List<XElement> FindNet472ProjectReference(XElement xDocument)
        {
            List<XElement> notnet6 = new List<XElement>();
            var pReferences = xDocument.GetDescendents("ProjectReference").ToList();

            foreach (var pReference in pReferences)
            {
                string includePath = pReference.Attribute("Include")?.Value;
                if (includePath != null)
                {
                    string fullPath = Path.Combine(rootPath, includePath.Replace("$(INETROOT)\\", string.Empty, StringComparison.OrdinalIgnoreCase));

                    var root = XDocument.Load(fullPath).Root;
                    var el = root.GetFirstChild("PropertyGroup");
                    string pg = el.Value;
                    if (pg != null && !pg.Contains("net6"))
                    {
                        notnet6.Add(pReference);
                        pReference.Remove();
                    }

                    //if (root.CheckAttribute("Sdk") != null)
                    //{
                    //    pReference.RemoveNodes();
                    //}
                }
            }

            var ig = ItemGroupNet472OfTargetChild(xDocument, "ProjectReference");
            ig.Add(notnet6);
            return notnet6;
        }

        private XElement ItemGroupOfTargetChild(XElement root, string elementName)
        {
            var igs = root.GetDescendents("ItemGroup").ToArray();
            foreach (var item in igs)
            {
                if (!string.IsNullOrEmpty(item.GetAttribute("Condition")))
                {
                    continue;
                }

                if (!item.HasElements)
                {
                    return item;
                }

                var names = item.GetChildren(elementName).DistinctBy(a => a.Name.LocalName).Select(a => a.Name.LocalName).ToList();
                if (names.Count == 1 && names[0] == elementName)
                {
                    return item;
                }
            }

            XElement xElement = new XElement("ItemGroup");
            igs.First().AddBeforeSelf(xElement);
            return xElement;
        }

        private XElement ItemGroupNet472OfTargetChild(XElement root, string elementName)
        {
            var igs = root.GetDescendents("ItemGroup").ToArray();
            foreach (var item in igs)
            {
                if (item.CheckAttribute("Condition", true, "net472", "==") == null)
                {
                    continue;
                }

                if (!item.HasElements)
                {
                    return item;
                }

                var names = item.GetChildren(elementName).DistinctBy(a => a.Name.LocalName).Select(a => a.Name.LocalName).ToList();
                if (names.Count == 1 && names[0] == elementName)
                {
                    return item;
                }
            }

            XElement xElement = new XElement("ItemGroup");
            xElement.SetAttributeValue("Condition", "'$(TargetFramework)' == 'net472'");
            igs.First().AddAfterSelf(xElement);
            return xElement;
        }

        private XElement ItemGroupNet6OfTargetChild(XElement root, string elementName)
        {
            var igs = root.GetDescendents("ItemGroup").ToArray();
            foreach (var item in igs)
            {
                if (item.CheckAttribute("Condition", true, "net472", "!=") == null)
                {
                    continue;
                }

                if (!item.HasElements)
                {
                    return item;
                }

                var names = item.GetChildren(elementName).DistinctBy(a => a.Name.LocalName).Select(a => a.Name.LocalName).ToList();
                if (names.Count == 1 && names[0] == elementName)
                {
                    return item;
                }
            }

            XElement xElement = new XElement("ItemGroup");
            xElement.SetAttributeValue("Condition", "'$(TargetFramework)' != '' and '$(TargetFramework)' != 'net472'");
            igs.First().AddAfterSelf(xElement);
            return xElement;
        }

        private void ClearEmptyItemGroup(XElement root)
        {
            var igs = root.GetDescendents("ItemGroup").ToArray();
            foreach (var item in igs)
            {
                if (!item.HasElements)
                {
                    item.Remove();
                }
            }
        }

        private HashSet<string> GetPackageProps()
        {
            string path = Path.Combine(rootPath, "Packages.props");
            XElement root = XDocument.Load(path).Root;
            return root.GetDescendents(XmlConst.PackageVersion).Select(a => a.GetAttribute(XmlConst.Include)).ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public void FindNotNet6ProjectReference(XDocument xDocument, TreeNode treeNode, int deep, int maxDeep = 10)
        {
            deep++;

            var projectReferenceElements = xDocument.Root.GetDescendents("ProjectReference");

            foreach (var projectReferenceElement in projectReferenceElements)
            {
                string includePath = projectReferenceElement.Attribute("Include")?.Value;
                if (includePath != null)
                {
                    string fullPath = Path.Combine(rootPath, includePath.ReplaceIgnoreCase("$(INETROOT)\\", string.Empty));

                    var propertyGroupElement = XDocument.Load(fullPath).Root.GetFirstChild("PropertyGroup");
                    string propertyGroupElementValue = propertyGroupElement?.Value;
                    if (propertyGroupElementValue != null && !propertyGroupElementValue.ContainIgnoreCase("net6"))
                    {
                        TreeNode childTree = new TreeNode { Parent = treeNode, NodeValue = includePath };

                        treeNode.Children.Add(childTree);
                        if (deep <= maxDeep)
                        {
                            XDocument xd = XDocument.Load(fullPath);

                            FindNotNet6ProjectReference(xd, childTree, deep);
                        }
                    }
                }
            }
        }
    }
}
