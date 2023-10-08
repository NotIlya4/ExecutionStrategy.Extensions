using System.Runtime.CompilerServices;

namespace ExecutionStrategy.Extensions.IntegrationTests;

public static class Initializer
{
    [ModuleInitializer]
    public static void Init() =>
        UseProjectRelativeDirectory("VerifyGenerated");
}