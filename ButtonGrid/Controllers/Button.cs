using ButtonGrid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Globalization;

namespace ButtonGrid.Controllers
{
    public class Button : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();
        Random random = new Random();
        private NumberStyles style;
        const int GRID_SIZE = 25;

        public IActionResult Index()
        {
            if(buttons.Count < GRID_SIZE)
            {
                for (int i = 0; i < GRID_SIZE; i++)
                            {
                                buttons.Add(new ButtonModel { Id = i, ButtonState = random.Next(4) });
                            }
            }
            
            
            return View("Index", buttons);
        }
        public IActionResult HandleButtonClick( string buttonNumber)
        {
            int bn = int.Parse(buttonNumber);

            buttons.ElementAt(bn).ButtonState = (buttons.ElementAt(bn).ButtonState + 1) % 4;
            
            return View("Index", buttons);
        }

        [HttpPost]
        public IActionResult ShowOneButton(int buttonNumber)
        {
            // add one to the button state. if > 4, then reset to zero
            buttons.ElementAt(buttonNumber).ButtonState = (buttons.ElementAt(buttonNumber).ButtonState + 1) % 4;

            // Render a button and Save it to a string
            string buttonString = RenderRazorViewToString(this, "ShowOneButton", buttons.ElementAt(buttonNumber));

            //Generate a win or loss string based on the state of the button array
            bool DidIWinYet = true;
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons.ElementAt(i).ButtonState != buttons.ElementAt(0).ButtonState)
              
                    DidIWinYet = false;
            }
            string messageString = "";
            if (DidIWinYet == true)
                messageString = "<p>Hurray! You Won They're all A Match</p>";
            else
                messageString = "<p>Oops! Waraburije</p>";

            // Assembly a JSON string that has two parts (button string htlm and win loss message)
            var package = new { part1 = buttonString, part2 = messageString };
            // Send JSON result
            return Json(package);

            //return PartialView(buttons.ElementAt(buttonNumber));
        }
        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine? viewEngine =
                    controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as
                        ICompositeViewEngine;

                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                if (viewResult.View != null)
                {
                    viewResult.View.RenderAsync(viewContext);
                }
                return sw.GetStringBuilder().ToString();
            }
        }
        public IActionResult RightClickShowOneButton(int buttonNumber)
        {
            // Right click reset button to zero state
            buttons.ElementAt(buttonNumber).ButtonState = 0;

            //return Json(package);

            return PartialView("ShowOneButton", buttons.ElementAt(buttonNumber));
        }
    }
}
