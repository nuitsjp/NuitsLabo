using FlatFile.FixedLength.Implementation;
using System.Text;

const string _testSource = @"000あDescription 1            00003
00002Description 2            00003
00003Description 3            00003
00004Description 4            00003
00005Description 5            =Null
00006Description 6            00003
00007Description 7            00003
00008Description 8            00003
00009Description 9            00003
00010Description 10           =Null"; 

var layout = new FixedSampleRecordLayout();
var factory = new FixedLengthFileEngineFactory();
using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_testSource));
var flatFile = factory.GetEngine(layout);

var records = flatFile.Read<TestObject>(stream).ToArray();
Console.WriteLine("Completed.");

public class TestObject
{
    public string? Cuit { get; set; }
    public string? Nombre { get; set; }
    public string? Actividad { get; set; }
}

public sealed class FixedSampleRecordLayout : FixedLayout<TestObject>
{
    public FixedSampleRecordLayout()
    {
        this
            .WithMember(o => o.Cuit, set => set.WithLength(5).WithLeftPadding('0'))
            .WithMember(o => o.Nombre, set => set.WithLength(25).WithRightPadding(' '))
            .WithMember(o => o.Actividad, set => set.WithLength(5).AllowNull("=Null").WithLeftPadding('0'));
    }
}