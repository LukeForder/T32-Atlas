using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Xml.Linq;

namespace Atlas.Test
{
    class NestedMappedClass
    {
        public int IntProperty { get; set; }
    }

    class MappedClass
    {
        public string[] StringProperty  { get; set; }
        public int IntProperty          { get; set; }
        public Random RNG               { get; set; }
        public NestedMappedClass Nested { get; set; }
    }

    public class MappingsTest
    {
        public const string xml = "<mapped_class_xml an_attribute='1,2,3,4'><some_element>this is a string</some_element><some_element>another some element</some_element><another_element>123</another_element><random_map>142</random_map><nested_class><property>124</property></nested_class></mapped_class_xml>";

        [Fact]
        public void CanMapPropertyToElement()
        {
            MappingsStore store = new MappingsStore(new MappingExpressions());

            store.AddElementMap((MappedClass c) => c.StringProperty,  (IEnumerable<XElement> s) => s.Select(x => x.Value).ToArray(), "some_element");
            store.AddElementMap((MappedClass c) => c.RNG, s => new Random(int.Parse(s)), "random_map");
            store.AddElementMap((MappedClass c) => c.IntProperty, s => int.Parse(s), "another_element");

            MappedClass instance = new MappedClass();

            store.MapAndReturn(
                XElement.Parse(xml),
                instance);

            instance.IntProperty.Should().Be(123);
            instance.StringProperty.Should().BeEquivalentTo(new string[] { "this is a string", "another some element" });
            instance.RNG.Next().Should().Be(new Random(142).Next());
        }

        [Fact]
        public void CanMapPropertyToAttribute()
        {
            MappingsStore store = new MappingsStore(new MappingExpressions());

            store.AddAttributeMap((MappedClass c) => c.StringProperty, (string s) => s.Split(',').ToArray(), "an_attribute");

            MappedClass instance = new MappedClass();

            store.MapAndReturn(
                XElement.Parse(xml),
                instance);

            instance.StringProperty.Should().BeEquivalentTo(new string[] { "1", "2", "3", "4" });
        }

        [Fact]
        public void CanMapPropertyToMapping()
        {
            MappingsStore store = new MappingsStore(new MappingExpressions());

            store.AddElementMap(
                (NestedMappedClass c) => c.IntProperty,
                (string s) => int.Parse(s),
                "property");

            store.AddElementMap(
                (MappedClass c) => c.Nested,
                (XElement s, MappedClass c) => store.MapAndReturn(s, c.Nested),
                "nested_class");

            MappedClass instance = new MappedClass { Nested = new NestedMappedClass() };

            store.MapAndReturn(
                XElement.Parse(xml),
                instance);



            instance.Nested.IntProperty.Should().Be(124);
        }


    }
}
