public static class SaveMigrations
{
    public static SaveData Upgrade(SaveData data, int target)
    {
        while (data.version < target)
        {
            switch (data.version)
            {
                case 1: FromV1(data); break;
                case 2: FromV2(data); break;
            }
            data.version++;
        }
        return data;
    }

    // v1 → v2: inventory added, empty by default
    private static void FromV1(SaveData d) { /* add an empty entry */ }

    // v2 → v3: the gold field moved to the wallet
    private static void FromV2(SaveData d) { /* move the value */ }
}
