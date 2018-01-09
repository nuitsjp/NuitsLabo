using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class ModuleWeaver
{
    // An instance of Mono.Cecil.ModuleDefinition for processing. REQUIRED
    public ModuleDefinition ModuleDefinition { get; set; }

    public void Execute()
    {
        var type = ModuleDefinition.Types.Single(x => x.Name == "Program");
        var method = type.Methods.Single(x => x.Name == "SayHello");

        var processor = method.Body.GetILProcessor();
        var current = method.Body.Instructions.First();

        processor.InsertBefore(current, Instruction.Create(OpCodes.Nop));
        processor.InsertBefore(current, Instruction.Create(OpCodes.Ldstr, $"DEBUG: {type.Name}#{method.Name}()"));
        processor.InsertBefore(current, Instruction.Create(OpCodes.Call, ModuleDefinition.ImportReference(GetDebugWriteLine())));
    }

    private MethodInfo GetDebugWriteLine()
    {
        return typeof(System.Diagnostics.Debug)
            .GetTypeInfo()
            .DeclaredMethods
            .Where(x => x.Name == nameof(System.Diagnostics.Debug.WriteLine))
            .Single(x =>
            {
                var parameters = x.GetParameters();
                return parameters.Length == 1 &&
                       parameters[0].ParameterType == typeof(string);
            });
    }
}
