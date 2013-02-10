using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Atlas
{
    public class MappingsStore
    {
        public MappingsStore(IMappingExpressions expressionsGenerator)
        {
            if (expressionsGenerator == null)
                throw new ArgumentNullException("expressionsGenerator");

            _expressionsGenerator = expressionsGenerator;
        }

        protected readonly ConcurrentDictionary<Type, ConcurrentBag<Tuple<string, object>>> mappings =
            new ConcurrentDictionary<Type, ConcurrentBag<Tuple<string, object>>>();
        
        protected readonly IMappingExpressions _expressionsGenerator;

        public virtual void AddElementMap<T, P>(
            Expression<Func<T, P>> propertySelector, 
            Expression<Func<string, P>> conversionExpression, 
            string elementName)
        {
            var typeMappings = mappings.GetOrAdd(typeof(T), new ConcurrentBag<Tuple<string, object>>());

            var propertyExpression = _expressionsGenerator.GetPropertyExpression(propertySelector);

            if (typeMappings.Any(t => t.Item1 == propertyExpression.Member.Name))
                throw new InvalidOperationException("The mapping of multiple elements to a single property is not supported.");

            typeMappings.Add(new Tuple<string, object>(propertyExpression.Member.Name, _expressionsGenerator.ElementMap(propertySelector, conversionExpression, elementName)));
        }

        public virtual void AddElementMap<T, P>(
            Expression<Func<T, P>> propertySelector,
            Expression<Func<IEnumerable<XElement>, P>> conversionExpression,
            string elementName)
        {
            var typeMappings = mappings.GetOrAdd(typeof(T), new ConcurrentBag<Tuple<string, object>>());

            var propertyExpression = _expressionsGenerator.GetPropertyExpression(propertySelector);

            if (typeMappings.Any(t => t.Item1 == propertyExpression.Member.Name))
                throw new InvalidOperationException("The mapping of multiple elements to a single property is not supported.");

            typeMappings.Add(new Tuple<string, object>(propertyExpression.Member.Name, _expressionsGenerator.ElementMap(propertySelector, conversionExpression, elementName)));
        }

        public virtual void AddElementMap<T, P>(
            Expression<Func<T, P>> propertySelector,
            Expression<Func<IEnumerable<XElement>, T, P>> conversionExpression,
            string elementName)
        {
            var typeMappings = mappings.GetOrAdd(typeof(T), new ConcurrentBag<Tuple<string, object>>());

            var propertyExpression = _expressionsGenerator.GetPropertyExpression(propertySelector);

            if (typeMappings.Any(t => t.Item1 == propertyExpression.Member.Name))
                throw new InvalidOperationException("The mapping of multiple elements to a single property is not supported.");

            typeMappings.Add(new Tuple<string, object>(propertyExpression.Member.Name, _expressionsGenerator.ElementMap(propertySelector, conversionExpression, elementName)));
        }

        public virtual void AddElementMap<T, P>(
           Expression<Func<T, P>> propertySelector,
           Expression<Func<string, T, P>> conversionExpression,
           string elementName)
        {
            var typeMappings = mappings.GetOrAdd(typeof(T), new ConcurrentBag<Tuple<string, object>>());

            var propertyExpression = _expressionsGenerator.GetPropertyExpression(propertySelector);

            if (typeMappings.Any(t => t.Item1 == propertyExpression.Member.Name))
                throw new InvalidOperationException("The mapping of multiple elements to a single property is not supported.");

            typeMappings.Add(new Tuple<string, object>(propertyExpression.Member.Name, _expressionsGenerator.ElementMap(propertySelector, conversionExpression, elementName)));
        }

        public virtual void AddElementMap<T, P>(
           Expression<Func<T, P>> propertySelector,
           Expression<Func<XElement, T, P>> conversionExpression,
           string elementName)
        {
            var typeMappings = mappings.GetOrAdd(typeof(T), new ConcurrentBag<Tuple<string, object>>());

            var propertyExpression = _expressionsGenerator.GetPropertyExpression(propertySelector);

            if (typeMappings.Any(t => t.Item1 == propertyExpression.Member.Name))
                throw new InvalidOperationException("The mapping of multiple elements to a single property is not supported.");

            typeMappings.Add(new Tuple<string, object>(propertyExpression.Member.Name, _expressionsGenerator.ElementMap(propertySelector, conversionExpression, elementName)));
        }


        public virtual void AddAttributeMap<T, P>(
            Expression<Func<T, P>> propertySelector, 
            Expression<Func<string, P>> convert, 
            string attributeName)
        {
            var typeMappings = mappings.GetOrAdd(typeof(T), new ConcurrentBag<Tuple<string, object>>());

            var propertyExpression = _expressionsGenerator.GetPropertyExpression(propertySelector);

            if (typeMappings.Any(t => t.Item1 == propertyExpression.Member.Name))
                throw new InvalidOperationException("The mapping of multiple attributes to a single property is not supported.");

            typeMappings.Add(new Tuple<string, object>(propertyExpression.Member.Name, _expressionsGenerator.AttributeMap(propertySelector, convert, attributeName)));
        }

        public virtual void Map<T>(XElement xml, T instance)
        {
            var typeMappings = mappings.GetOrAdd(typeof(T), new ConcurrentBag<Tuple<string, object>>());

            typeMappings
                .Select(t => t.Item2)
                .Cast<Action<XElement, T>>()
                .ToList()
                .ForEach(a => a.Invoke(xml, instance));
        }

        public virtual T MapAndReturn<T>(XElement xml, T instance)
        {
            Map(xml, instance);

            return instance;
        }


    }
}
