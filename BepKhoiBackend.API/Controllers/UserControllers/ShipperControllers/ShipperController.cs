﻿using BepKhoiBackend.BusinessObject.dtos.UserDto.ShipperDto;
using BepKhoiBackend.BusinessObject.Services.UserService.ShipperService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BepKhoiBackend.API.Controllers.UserControllers.ShipperControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;

        public ShipperController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, Manager")]
        public ActionResult<IEnumerable<ShipperDTO>> GetAllShippers()
        {
            var shippers = _shipperService.GetAllShippers();
            if (shippers == null || shippers.Count == 0)
            {
                return NotFound("Không có shipper nào trong hệ thống.");
            }
            return Ok(shippers);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin, Manager")]
        public ActionResult<ShipperDTO> GetShipperById(int id)
        {
            var shipper = _shipperService.GetShipperById(id);
            if (shipper == null)
            {
                return NotFound($"Không tìm thấy shipper có ID: {id}");
            }
            return Ok(shipper);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin, Manager")]
        public IActionResult CreateShipper([FromBody] CreateShipperDTO newShipper)
        {
            if (newShipper == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            _shipperService.CreateShipper(newShipper.Email, newShipper.Password, newShipper.Phone, newShipper.UserName);
            return Ok("Shipper đã được tạo thành công.");
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin, Manager")]
        public IActionResult UpdateShipper(int id, [FromBody] UpdateShipperDTO updatedShipper)
        {
            if (updatedShipper == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            _shipperService.UpdateShipper(id, updatedShipper.Email, updatedShipper.Phone, updatedShipper.UserName);
            return Ok($"Shipper có ID {id} đã được cập nhật thành công.");
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteShipper(int id)
        {
            _shipperService.DeleteShipper(id);
            return Ok($"Shipper có ID {id} đã bị xóa.");
        }
        [HttpGet("{id}/invoices")]
        public ActionResult<IEnumerable<ShipperInvoiceDTO>> GetShipperInvoices(int id)
        {
            var invoices = _shipperService.GetShipperInvoices(id);
            if (invoices == null || !invoices.Any())
            {
                return NotFound($"Không có hóa đơn nào cho shipper với ID {id}.");
            }
            return Ok(new { ShipperId = id, Invoices = invoices });
        }
        [HttpGet("search")]
        public ActionResult<List<ShipperDTO>> GetShippersByNameOrPhone([FromQuery] string searchTerm)
        {
            var shippers = _shipperService.GetShippersByNameOrPhone(searchTerm);
            return Ok(shippers);

        }
        [HttpGet("status/{status}")]
        public IActionResult GetShippersByStatus(bool status)
        {
            var shippers = _shipperService.GetShipperByStatus(status);
            return Ok(shippers);
        }
    }
}
