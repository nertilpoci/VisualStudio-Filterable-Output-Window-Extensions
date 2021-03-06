﻿using FilteredOutputWindowVSX.Enums;
using FilteredOutputWindowVSX.ViewModels;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace FilteredOutputWindowVSX.Models
{
    public class StringFilterItem : ObservableObject
    {
        private string _value;

        public string Value { get => _value ?? string.Empty; set => Set(ref _value, value); }

        public StringOperation Operation { get; set; }
        [JsonIgnore]
        public Expression<Func<string, bool>> Expression => BuildExpression();
        private Expression<Func<string, bool>> BuildExpression()
        {
            var predicate = PredicateBuilder.True<string>();

            switch (Operation)
            {
                case StringOperation.Contains:
                    predicate = predicate.And(input => input.Contains(Value));
                    break;
                case StringOperation.StartsWith:
                    predicate = predicate.And(input => input.StartsWith(Value, StringComparison.InvariantCultureIgnoreCase));
                    break;
                case StringOperation.EndsWith:
                    predicate = predicate.And(input => input.EndsWith(Value, StringComparison.InvariantCultureIgnoreCase));
                    break;
                case StringOperation.Regex:
                    predicate = predicate.And(input => Regex.IsMatch(input, Value, RegexOptions.Multiline));
                    break;
                default:
                    break;
            }
            return predicate;
        }

        public override string ToString()
        {
            return $"{Operation.ToString() } { Value }";
        }
    }
}
