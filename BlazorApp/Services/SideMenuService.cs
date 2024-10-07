namespace BlazorApp.Services
{
    public class SideMenuService
    {
        public bool IsContactsExpanded { get; private set; }

        public void ToggleContactsMenu()
        {
            IsContactsExpanded = !IsContactsExpanded;
        }
    }

}
