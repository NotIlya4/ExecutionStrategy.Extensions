﻿namespace ExecutionStrategyExtended.UnitTests.PostgresBootstrapping.FluentDocker;

public record FluentDockerOptions
{
    public PostgresContainerOptions PostgresContainerOptions { get; set; } = new();
    public OnCleanType OnClean { get; set; } = OnCleanType.RecreateContainer;
    
    public enum OnCleanType
    {
        RecreateContainer,
        RecreateDb,
        CleanTables
    }
}