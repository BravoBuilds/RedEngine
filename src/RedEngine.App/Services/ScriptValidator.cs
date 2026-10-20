namespace RedEngine.Services;
public static class ScriptValidator
{
    // Lightweight bracket matching only; this is not a full Lua/Luau parser.
    public static string ValidateBasic(string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var stack = new Stack<(char Open, int Position)>();
        var quote = '\0';
        var escaped = false;
        var lineComment = false;
        for (var i = 0; i < source.Length; i++)
        {
            var c = source[i];
            if (lineComment) { if (c == '\n') lineComment = false; continue; }
            if (quote != '\0')
            {
                if (escaped) { escaped = false; continue; }
                if (c == '\\') { escaped = true; continue; }
                if (c == quote) quote = '\0';
                continue;
            }
            if (c == '-' && i + 1 < source.Length && source[i + 1] == '-') { lineComment = true; i++; continue; }
            if (c is '\'' or '"') { quote = c; continue; }
            if (c is '(' or '{' or '[') { stack.Push((c, i)); continue; }
            if (c is ')' or '}' or ']')
            {
                if (stack.Count == 0) return $"Warning: unexpected closing bracket '{c}' at character {i + 1}.";
                var open = stack.Pop().Open;
                if (!Matches(open, c)) return $"Warning: bracket mismatch: '{open}' does not match '{c}' at character {i + 1}.";
            }
        }
        if (stack.Count > 0) return $"Warning: opening bracket '{stack.Peek().Open}' is not closed.";
        if (quote != '\0') return "Warning: a quoted string is not closed.";
        return "Basic checks passed (matching brackets and quoted strings only; not a full syntax check).";
    }
    private static bool Matches(char open, char close) => (open, close) is ('(', ')') or ('{', '}') or ('[', ']');
}