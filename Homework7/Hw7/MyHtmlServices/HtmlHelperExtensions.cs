using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            divTagBuilder.InnerHtml.AppendHtml("<div>");
            divTagBuilder.InnerHtml.AppendHtml(GetLabel(modelProperty, model));
            divTagBuilder.InnerHtml.AppendHtml(GetInput(modelProperty));
            divTagBuilder.InnerHtml.AppendHtml(GetValidator(modelProperty, model));
            divTagBuilder.InnerHtml.AppendHtml("</div>");
            divTagBuilder.InnerHtml.AppendHtml("<br>");
        }
        return divTagBuilder.InnerHtml;
    }

    private static IHtmlContent? GetValidator(PropertyInfo modelProperty, object? model)
    {
        if (model == null)
        {
            return null;
        }

        var attributes = modelProperty.GetCustomAttributes<ValidationAttribute>();
        foreach (var attribute in attributes)
        {
            if (!attribute.IsValid(modelProperty.GetValue(model)))
            {
                var spanTagBuilder = new TagBuilder("span");
                spanTagBuilder.InnerHtml.AppendHtml(attribute.ErrorMessage);
                return spanTagBuilder;
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
            inputTagBuilder.Attributes.Add("name", modelProperty.Name);
            inputTagBuilder.Attributes.Add("id", modelProperty.Name);
            return inputTagBuilder;
        }
        if (modelProperty.PropertyType.Equals(typeof(int)))
        {
            var inputTagBuilder = new TagBuilder("input");
            inputTagBuilder.Attributes.Add("type", "number");
            inputTagBuilder.Attributes.Add("name", modelProperty.Name);
            inputTagBuilder.Attributes.Add("id", modelProperty.Name);
            return inputTagBuilder;
        }
        if (modelProperty.PropertyType.IsEnum)
        {
            return GetEnumInput(modelProperty);
        }
        var spanTagBuilder = new TagBuilder("label");
        spanTagBuilder.Attributes.Add("id", "unsupported: "+modelProperty.Name);
        return spanTagBuilder;
    }

    private static IHtmlContent GetEnumInput(PropertyInfo modelProperty)
    {
        var selectTagBuilder = new TagBuilder("select");
        selectTagBuilder.Attributes.Add("name", modelProperty.Name);
        selectTagBuilder.Attributes.Add("id", modelProperty.Name);
        foreach (var enumName in modelProperty.PropertyType.GetEnumNames())
        {
            var optionTagBuilder = new TagBuilder("option");
            optionTagBuilder.Attributes.Add("type", enumName);
            optionTagBuilder.InnerHtml.Append(enumName);
            selectTagBuilder.InnerHtml.AppendHtml(optionTagBuilder);
        }
        return selectTagBuilder;
    }

    private static IHtmlContent GetLabel(PropertyInfo modelProperty, object? model)
    {
        var label = new TagBuilder("label");
        label.Attributes.Add("for", modelProperty.Name);
        var propertyAttributes = modelProperty.GetCustomAttributes();

        foreach (var attribute in propertyAttributes)
        {
            if (attribute is DisplayAttribute displayAttribute)
            {
                label.InnerHtml.AppendHtml(displayAttribute.Name);
                return label;
            }
        }
        label.InnerHtml.Append(SplitCamelCase(modelProperty.Name));
        return label;
    }

    private static string SplitCamelCase(string name)
    {
        string[] words = Regex.Split(name, @"(?=\p{Lu})");
        var newName = words.Skip(1).ToArray();
        return string.Join(" ", newName);
    }
}