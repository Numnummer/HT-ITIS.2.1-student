using Hw7.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MakeFormInputs(this IHtmlHelper helper)
    {
        var divTagBuilder = new TagBuilder("div");
        var modelType = helper.ViewData.Model.GetType();
        var modelProperties = modelType.GetProperties();
        foreach (var modelProperty in modelProperties)
        {
            divTagBuilder.InnerHtml.AppendHtml(GetLabel(modelProperty));
            divTagBuilder.InnerHtml.AppendHtml(GetInput(modelProperty));
        }

        return divTagBuilder.InnerHtml;
    }

    private static IHtmlContent GetInput(PropertyInfo modelProperty)
    {
        if (modelProperty.PropertyType.Equals(typeof(string)))
        {
            var inputTagBuilder = new TagBuilder("input");
            inputTagBuilder.Attributes.Add("type", "text");
            return inputTagBuilder.InnerHtml;
        }
        if (modelProperty.PropertyType.Equals(typeof(int)))
        {
            var inputTagBuilder = new TagBuilder("input");
            inputTagBuilder.Attributes.Add("type", "number");
            return inputTagBuilder.InnerHtml;
        }
        if (modelProperty.PropertyType.IsEnum)
        {
            return GetEnumInput(modelProperty);
        }
        return new HtmlContentBuilder().Append("Type not supported");
    }

    private static IHtmlContent GetEnumInput(PropertyInfo modelProperty)
    {
        var selectTagBuilder = new TagBuilder("select");
        foreach (var enumName in modelProperty.PropertyType.GetEnumNames())
        {
            var optionTagBuilder = new TagBuilder("option");
            optionTagBuilder.Attributes.Add("type", enumName);
            optionTagBuilder.InnerHtml.Append(enumName);
            selectTagBuilder.InnerHtml.AppendHtml(optionTagBuilder);
        }
        return selectTagBuilder.InnerHtml;
    }

    private static IHtmlContent GetLabel(PropertyInfo modelProperty)
    {
        var label = new TagBuilder("label");
        var propertyAttributes = modelProperty.GetCustomAttributes();
        foreach (var attribute in propertyAttributes)
        {
            if (attribute is DisplayAttribute displayAttribute)
            {
                label.InnerHtml.Append(displayAttribute.Name);
                return label.InnerHtml;
            }
        }
        label.InnerHtml.Append(SplitCamelCase(modelProperty.Name));
        return label.InnerHtml;
    }

    private static string SplitCamelCase(string name)
    {
        string[] words = Regex.Split(name, @"(?=\p{Lu})");
        return string.Join(" ", words);
    }
}