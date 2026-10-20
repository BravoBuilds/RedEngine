namespace RedEngine.Services;
public static class ScriptValidator
{
    // Basic heuristic only; this is not a full Lua/Luau parser.
    public static string ValidateBasic(string source)
    {
        var opens = 0;
        foreach (var c in source)
        {
            if (c is '(' or '{' or '[') opens++;
            else if (c is ')' or '}' or ']') opens--;
            if (opens < 0) return "Warning: closing bracket appears before a matching opening bracket.";
        }
        return opens == 0 ? "Basic checks passed (balanced bracket count only; not a full syntax check)." : "Warning: bracket count does not balance.";
    }
}