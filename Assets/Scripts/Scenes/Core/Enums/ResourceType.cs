[System.Flags]
public enum ResourceType
{
    None = 1 << 0,
    ResourceOne = 1 << 1,
    ResourceTwo = 1 << 2,
    BothResource = ResourceOne | ResourceTwo
}
