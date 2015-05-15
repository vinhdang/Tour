using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class CheckBoxListHelper
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, List<string> values, object htmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, values, values, htmlAttributes);
        }
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, List<string> values, List<string> labels, object htmlAttributes)
        {
            if (values == null)
            {
                return MvcHtmlString.Empty;
            }
            if (labels == null)
            {
                labels = new List<string>();
            }
            RouteValueDictionary attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            attributes.Remove("checked");
            StringBuilder sb = new StringBuilder();
            string[] modelValues = new string[] { };
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                modelValues = ((string[])modelState.Value.RawValue);
            }
            IEnumerator<string> labelEnumerator = labels.GetEnumerator();
            foreach (string s in values)
            {
                bool isChecked = modelValues.Contains(s);
                sb.Append(CrearCheckBox(name, s, isChecked, attributes));
                labelEnumerator.MoveNext();
                if (labelEnumerator.Current != null)
                {
                    sb.AppendLine(labelEnumerator.Current);
                }
            }
            TagBuilder divTag = new TagBuilder("div");
            divTag.InnerHtml = sb.ToString();
            if (modelState != null && modelState.Errors.Count > 0)
            {
                divTag.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }
            return MvcHtmlString.Create(divTag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, List<SelectListItem> list, object htmlAttributes)
        {
            if (list == null)
            {
                return MvcHtmlString.Empty;
            }
            RouteValueDictionary attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            attributes.Remove("checked");
            StringBuilder sb = new StringBuilder();
            foreach (SelectListItem s in list)
            {
                sb.Append(CrearCheckBox(name, s.Value, s.Selected, attributes));
                sb.AppendLine("<label>" + s.Text + "</label>");
            }
            TagBuilder divTag = new TagBuilder("div");
            divTag.MergeAttribute("class", "checkboxlist");
            divTag.InnerHtml = sb.ToString();
            return MvcHtmlString.Create(divTag.ToString(TagRenderMode.Normal));

        }
        private static string CrearCheckBox(string name, string value, bool isChecked, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.GenerateId(name);
            if (isChecked)
            {
                tagBuilder.MergeAttribute("checked", "checked");
            }
            if (value != null)
            {
                tagBuilder.MergeAttribute("value", value, true);
            }
            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }
    }
}
