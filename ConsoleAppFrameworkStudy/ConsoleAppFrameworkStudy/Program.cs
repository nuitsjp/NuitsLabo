using ConsoleAppFrameworkStudy;

var dir = new DirectoryInfo("dir");
dir.Create();
dir.Create();

using var fileInfo = new FileInfo(@"foo\bar.txt").OpenWrite();



var app = ConsoleApp.Create(args);
app.AddCommands<FooApp>();
app.Run();