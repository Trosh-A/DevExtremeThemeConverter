using ExCSS;
using System.Text;

namespace dxThemeConverter;

public static class DxThemeConverter
{
  public static string GetColorVariablesText(string cssContent)
  {
    var colorClasses = new string[]
    {
      ".dx-theme-accent-as-text-color",
      ".dx-theme-text-color",
      ".dx-theme-background-color-as-text-color",
      ".dx-theme-border-color-as-text-color",
      ".dx-theme-accent-as-background-color",
      ".dx-theme-text-color-as-background-color",
      ".dx-theme-background-color",
      ".dx-theme-border-color-as-background-color",
      ".dx-theme-accent-as-border-color",
      ".dx-theme-text-color-as-border-color",
      ".dx-theme-background-color-as-border-color",
      ".dx-theme-border-color",
    };
    var parser = new StylesheetParser();
    var stylesheet = parser.Parse(cssContent);
    var themeColorRules = stylesheet.StyleRules.Where(sr => colorClasses.Contains(sr.SelectorText));
    var result = new StringBuilder(750); //Я посмотерел, какой длины получается текст(713 символов)
    result.AppendLine(":root {");
    foreach (var themeColorRule in themeColorRules)
    {
      //.dx-theme-accent-as-text-color -> accent-as-text-color
      var cssVarName = themeColorRule.SelectorText.Trim('.');
      //Беру значение цвета
      var varValue = themeColorRule.Style.Color;
      //Если значение цвета пустое, значит это класс для фонового цвета
      // и надо брать свойство не color, а background-color(или fill) они всегда в теме рядом
      if (string.IsNullOrEmpty(varValue)) varValue = themeColorRule.Style.BackgroundColor;
      if (string.IsNullOrEmpty(varValue)) varValue = themeColorRule.Style.BorderColor;
      //Хотел в одну строку, но студия посоветовала так
      result.Append("  --").Append(cssVarName).Append(": ").Append(varValue).AppendLine(";");
    }
    result.AppendLine("}");
    return result.ToString();
  }
}
//https://github.com/TylerBrinks/ExCSS