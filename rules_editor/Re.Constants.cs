using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Re
{
    public class Constant
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    // TODO: Load them from lexer?
    public class ConstantSet
    {
        public string Name { get; set; }
        public IEnumerable<Constant> Constants { get; set; }

        public static IEnumerable<ConstantSet> Load(string path)
        {
            try
            {
                var doc = XDocument.Load(path);
                var sets = new List<ConstantSet>();
                foreach (var set in doc.Root.Elements())
                {
                    var constants = new List<Constant>();
                    foreach (var c in set.Elements())
                    {
                        constants.Add(new Constant()
                        {
                            Name = c.Element("Name").Value,
                            Description = (string)c.Element("Description")
                        });
                    }
                    sets.Add(new ConstantSet()
                    {
                        Name = set.Attribute("Name").Value,
                        Constants = constants
                    });
                }
                return sets;
            }
            catch
            {
                return Enumerable.Empty<ConstantSet>();
            }
        }

    }

    public static class ConstantSets
    {
        public static ConstantSet CharacteristicTypes => Sets.FirstOrDefault(s => s.Name == "Characteristic Types");
        public static ConstantSet FeatureTypes => Sets.FirstOrDefault(s => s.Name == "Feature Types");
        public static ConstantSet ShapeClasses => Sets.FirstOrDefault(s => s.Name == "Shape Classes");
        public static ConstantSet DmeClassName => Sets.FirstOrDefault(s => s.Name == "DME Class Names");

        public static void Load(string path)
        {
            Sets = ConstantSet.Load(path);
        }

        private static IEnumerable<ConstantSet> Sets { get; set; }
    }
}
