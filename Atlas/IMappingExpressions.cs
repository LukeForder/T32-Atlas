using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
namespace Atlas
{
    public interface IMappingExpressions
    {
        Action<XElement, T> AttributeMap<T, P>(
            Expression<Func<T, P>> propertySelector, 
            Expression<Func<string, P>> convert, 
            string attributeName);

        Action<XElement, T> ElementMap<T, P>(
            Expression<Func<T, P>> propertySelector, 
            Expression<Func<string, P>> convert, 
            string elementName);

        Action<XElement, T> ElementMap<T, P>(
            Expression<Func<T, P>> propertySelector,
            Expression<Func<string, T, P>> convert,
            string elementName);

        Action<XElement, T> ElementMap<T, P>(
            Expression<Func<T, P>> propertySelector,
            Expression<Func<IEnumerable<XElement>, P>> convert,
            string elementName);

        Action<XElement, T> ElementMap<T, P>(
            Expression<Func<T, P>> propertySelector,
            Expression<Func<IEnumerable<XElement>, T, P>> convert,
            string elementName);

        Action<XElement, T> ElementMap<T, P>(
            Expression<Func<T, P>> propertySelector,
            Expression<Func<XElement, P>> convert,
            string elementName);

        Action<XElement, T> ElementMap<T, P>(
            Expression<Func<T, P>> propertySelector,
            Expression<Func<XElement, T, P>> convert,
            string elementName);

        MemberExpression GetPropertyExpression<T, P>(
            Expression<Func<T, P>> propertySelector);
        
    }
}

