using Microsoft.EntityFrameworkCore;

namespace P03_SalesDatabase.Data.Common;

public static class Config
{
    public const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=SalesDatabase;Integrated Security=True;";
}
