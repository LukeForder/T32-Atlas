using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Atlas
{
    public static class Mappings
    {
        private static MappingsStore _store = new MappingsStore(new MappingExpressions());

        public static void SetStore(MappingsStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            _store = store;
        }

        public static void AddElementMap<T, P>(
           Expression<Func<T, P>> propertySelector,
           Expression<Func<string, P>> conversionExpression,
           string elementName)
        {
            _store.AddElementMap(propertySelector, conversionExpression, elementName);
        }

        public static void AddAttributeMap<T, P>(
            Expression<Func<T, P>> propertySelector, 
            Expression<Func<string, P>> conversionExpression, 
            string elementName, 
            string attributeName)
        {
            _store.AddAttributeMap(propertySelector, conversionExpression, attributeName);
        }

        public static void Map<T>(XElement xml, T instance)
        {
            _store.Map(xml, instance);
        }
    }
}
