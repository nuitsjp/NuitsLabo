namespace Dapper.QueryTemplate;

public record CompilerError(string ErrorText, bool IsWarning = false);
