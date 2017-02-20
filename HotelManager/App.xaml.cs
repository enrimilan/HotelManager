using System.Windows;

namespace HotelManager
{
    public partial class App : Application
    {

        [System.STAThreadAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            App app = new App();
            app.InitializeComponentCustom();
            app.Run();
        }

        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponentCustom()
        {
            this.StartupUri = new System.Uri("Gui/Main.xaml", System.UriKind.Relative);
        }

    }
}
