using dxThemeConverter;
//Массив полных путей к css файлам
var cssFilesFullPathes = Directory.GetFiles("Themes", "*.css");
foreach (var cssFileFulPath in cssFilesFullPathes)
{
  string cssContent = File.ReadAllText(cssFileFulPath);
  string colorVariablesText = DxThemeConverter.GetColorVariablesText(cssContent);
  Console.WriteLine();
  Console.WriteLine(colorVariablesText);
  string oldFileName = Path.GetFileName(cssFileFulPath);
  string newFileName = $"withColors.{oldFileName}";
  string resultDirName = "Themes_With_Colors";
  string newFilePath = Path.Combine(resultDirName, newFileName);
  if (!Directory.Exists(resultDirName)) Directory.CreateDirectory(resultDirName);
  using var writer = new StreamWriter(newFilePath, new FileStreamOptions()
  {
    Mode = FileMode.Create,
    Access = FileAccess.Write,
  });
  //Первая строка @charset должна быть только первой, до неё вставлять нельзя
  var cssContentLines = File.ReadAllLines(cssFileFulPath);
  writer.WriteLine(cssContentLines[0]);
  writer.WriteLine(colorVariablesText);
  foreach (var line in cssContentLines.Skip(1))
  {
    writer.WriteLine(line);
  }
  Console.WriteLine($"Файл {newFilePath} готов.");
}
Console.ReadKey();