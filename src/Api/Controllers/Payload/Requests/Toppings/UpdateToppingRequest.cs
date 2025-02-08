namespace Api.Controllers.Payload.Requests.Toppings;

public class UpdateToppingRequest
{
    public string ToppingName { get; set; }
    public string ToppingDescription { get; set; }
    public double Price { get; set; }
}