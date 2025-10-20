using System;
using System.Collections.Generic;
using System.Text;

namespace MMABooksBusinessClasses
{
    public class Product
    {
        private string _productCode = string.Empty;
        private string _description = string.Empty;
        private int _onHandQuantity;
        private decimal _unitPrice;

        // Parameterless ctor
        public Product() { }

        // Properties
        public string ProductCode
        {
            get => _productCode;
            set
            {
                var s = (value ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(s))
                    throw new ArgumentException("ProductCode is required.");
                if (s.Length > 20)
                    throw new ArgumentException("ProductCode cannot exceed 20 characters.");
                _productCode = s;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                var s = (value ?? string.Empty).Trim();
                if (s.Length > 200) throw new ArgumentException("Description is too long.");
                _description = s;
            }
        }

        public int OnHandQuantity
        {
            get => _onHandQuantity;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(OnHandQuantity), "Quantity cannot be negative.");
                _onHandQuantity = value;
            }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (value < 0m) throw new ArgumentOutOfRangeException(nameof(UnitPrice), "UnitPrice cannot be negative.");
                if (decimal.Round(value, 2) != value) throw new ArgumentException("UnitPrice can have at most 2 decimal places.");
                _unitPrice = value;
            }
        }

        public override string ToString()
        {
            return $"{ProductCode} - {Description} | Qty: {OnHandQuantity} | Price: {UnitPrice:C}";
        }
    }
}

