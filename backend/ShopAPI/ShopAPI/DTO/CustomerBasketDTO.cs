﻿using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; }

        public List<BasketItemDTO> Items { get; set; }
    }
}
