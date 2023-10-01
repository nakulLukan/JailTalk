namespace JailTalk.Web.Helpers;

public class UIHelper
{
    public static int GetNumberOfPages(int totalRecords, int pageSize)
    {
        return (int)Math.Ceiling(totalRecords / (double)pageSize);
    }
    public static int GetPageSkipValue(int selectedPage, int pageSize)
    {
        return (selectedPage - 1) * pageSize;
    }
}
