using Api.Controllers.Payload.Requests.Items;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.ItemCategories.Commands.CreateItemCategory;
using Application.Items.Commands.AddItem;
using Application.Sizes.Commands.AddSize;
using Application.Toppings.Commands.AddTopping;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Api.Controllers
{
    public class AllInOneControllerBaser : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Result<ItemDto>>> AddItem([FromBody] AddItemRequest request)
        {
            //var categoriesCommand = new CreateItemCategoryCommand()
            //{
            //    Name = "bullshit",
            //    Categories = "cà phê,trà,trà sữa,nước ép,sinh tố,latte,matcha,bánh ngọt,bánh mặn".Split(',')
            //};

            //var resultCategories = await Mediator.Send(categoriesCommand);

            //var sizesCommand = new AddSizeCommand()
            //{
            //    SizeName = "bullshit",
            //    PriceAdjustment = 10,
            //    Sizes = "s:0,m:5,l:10,xl:15".Split(",")
            //};

            //var resultSizes = await Mediator.Send(sizesCommand);

            //var toppingsCommand = new AddToppingCommand()
            //{
            //    ToppingName = "bullshit",
            //    ToppingDescription = "bullshit",
            //    Price = 10,
            //    Toppings = "không có:0,trân trâu đen:5,trân trâu trắng:5,trân trâu vàng:5,trân trâu đỏ:5,trân trâu thâm:5,trân trâu bò:5".Split(",")
            //};

            //var resultToppings = await Mediator.Send(toppingsCommand);

            var itemsCommand = new AddItemCommand()
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Description = request.Description,
                Img = request.Img,
                SizeIds = request.SizeIds,
                AutoCreate = true
            };

            var resultItems = await Mediator.Send(itemsCommand);

            return Ok(new {item = itemsCommand});
        }

    }
}
