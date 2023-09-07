using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;

namespace CRUDelicious.Controllers;

public class DishController : Controller
{
    private readonly ILogger<DishController> _logger;

    private MyContext _context;

    public DishController(ILogger<DishController> logger, MyContext context)
    {
        _logger = logger;
        _context = context; //how you access your db in the controller
    }



//route for home page that also serves as view all dishes
    [HttpGet("")]
    public ViewResult AllDishes()
    {
        List<Dish> Dishes = _context.Dishes.OrderByDescending(d => d.CreatedAt).ToList();
        return View(Dishes); //do not have to indicate all dishes since its the same, but passing in our Dishes model
    }



//route for creating a new dish
    [HttpGet("dishes/new")]
    public ViewResult NewDish()
    {
        return View(); //this only has to render an empty form
    }



//route that will post/create that new dish into our DB
    [HttpPost("dishes/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if (!ModelState.IsValid) //confirming this passes our validations 
        {
            return View("NewDish");
        }
        _context.Add(newDish);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }



//route that will show view one page
    [HttpGet("dishes/{dishId}")]
    public IActionResult ViewDish(int dishId) //since we have a parameter, we need to make sure we bring it in
    {
        Dish? SingleDish = _context.Dishes.FirstOrDefault(d => d.DishId == dishId); //firstordefault so that it does not crash if it does not find, sends null instead
                                                                //making our DishId from our model key == to our parameter 
        if (SingleDish == null){
            return RedirectToAction(""); //if they click on something that does not exit, send them back home
        }
        return View(SingleDish); //if exists, take them to page for that selection
    } 



//route to view edit page
    [HttpGet("dishes/{dishId}/edit")]
    public IActionResult EditDish(int dishId)
    {
        Dish? ToBeEdited = _context.Dishes.FirstOrDefault(d => d.DishId == dishId); 
        if (ToBeEdited == null)
        {
            return RedirectToAction(""); //if they manually edited URL or something we we got a null item
        }
        return View(ToBeEdited); //passing this in so that it can be our editing model in the viewing page
    }



//route to process view edit changes
    [HttpPost("dishes/{dishID}edit")]
    public IActionResult UpdateDish(int dishId, Dish editedDish) //also bringing in Dish model that is edited as a parameter
    {
        Dish? ToBeUpdated = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
        if(!ModelState.IsValid || ToBeUpdated == null) //if any validation errors or if we did not find id (if someone found  away to edit on URL)
        {
            return View("EditDish", editedDish); //pass in the model/object (can do ToBeUdpated + add value tags in edit form for each one, OR editDish which keeps whatever changes they made )
        } //otherwise updated all keys
        ToBeUpdated.Chef = editedDish.Chef;
        ToBeUpdated.DishName = editedDish.DishName;
        ToBeUpdated.Calories = editedDish.Calories;
        ToBeUpdated.Tastiness = editedDish.Tastiness;
        ToBeUpdated.Description = editedDish.Description;
        ToBeUpdated.UpdatedAt = DateTime.Now;

        _context.SaveChanges();

        return RedirectToAction("ViewDish", new {dishId=dishId}); //since this route has parameter, add object to take this in
    }




//route that will delete a selected one
    [HttpPost("dishes/{dishID}/delete")]
    public IActionResult DeleteDish (int dishId) //taking in param from path variable
    { //looking for dish
        Dish? ToBeDeleted = _context.Dishes.SingleOrDefault(d => d.DishId == dishId); //checking if db id is same as route id
        if (ToBeDeleted != null) //this means we found a match/post
        {
            _context.Remove(ToBeDeleted); //if it is not null, means we found it and can delete
            _context.SaveChanges();
        }
        return RedirectToAction(""); //if it is null, then we can just reroute to home page since we did not find a match
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
