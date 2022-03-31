using ExCSS;
using System.Text;

namespace dxThemeConverter;

public static class DxThemeConverter
{
  public static string GetColorVariablesText(string cssContent)
  {
    var colorClassesDictionary = new Dictionary<string, string>()
    {
      //Селектор, название переменной
      { ".dx-theme-accent-as-text-color", "dx-theme-accent-as-text-color" },
      { ".dx-theme-text-color", "dx-theme-text-color" },
      { ".dx-theme-background-color-as-text-color", "dx-theme-background-color-as-text-color" },
      { ".dx-theme-border-color-as-text-color", "dx-theme-border-color-as-text-color" },
      { ".dx-theme-accent-as-background-color", "dx-theme-accent-as-background-color" },
      { ".dx-theme-text-color-as-background-color", "dx-theme-text-color-as-background-color" },
      { ".dx-theme-background-color", "dx-theme-background-color" },
      { ".dx-theme-border-color-as-background-color", "dx-theme-border-color-as-background-color" },
      { ".dx-theme-accent-as-border-color", "dx-theme-accent-as-border-color" },
      { ".dx-theme-text-color-as-border-color", "dx-theme-text-color-as-border-color" },
      { ".dx-theme-background-color-as-border-color", "dx-theme-background-color-as-border-color" },
      { ".dx-theme-border-color", "dx-theme-border-color" },
      { ".dx-list-group-header", "dx-list-group-header" },
      { ".dx-button-mode-outlined.dx-button-success", "dx-button-success" },
      { ".dx-button-mode-outlined.dx-button-default", "dx-button-default" },
      { ".dx-button-mode-outlined.dx-button-danger", "dx-button-danger" },

    };
    var parser = new StylesheetParser();
    var stylesheet = parser.Parse(cssContent);
    var themeColorRules = stylesheet.StyleRules.Where(sr => colorClassesDictionary.ContainsKey(sr.SelectorText));
    var result = new StringBuilder(1000);
    result.AppendLine(":root {");
    foreach (var themeColorRule in themeColorRules)
    {
      var cssVarName = colorClassesDictionary[themeColorRule.SelectorText];
      var cssVarName2 = colorClassesDictionary[themeColorRule.SelectorText] + "-o"; //Это для использования с прозрачностью
      //Беру значение цвета
      var varValue = themeColorRule.Style.Color;
      //Если значение цвета пустое, значит это класс для фонового цвета
      // и надо брать свойство не color, а background-color(или fill) они всегда в теме рядом
      if (string.IsNullOrEmpty(varValue)) varValue = themeColorRule.Style.BackgroundColor;
      if (string.IsNullOrEmpty(varValue)) varValue = themeColorRule.Style.BorderColor;

      var varValue2 = varValue.Replace("rgb(", "").Replace(")", ""); //Эта переменная, чтобы использовать прозрачность
      result.Append("  --").Append(cssVarName).Append(": ").Append(varValue).AppendLine(";");
      result.Append("  --").Append(cssVarName2).Append(": ").Append(varValue2).AppendLine(";");
    }
    result.AppendLine("}");
    return result.ToString();
  }
}
//https://github.com/TylerBrinks/ExCSS
