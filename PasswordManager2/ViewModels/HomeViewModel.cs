using PasswordManager2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; }

        public HomeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string viewName)
        {
            Console.WriteLine($"Navigating to: {viewName}");
            _regionManager.RequestNavigate("MainRegion", viewName);
        }
    }
}
