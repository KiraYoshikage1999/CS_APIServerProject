using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CS_APIServerProject.Utils
{
    public static class ManualMapper
    {
        public static Product ToProduct(ProductCreateDTO dto)
        {
            if (dto == null) return null!;
            return new Product
            {
                Brand = dto.Brand,
                Model = dto.Model,
                Description = dto.Description,
                Price = dto.Price,
                Quanity = dto.Quanity,
                Currency = dto.Currency,
                Characteristics = dto.Characteristics == null ? null : ToCharacteristics(dto.Characteristics),
                imageCode = dto.ImageCode,
                CreatedAt = dto.CreatedAt
            };
        }

        public static void UpdateOrder(OrderUpdateDTO dto, Order entity)
        {
            if (dto == null || entity == null) return;
            entity.CustomerId = dto.CustomerId;
            entity.Status = dto.Status;
            entity.Number = dto.Number;
            // Items merge/update logic omitted intentionally
        }

        public static void UpdateProduct(ProductUpdateDTO dto, Product entity)
        {
            if (dto == null || entity == null) return;
            entity.Brand = dto.Brand;
            entity.Model = dto.Model;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.Quanity = dto.Quanity;
            entity.Currency = dto.Currency;
            if (dto.Characteristics != null)
            {
                entity.Characteristics ??= new Characteristics();
                entity.Characteristics.state = dto.Characteristics.state;
                entity.Characteristics.typeGas = dto.Characteristics.typeGas;
                entity.Characteristics.milege = dto.Characteristics.milege;
                entity.Characteristics.typeMilege = dto.Characteristics.typeMilege;
                entity.Characteristics.typeBody = dto.Characteristics.typeBody;
                entity.Characteristics.Color = dto.Characteristics.Color;
                entity.Characteristics.DriveType = dto.Characteristics.DriveType;
                entity.Characteristics.Engine = dto.Characteristics.Engine;
            }
        }

        public static ProductReadDTO ToProductReadDTO(Product p)
        {
            if (p == null) return null!;
            return new ProductReadDTO
            {
                Id = p.Id,
                Brand = p.Brand,
                Model = p.Model,
                Description = p.Description,
                Price = p.Price,
                Quanity = p.Quanity,
                Currency = p.Currency,
                Characteristics = p.Characteristics == null ? null : ToCharacteristicsDTO(p.Characteristics),
                ImageCode = p.imageCode,
                CreatedAt = p.CreatedAt
            };
        }

        public static Characteristics ToCharacteristics(CharacteristicsDTO dto)
        {
            if (dto == null) return null!;
            return new Characteristics
            {
                state = dto.state,
                typeGas = dto.typeGas,
                milege = dto.milege,
                typeMilege = dto.typeMilege,
                typeBody = dto.typeBody,
                Color = dto.Color,
                DriveType = dto.DriveType,
                Engine = dto.Engine
            };
        }

        public static CharacteristicsDTO ToCharacteristicsDTO(Characteristics m)
        {
            if (m == null) return null!;
            return new CharacteristicsDTO(m.state, m.typeGas, m.milege, m.typeMilege, m.typeBody, m.Color, m.DriveType, m.Engine);
        }

        public static Order ToOrder(OrderCreateDTO dto)
        {
            if (dto == null) return null!;
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                Status = dto.Status,
                Number = dto.Number,
                CreatedAt = dto.CreatedAt,
                Items = dto.Items?.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Product = i.Product == null ? null : ToProduct(i.Product),
                    Quanity = i.Quanity,
                    Price = i.Price
                }).ToList()
            };
            return order;
        }

        public static OrderReadDTO ToOrderReadDTO(Order o)
        {
            if (o == null) return null!;
            return new OrderReadDTO
            {
                CustomerId = o.CustomerId,
                Status = o.Status,
                Number = o.Number,
                CreatedAt = o.CreatedAt,
                Items = o.Items?.Select(it => new OrderItemReadDTO
                {
                    OrderId = it.OrderId,
                    Order = null,
                    ProductId = it.ProductId,
                    Product = it.Product == null ? null : ToProductReadDTO(it.Product),
                    Quanity = it.Quanity,
                    Price = it.Price
                }).ToList()
            };
        }

        public static User ToUser(UserCreateDTO dto)
        {
            if (dto == null) return null!;
            return new User
            {
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Name = dto.Name,
                LastName = dto.LastName
            };
        }

        public static UserReadDTO ToUserReadDTO(User u)
        {
            if (u == null) return null!;
            return new UserReadDTO(u.Id, u.DateOfBirth, u.PhoneNumber, u.Email, u.Name, u.LastName);
        }

        public static void UpdateUser(UserUpdateDTO dto, User entity)
        {
            if (dto == null || entity == null) return;
            entity.DateOfBirth = dto.DateOfBirth;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Email = dto.Email;
            entity.Name = dto.Name;
            entity.LastName = dto.LastName;
        }
    }
}
