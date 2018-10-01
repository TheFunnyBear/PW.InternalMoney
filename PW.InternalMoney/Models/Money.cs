using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PW.InternalMoney.Models
{
    [ComplexType]
    public sealed class Money : IEquatable<Money>
    {
        public decimal Amount { get; set; }
        public Currency SelectedCurrency { get; set; }

        public Money()
        {
            Amount = 0;
            SelectedCurrency = Currency.Pw;
        }

        public Money(decimal amount)
        {
            Amount = amount;
            SelectedCurrency = Currency.Pw;
        }

        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            SelectedCurrency = currency;
        }

        public static bool operator ==(Money firstValue, Money secondValue)
        {
            if ((object)firstValue == null || (object)secondValue == null)
                return false;

            if (firstValue.SelectedCurrency != secondValue.SelectedCurrency) return false;
            return firstValue.Amount == secondValue.Amount;
        }

        public static bool operator !=(Money firstValue, Money secondValue)
        {
            return !(firstValue == secondValue);
        }

        public static bool operator >(Money firstValue, Money secondValue)
        {
            if (firstValue.SelectedCurrency != secondValue.SelectedCurrency)
                throw new InvalidOperationException("Comparison between different currencies is not allowed.");

            return firstValue.Amount > secondValue.Amount;
        }

        public static bool operator <(Money firstValue, Money secondValue)
        {
            if (firstValue == secondValue) return false;

            return !(firstValue > secondValue);
        }

        public static bool operator <=(Money firstValue, Money secondValue)
        {
            if (firstValue < secondValue || firstValue == secondValue) return true;

            return false;
        }

        public static bool operator >=(Money firstValue, Money secondValue)
        {
            if (firstValue > secondValue || firstValue == secondValue) return true;

            return false;
        }

        public static Money operator +(Money firstValue, Money secondValue)
        {
            if (firstValue.SelectedCurrency != secondValue.SelectedCurrency)
            {
                throw new InvalidCastException("Calculation is using different currencies!");
            }

            return new Money(firstValue.Amount + secondValue.Amount, firstValue.SelectedCurrency);
        }

        public static Money operator -(Money firstValue, Money secondValue)
        {
            if (firstValue.SelectedCurrency != secondValue.SelectedCurrency)
            {
                throw new InvalidCastException("Calculation is using different currencies!");
            }

            return new Money(firstValue.Amount - secondValue.Amount, firstValue.SelectedCurrency);
        }

        public static Money operator *(Money firstValue, Money secondValue)
        {
            if (firstValue.SelectedCurrency != secondValue.SelectedCurrency)
            {
                throw new InvalidCastException("Calculation is using different currencies!");
            }

            return new Money(firstValue.Amount * secondValue.Amount, firstValue.SelectedCurrency);
        }

        public static Money operator /(Money firstValue, Money secondValue)
        {
            if (firstValue.SelectedCurrency != secondValue.SelectedCurrency)
            {
                throw new InvalidCastException("Calculation is using different currencies!");
            }

            return new Money(firstValue.Amount / secondValue.Amount, firstValue.SelectedCurrency);
        }

        public static Money ConvertToCurrency(Money sourceValue, Currency destinationCurrency, double exchangeRate)
        {
            if (sourceValue == null || exchangeRate <= 0)
                throw new InvalidCastException("Wrong amount or exchange rate");

            return new Money(sourceValue.Amount * (decimal)exchangeRate, destinationCurrency);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Money money = obj as Money;
            return (this.Amount == money.Amount && this.SelectedCurrency == money.SelectedCurrency);
        }

        public bool Equals(Money money)
        {
            if ((object)money == null) return false;

            return (this.Amount == money.Amount && this.SelectedCurrency == money.SelectedCurrency);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => string.Format("{0:0.00} {1}", Amount, SelectedCurrency.ToString().ToUpper());

        [JsonIgnore]
        public string Serialized
        {
            get { return JsonConvert.SerializeObject(this); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                var jData = JsonConvert.DeserializeObject<Money>(value);
                Amount = jData.Amount;
                SelectedCurrency = jData.SelectedCurrency;
            }
        }
    }

}