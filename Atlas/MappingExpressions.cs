using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Atlas
{
    public class MappingExpressions : IMappingExpressions
    {
        public Expression<Func<XElement, string>> GetValue(string elementName)
        {
            return (element) => element.Element(elementName).Value;
        }

        public Expression<Func<XElement, XElement>> GetElement(string elementName)
        {
            return (element) => element.Element(elementName);
        }

        public Expression<Func<XElement, IEnumerable<XElement>>> GetElements(string elementName)
        {
            return (element) => element.Elements(elementName).ToList();
        }

        public Action<XElement, T> AttributeMap<T, P>(Expression<Func<T, P>> propertySelector, Expression<Func<string, P>> conversionFunction, string attributeName)
        {
            var xmlParameter = Expression.Parameter(typeof(XElement), "xml");

            var instanceExpression = Expression.Parameter(typeof(T), "instance");

            Expression<Func<XElement, string>> getValue = (xml) => xml.Attribute(attributeName).Value;

            var property = GetPropertyExpression<T, P>(propertySelector);

            var conversionExpression = Expression.Invoke(conversionFunction, Expression.Invoke(getValue, xmlParameter));

            var assignment = Expression.Assign(Expression.MakeMemberAccess(instanceExpression, property.Member), conversionExpression);

            var assignmentLambda = Expression.Lambda<Action<XElement, T>>(assignment, xmlParameter, instanceExpression);

            var func = assignmentLambda.Compile();

            return func;
        }

        public Action<XElement, T> ElementMap<T, P>(Expression<Func<T, P>> propertySelector, Expression<Func<IEnumerable<XElement>, P>> conversionExpression, string elementName)
        {
            var property = GetPropertyExpression<T, P>(propertySelector);

            var extractionExtraction = GetElements(elementName);

            return ElementMap<T, P>(property, extractionExtraction, conversionExpression, elementName);
        }

        public Action<XElement, T> ElementMap<T, P>(Expression<Func<T, P>> propertySelector, Expression<Func<XElement, P>> conversionExpression, string elementName)
        {
            var property = GetPropertyExpression<T, P>(propertySelector);

            var extractionExtraction = GetElement(elementName);

            return ElementMap<T, P>(property, extractionExtraction, conversionExpression, elementName);
        }

        public Action<XElement, T> ElementMap<T, P>(Expression<Func<T, P>> propertySelector, Expression<Func<string, P>> conversionExpression, string elementName)
        {
            var property = GetPropertyExpression<T, P>(propertySelector);

            var extractionExtraction = GetValue(elementName);

            return ElementMap<T, P>(property, extractionExtraction, conversionExpression, elementName);
        }

        public Action<XElement, T> ElementMap<T, P>(Expression<Func<T, P>> propertySelector, Expression<Func<string, T, P>> conversionExpression, string elementName)
        {
            var property = GetPropertyExpression(propertySelector);

            var extractionExtraction = GetValue(elementName);

            return ElementMapWithInstance<T, P>(property, extractionExtraction, conversionExpression, elementName);
        }

        public Action<XElement, T> ElementMap<T, P>(Expression<Func<T, P>> propertySelector, Expression<Func<IEnumerable<XElement>, T, P>> conversionExpression, string elementName)
        {
            var property = GetPropertyExpression(propertySelector);

            var extractionExtraction = GetElements(elementName);

            return ElementMapWithInstance<T, P>(property, extractionExtraction, conversionExpression, elementName);
        }

        public Action<XElement, T> ElementMap<T, P>(Expression<Func<T, P>> propertySelector, Expression<Func<XElement, T, P>> conversionExpression, string elementName)
        {
            var property = GetPropertyExpression(propertySelector);

            var extractionExtraction = GetElement(elementName);

            return ElementMapWithInstance<T, P>(property, extractionExtraction, conversionExpression, elementName);
        }

        protected Action<XElement, T> ElementMap<T, P>(MemberExpression propertySelector, Expression extractionExpression, Expression conversionExpression, string elementName)
        {
            var xmlParameter = Expression.Parameter(typeof(XElement), "xml");

            var instanceExpression = Expression.Parameter(typeof(T), "instance");

            var extractXmlValueExpression = Expression.Invoke(extractionExpression, xmlParameter);

            var processXmlValueExpression = Expression.Invoke(conversionExpression, extractXmlValueExpression);

            var assignment = Expression.Assign(Expression.MakeMemberAccess(instanceExpression, propertySelector.Member), processXmlValueExpression);

            var assignmentLambda = Expression.Lambda<Action<XElement, T>>(assignment, xmlParameter, instanceExpression);

            var func = assignmentLambda.Compile();

            return func;
        }

        protected Action<XElement, T> ElementMapWithInstance<T, P>(MemberExpression propertySelector, Expression extractionExpression, Expression conversionExpression, string elementName)
        {
            var xmlParameter = Expression.Parameter(typeof(XElement), "xml");

            var instanceExpression = Expression.Parameter(typeof(T), "instance");

            var extractXmlValueExpression = Expression.Invoke(extractionExpression, xmlParameter);

            var processXmlValueExpression = Expression.Invoke(conversionExpression, extractXmlValueExpression, instanceExpression);

            var assignment = Expression.Assign(Expression.MakeMemberAccess(instanceExpression, propertySelector.Member), processXmlValueExpression);

            var assignmentLambda = Expression.Lambda<Action<XElement, T>>(assignment, xmlParameter, instanceExpression);

            var func = assignmentLambda.Compile();

            return func;
        }

        public MemberExpression GetPropertyExpression<T, P>(Expression<Func<T, P>> propertySelector)
        {
            MemberExpression property = null;

            property = propertySelector.Body as System.Linq.Expressions.MemberExpression;

            if (property == null)
                throw new ArgumentException("The expression does not select a property");

            return property;
        }
    }
}

