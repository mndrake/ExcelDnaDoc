﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ExcelDna.Integration;

namespace ExcelDnaDoc.Utility
{
    public static class ExcelDnaHelper
    {
        // based on https://exceldna.codeplex.com/wikipage?title=Reference

        public static bool IsValidExcelDnaType(Type type)
        {
            var validTypes = 
                new Type[] {typeof(String), typeof(DateTime), typeof(Double), typeof(Double[]), typeof(Double[,]),
                            typeof(Object), typeof(Object[]), typeof(Object[,]), typeof(Boolean), typeof(Int32),
                            typeof(Int16), typeof(UInt16), typeof(Decimal), typeof(Int64)};

            return validTypes.Any(t => type.Equals(t));
        }

        public static bool IsValidFunction(MethodInfo method, bool explicitExports)
        {
            if (!(method.IsPublic && method.IsStatic))
            {
                return false;
            }

            if (explicitExports)
            {
                return Attribute.IsDefined(method, typeof(ExcelFunctionAttribute));
            }

            ParameterInfo[] parameters = method.GetParameters();
            return (parameters.All(p => IsValidExcelDnaType(p.ParameterType))) && IsValidExcelDnaType(method.ReturnType);
        }

        public static bool IsValidCommand(MethodInfo method)
        {
            if (!(method.IsPublic && method.IsStatic))
            {
                return false;
            }

            return Attribute.IsDefined(method, typeof(ExcelCommandAttribute));
        }
    }
}
