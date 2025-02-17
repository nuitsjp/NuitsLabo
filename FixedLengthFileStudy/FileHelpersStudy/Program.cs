using FileHelpers;
using FileHelpersStudy;

var engine = new FileHelperEngine<Customer2>();

// To Read Use:
var content = """
              01732Juan Perez           004350011052002
              00554Pedro Gomあ          123423006022004
              00112Ramiro Politti       000000001022000
              00924Pablo Ramirez        033213024112002
              """;
var result = engine.ReadString(content);
Console.WriteLine(result[0].Name); // Juan Perez