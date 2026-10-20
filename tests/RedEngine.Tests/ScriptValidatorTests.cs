using RedEngine.Services;
using Xunit;
namespace RedEngine.Tests;
public class ScriptValidatorTests
{
    [Fact] public void BalancedBracketsPass() => Assert.StartsWith("Basic checks passed", ScriptValidator.ValidateBasic("local x = { a = (1) }"));
    [Fact] public void UnbalancedBracketsWarn() => Assert.StartsWith("Warning:", ScriptValidator.ValidateBasic("print((1)"));
    [Fact] public void ExtraClosingBracketWarn() => Assert.StartsWith("Warning:", ScriptValidator.ValidateBasic("print(1))"));
}