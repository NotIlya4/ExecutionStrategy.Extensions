namespace ExecutionStrategyExtended.Demo.Models;

public record User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public bool IsDeleted { get; private set; }

    public User(int id, string name, bool isDeleted)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
    }
    
#pragma warning disable CS8618
    protected User() { }
#pragma warning restore CS8618
}