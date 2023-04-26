using DJK.Models;

namespace DJK.ViewModels
{
#nullable disable
    public static class SearchVM
    {
        public static List <Product> CurrentProduct { get; set; }

        public static string UsedSort {  get; set; }
        public static string UsedFilter { get; set; }
        public static string UsedCategory { get; set; }
        public static string UsedLocation { get; set; }

        public static string UsedSortName { get; set; }


    }
}
