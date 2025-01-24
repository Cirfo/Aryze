using System;

public static class Constants
{
    public enum ResourceType {
        Gold,
        Crystal,
        Steel,
        Army,
        Future,
        Influence,
    };

    public static int special_couns_min_cost = 3;
    public static int special_couns_max_cost = 9;
    public static int special_couns_min_res_count = 1;
    public static int special_couns_max_res_count = 4;
}
