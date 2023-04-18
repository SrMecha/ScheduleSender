namespace ScheduleSender.Utils;

public static class EnvReader
{
    public static string? GetEnviroment(string key)
    {
        return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User) ??
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ??
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
    }
}
