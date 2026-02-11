namespace UI.CustomPageContainer
{
    public interface IPageFlipper
    {
        bool IsFirstPage { get; }

        bool IsLastPage { get; }

        int CurrentPage { get; }

        void ShowNextPage();

        void ShowPreviousPage();
    }
}