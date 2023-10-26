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
        var modelType = helper.ViewData.ModelExplorer.ModelType;
        var model = helper.ViewData.Model;
        var modelProperties = modelType.GetProperties();
        foreach (var modelProperty in modelProperties)
        {
            divTagBuilder.InnerHtml.AppendHtml(GetLabel(modelProperty));
            divTagBuilder.InnerHtml.AppendHtml(GetInput(modelProperty));
            divTagBuilder.InnerHtml.AppendHtml("<br>");
        }
        divTagBuilder.InnerHtml.AppendHtml(GetValidator(modelProperties, model));
        return divTagBuilder.InnerHtml;
    }

    private static IHtmlContent? GetValidator(PropertyInfo[] modelProperties, object? model)
    {
        if (model == null)
        {
            return null;
        }
        foreach (var modelProperty in modelProperties)
        {
            var attributes = modelProperty.GetCustomAttributes<ValidationAttribute>();
            foreach (var attribute in attributes)
            {
                if (!attribute.IsValid(modelProperty.GetValue(model)))
                {
                    return new HtmlContentBuilder().Append(attribute.ErrorMessage);
                }
            }
        }
        return null;
    }

    private static IHtmlContent GetInput(PropertyInfo modelProperty)
    {
        if (modelProperty.PropertyType.Equals(typeof(string)))
        {
            var inputTagBuilder = new TagBuilder("input");
            inputTagBuilder.Attributes.Add("type", "text");
            inputTagBuilder.Attributes.Add("asp-for", modelProperty.Name);
            return inputTagBuilder;
        }
        if (modelProperty.PropertyType.Equals(typeof(int)))
        {
            var inputTagBuilder = new TagBuilder("input");
            inputTagBuilder.Attributes.Add("type", "number");
            inputTagBuilder.Attributes.Add("asp-for", modelProperty.Name);
            return inputTagBuilder;
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
        selectTagBuilder.Attributes.Add("asp-for", modelProperty.Name);
        foreach (var enumName in modelProperty.PropertyType.GetEnumNames())
        {
            var optionTagBuilder = new TagBuilder("option");
            optionTagBuilder.Attributes.Add("type", enumName);
            optionTagBuilder.InnerHtml.Append(enumName);
            selectTagBuilder.InnerHtml.AppendHtml(optionTagBuilder);
        }
        return selectTagBuilder;
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
                return label;
            }
        }
        label.InnerHtml.Append(SplitCamelCase(modelProperty.Name));
        return label;
    }

    private static string SplitCamelCase(string name)
    {
        string[] words = Regex.Split(name, @"(?=\p{Lu})");
        return string.Join(" ", words);
    }
}